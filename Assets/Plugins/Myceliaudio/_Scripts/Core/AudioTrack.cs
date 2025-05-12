using System.Collections;
using UnityEngine;
using CGT.Audio;

namespace CGT.Myceliaudio
{
    /// <summary>
    /// Helper class for that also kind of wraps Unity's built-in AudioSource component
    /// </summary>
    public class AudioTrack : IAudioTrackTweenables
    {
        public virtual int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    GameObject.name = $"Track_{ID:D3}";
                }
            }
        }

        protected int _id;

        public virtual void Init(GameObject parent)
        {
            GameObject = new GameObject($"Track_{ID:D3}");
            GameObject.transform.SetParent(parent.transform, false);

            SetUpAudioSource();
        }

        public GameObject GameObject { get; protected set; } // Might help with custom tweens

        protected virtual void SetUpAudioSource()
        {
            _playsIntros = GameObject.AddComponent<AudioSource>();
            _playsLoops = GameObject.AddComponent<AudioSource>();
            _playsIntros.playOnAwake = _playsLoops.playOnAwake = false;
            _playsIntros.volume = _playsLoops.volume = RealVolumeNormalized;

            _playsLoops.loop = true;
            // ^Since we use this to play the loop segments of clips that have a loop start point
        }

        protected AudioSource _playsIntros, _playsLoops;

        public virtual TrackManager Anchor
        {
            get { return _anchor; }
            set
            {
                TrackManager prevAnchor = _anchor;
                if (prevAnchor != null)
                {
                    prevAnchor.RealVolumeChanged += OnAnchorRealVolChanged;
                }

                _anchor = value;
                if (_anchor != null)
                {
                    _anchor.RealVolumeChanged += OnAnchorRealVolChanged;
                }

                UpdateCurrentVolApplied();
            }
        }

        protected TrackManager _anchor;

        public virtual void OnAnchorRealVolChanged(float newVolScale)
        {
            UpdateCurrentVolApplied();
        }

        protected virtual void UpdateCurrentVolApplied()
        {
            CurrentVolumeApplied = RealVolume;
        }

        /// <summary>
        /// On a scale of 0 to 100. 0 = total silence, 100 = max vol
        /// </summary>
        public virtual float CurrentVolumeApplied
        {
            get { return _playsIntros.volume * AudioMath.VolumeConversion; }
            protected set
            {
                // Need to convert to a scale of 0 to 1 since that's what the base
                // audio sources prefer
                float normalizedForAudioSource = value / AudioMath.VolumeConversion;
                float withinLimits = Mathf.Clamp(normalizedForAudioSource, AudioMath.MinVol, AudioMath.MaxVolNormalized);
                _playsIntros.volume = _playsLoops.volume = withinLimits;
            }
        }

        public virtual float RealVolume
        {
            get
            {
                float result = BaseVolume;

                if (Anchor != null)
                {
                    result *= Anchor.RealVolumeNormalized;
                }

                return result;
            }
        }

        public virtual float BaseVolume
        {
            get { return _baseVolume; }
            set
            {
                _baseVolume = value;
                UpdateCurrentVolApplied();
            }
        }

        protected float _baseVolume = 100f;

        protected virtual float RealVolumeNormalized
        {
            get { return RealVolume / AudioMath.VolumeConversion; }
        }

        public virtual void Play(PlayAudioArgs args)
        {
            _playsIntros.Stop();
            //_baseSource.loop = args.Loop;
            _playsLoops.loop = args.Loop;
            _playsIntros.clip = args.Clip;

            if (args.Loop)
            {
                if (_playOnLoop != null)
                {
                    AudioSys.StopCoroutine(_playOnLoop);
                }

                _playOnLoop = PlayOnLoopCoroutine(args);
                AudioSys.StartCoroutine(_playOnLoop);
            }
            else
            {
                _playsIntros.Play();
            }
        }

        protected IEnumerator _playOnLoop;
        // ^Need this as a separate object to avoid loop points getting confused when
        // switching from one song to another

        protected IEnumerator PlayOnLoopCoroutine(PlayAudioArgs args)
        {
            AudioClip baseClip = args.Clip;
            bool loopTheEntireSong = args.LoopStartPoint <= 0 && !args.HasEndPointBeforeEndOfClip;

            if (loopTheEntireSong)
            {
                // No need to do any fancy file-splitting in this case
                _playsLoops.clip = baseClip;
                _playsLoops.Play();
                yield break;
            }

            PlayBasedOnSplitClips();
            void PlayBasedOnSplitClips()
            {
                double loopStartPoint, loopEndPoint;
                FindLoopPoints();
                void FindLoopPoints()
                {
                    // The loop points in the args are in milliseconds, and as we need to
                    // work with points in seconds instead...
                    loopStartPoint = args.LoopStartPoint / 1000.0;
                    loopEndPoint = baseClip.PreciseLength();
                    // ^Since by default, end points are at the exact end of the audio clip
                    if (args.HasEndPointBeforeEndOfClip)
                    {
                        loopEndPoint = args.LoopEndPoint / 1000.0;
                    }
                }

                AudioClip intro = null, loopSegment;
                bool weHaveAnIntroToPlay;
                FetchSplitClips();
                
                void FetchSplitClips()
                {
                    weHaveAnIntroToPlay = loopStartPoint > 0 & args.HasEndPointBeforeEndOfClip;
                    // ^Can't have an intro without a loop segment. 

                    if (weHaveAnIntroToPlay)
                    {
                        intro = AudioClipSplitter.S.GetIntro(baseClip, loopStartPoint);
                    }

                    loopSegment = AudioClipSplitter.S.GetLoop(baseClip, loopStartPoint, loopEndPoint);
                }

                _playsIntros.clip = intro;
                _playsLoops.clip = loopSegment;

                PlayTheRightClips();
                void PlayTheRightClips()
                {
                    if (weHaveAnIntroToPlay)
                    {
                        _playsIntros.Play();

                        SetLoopToPlayRightAfterIntro();
                        void SetLoopToPlayRightAfterIntro()
                        {
                            double slightBuffer = 0.00;
                            double afterTheFirstPlayIsDone = AudioSettings.dspTime +
                                _playsIntros.clip.PreciseLength() + slightBuffer;
                            _playsLoops.PlayScheduled(afterTheFirstPlayIsDone);
                        }
                    }
                    else
                    {
                        _playsLoops.Play();
                        // ^This would mean that the only cutoff is with the end point, and thus that
                        // the start point is at the exact beginning of the song.
                    }
                }
                
            }


            yield break;
        }

        protected AudioClip CopyAudioClip(AudioClip originalClip, double clipLengthInSeconds, double startTimeInSeconds = 0)
        {
            if (originalClip == null)
            {
                Debug.LogError("Original AudioClip is null. Cannot create a copy.");
                return null;
            }

            // Calculate the starting sample based on the start time
            int startSample = (int)(startTimeInSeconds * originalClip.frequency);

            // Calculate the number of samples for the new clip
            int sampleCount = (int)(clipLengthInSeconds * originalClip.frequency);

            // Ensure we don't exceed the original clip's sample count
            sampleCount = Mathf.Min(sampleCount, originalClip.samples - startSample);

            if (sampleCount <= 0)
            {
                Debug.LogError("Invalid start time or clip length. No samples to copy.");
                return null;
            }

            // Create a new AudioClip with the same properties as the original
            AudioClip newClip = AudioClip.Create(
                originalClip.name + "_Copy",
                sampleCount,
                originalClip.channels,
                originalClip.frequency,
                false // Non-streaming
            );

            // Retrieve the audio data from the original clip
            float[] originalAudioData = new float[originalClip.samples * originalClip.channels];
            originalClip.GetData(originalAudioData, 0);

            // Copy only the relevant portion of the audio data
            float[] newAudioData = new float[sampleCount * originalClip.channels];
            int startIndex = startSample * originalClip.channels;
            int endIndex = startIndex + newAudioData.Length;

            for (int i = 0, j = startIndex; j < endIndex; i++, j++)
            {
                newAudioData[i] = originalAudioData[j];
            }

            // Set the data for the new clip
            newClip.SetData(newAudioData, 0);

            return newClip;
        }




        protected virtual AudioSystem AudioSys { get { return AudioSystem.S; } }

        public virtual void PlayOneShot(PlayAudioArgs args)
        {
            PlayOneShot(args.Clip);
        }

        public virtual void PlayOneShot(AudioClip clip)
        {
            _playsIntros.PlayOneShot(clip);
        }

        public virtual void Stop()
        {
            if (_playOnLoop != null)
            {
                AudioSys.StopCoroutine(_playOnLoop);
                _playOnLoop = null;
            }

            _playsIntros.Stop();
        }

        /// <summary>
        /// The clip playing on loop, as opposed to one shots
        /// </summary>
        public virtual AudioClip ClipPlaying
        {
            get
            {
                if (_playsIntros.isPlaying)
                {
                    return _playsIntros.clip;
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
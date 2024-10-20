using System.Collections;
using UnityEngine;

namespace CGT.Myceliaudio
{
    /// <summary>
    /// Helper class for NeoNeoAudioSys that also kind of wraps Unity's built-in AudioSource component
    /// </summary>
    public class AudioTrack
    {
        public virtual int ID { get; set; }

        public virtual void Init(GameObject toWorkWith)
        {
            holdsSource = toWorkWith;
            SetUpAudioSource();
        }

        protected GameObject holdsSource; // So we can pull off tweens

        protected virtual void SetUpAudioSource()
        {
            _baseSource = holdsSource.AddComponent<AudioSource>();
            _baseSource.playOnAwake = false;
            _baseSource.volume = EffVolScaleNormalized;
        }

        protected AudioSource _baseSource;

        public virtual TrackManager Anchor
        {
            get { return _anchor; }
            set
            {
                TrackManager prevAnchor = _anchor;
                if (prevAnchor != null)
                {
                    prevAnchor.EffVolScaleChanged += OnAnchorEffVolChanged;
                }

                _anchor = value;
                if (_anchor != null)
                {
                    _anchor.EffVolScaleChanged += OnAnchorEffVolChanged;
                }
            }
        }

        protected TrackManager _anchor;

        public virtual void OnAnchorEffVolChanged(float newVolScale)
        {
            UpdateCurrentVol();
        }

        protected virtual void UpdateCurrentVol()
        {
            CurrentVolume = EffVolScale;
        }

        /// <summary>
        /// On a scale of 0 to 100. 0 = total silence, 100 = max vol
        /// </summary>
        public virtual float CurrentVolume
        {
            get { return _baseSource.volume * AudioMath.VolumeConversion; }
            protected set
            {
                // Need to convert to a scale of 0 to 1 since that's what the base
                // audio sources prefer
                float normalizedForAudioSource = value / AudioMath.VolumeConversion;
                float withinLimits = Mathf.Clamp(normalizedForAudioSource, AudioMath.MinVol, AudioMath.MaxVolNormalized);
                _baseSource.volume = withinLimits;
            }
        }

        public virtual float EffVolScale
        {
            get
            {
                float result = BaseVolScale;

                if (Anchor != null)
                {
                    result *= Anchor.EffVolScaleNormalized;
                }

                return result;
            }
        }

        public virtual float BaseVolScale
        {
            get { return _baseVolScale; }
            set
            {
                _baseVolScale = value;
                UpdateCurrentVol();
            }
        }

        protected float _baseVolScale = 100f;

        protected virtual float EffVolScaleNormalized
        {
            get { return EffVolScale / AudioMath.VolumeConversion; }
        }

        public virtual void Play(PlayAudioArgs args)
        {
            if (args.Loop)
            {
                _baseSource.Stop();
                _baseSource.loop = true; // To avoid issues for when the end point is at the exact end of the song

                if (_playOnLoop != null)
                {
                    AudioSys.StopCoroutine(_playOnLoop);
                }

                _playOnLoop = PlayOnLoopCoroutine(args);
                AudioSys.StartCoroutine(_playOnLoop);
            }
            else
            {
                _baseSource.PlayOneShot(args.Clip);
            }
        }

        protected IEnumerator _playOnLoop;
        // ^Need this as a separate object to avoid loop points getting confused when
        // switching from one song to another

        protected IEnumerator PlayOnLoopCoroutine(PlayAudioArgs args)
        {
            AudioClip clip = _baseSource.clip = args.Clip;
            _baseSource.Play();

            // Since the base loop point is in milliseconds...
            float loopPoint = (float)(args.LoopStartPoint / 1000.0);
            // ^AudioSource.time is a float, not a double, so...

            double clipLength = clip.PreciseLength();
            if (args.HasLoopEndPoint)
            {
                clipLength = args.LoopEndPoint / 1000.0;
            }

            double lengthOfTheLoopSegment = clipLength - loopPoint;
            double whenToReturnToLoopPoint = AudioSettings.dspTime + clipLength;

            while (true)
            {
                bool shouldReturnToLoopPoint = AudioSettings.dspTime >= whenToReturnToLoopPoint;

                if (shouldReturnToLoopPoint)
                {
                    _baseSource.time = loopPoint;
                    whenToReturnToLoopPoint += lengthOfTheLoopSegment;
                }
                
                yield return null;
            }
        }

        protected virtual AudioSystem AudioSys { get { return AudioSystem.S; } }

        public virtual void Stop()
        {
            if (_playOnLoop != null)
            {
                AudioSys.StopCoroutine(_playOnLoop);
                _playOnLoop = null;
            }

            _baseSource.Stop();
        }

    }
}
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Myceliaudio
{
    /// <summary>
    /// Helper class for NeoNeoAudioSys that also kind of wraps Unity's built-in AudioSource component
    /// </summary>
    public class AudioTrack
    {
        public virtual int ID { get; set; }
        public virtual float EffVolScale
        {
            get { return _effVolScale; }
            set
            {
                _effVolScale = Mathf.Clamp(value, AudioMath.MinVol, AudioMath.MaxVol);
                UpdateCurrentVol();
            }
        }

        protected float _effVolScale = 100f;

        protected virtual void UpdateCurrentVol()
        {
            CurrentVolume = _latestTargetVolume * EffVolScaleNormalized;
        }

        protected virtual float EffVolScaleNormalized
        {
            get { return EffVolScale / AudioMath.VolumeConversion; }
        }

        protected AudioSource baseSource;

        public virtual void Init(GameObject toWorkWith = null)
        {
            holdsSource = toWorkWith;
            SetUpAudioSource();
        }

        protected GameObject holdsSource; // So we can pull off tweens

        protected virtual void SetUpAudioSource()
        {
            baseSource = holdsSource.AddComponent<AudioSource>();
            baseSource.playOnAwake = false;
            baseSource.volume = EffVolScaleNormalized;
        }

        public virtual void Play(IAudioArgs args)
        {
            if (!args.WantsClipPlayed)
                return;

            if (args.WantsFade) // The only fade we'll consider is for volume
            {
                args.OnComplete = SetBeforeOnComplete(args, PlayASAP);
                args.OnComplete = SetBeforeOnComplete(args, UpdateSettings);
                // ^ They're in this order so that the settings are updated right
                // after the fade and right before the sound-playing
                FadeVolume(args);
            }
            else
            {
                UpdateSettings(args);
                PlayASAP(args);
                args.OnComplete(args);
            }
        }

        public virtual void Play(PlayAudioArgs args)
        {
            if (args.Loop)
            {
                baseSource.Stop();
                baseSource.loop = true; // to avoid issues for when the end point is at the exact end of the song

                if (_playOnLoop != null)
                {
                    AudioSys.StopCoroutine(_playOnLoop);
                }

                _playOnLoop = PlayOnLoopCoroutine(args);
                AudioSys.StartCoroutine(_playOnLoop);
            }
            else
            {
                baseSource.PlayOneShot(args.Clip);
            }
        }

        /// <returns>
        /// A version of the args' OnComplete that has the passed toExecute
        /// executing first
        /// </returns>
        protected virtual AudioHandler SetBeforeOnComplete(IAudioArgs args,
            AudioHandler toExecute)
        {
            AudioHandler origOnComplete = args.OnComplete;
            AudioHandler result = (IAudioArgs maybeOtherArgs) =>
            {
                toExecute(maybeOtherArgs);
                origOnComplete(maybeOtherArgs);
            };

            return result;
        }

        protected AudioClip Clip
        {
            get { return baseSource.clip; }
            set { baseSource.clip = value; }
        }

        protected virtual void UpdateSettings(IAudioArgs args)
        {
            if (args.WantsVolumeSet && !tweeningVolume)
            {
                SetVolumeImmediate(args.TargetVolume);
            }

            if (args.WantsPitchSet && !tweeningPitch)
            {
                SetPitchWithoutDelay(args);
            }

            if (args.WantsPlayAtNewTime)
            {
                AtTime = args.AtTime;
                // ^ May be inaccurate if the audio source is compressed http://docs.unity3d.com/ScriptReference/AudioSource-time.html BK
            }

            bool wantsToPlayNewClipOnLoop = args.Loop && args.WantsClipPlayed && Clip != args.Clip;
            if (wantsToPlayNewClipOnLoop)
            {
                Clip = args.Clip;
            }
        }

        protected float _latestTargetVolume = 100f;
        protected bool tweeningVolume, tweeningPitch;

        public virtual void SetVolumeImmediate(float targVol)
        {
            _latestTargetVolume = targVol;
            UpdateCurrentVol();
        }

        protected virtual void SetPitchWithoutDelay(IAudioArgs args)
        {
            CurrentPitch = args.TargetPitch;
        }

        protected virtual void PlayASAP(IAudioArgs args)
        {
            if (args.Loop)
            {
                baseSource.Stop();
                baseSource.loop = true; // to avoid issues for when the end point is at the exact end of the song

                if (_playOnLoop != null)
                {
                    AudioSys.StopCoroutine(_playOnLoop);
                }

                _playOnLoop = PlayOnLoopCoroutine(args);
                AudioSys.StartCoroutine(_playOnLoop);
            }
            else
            {
                baseSource.PlayOneShot(args.Clip);
            }
        }

        protected IEnumerator _playOnLoop;
        // ^Need this as a separate object to avoid loop points getting confused when
        // switching from one song to another
        protected virtual AudioSystem AudioSys { get {  return AudioSystem.S; } }
        protected IEnumerator PlayOnLoopCoroutine(IAudioArgs args)
        {
            AudioClip clip = baseSource.clip = args.Clip;
            baseSource.Play();
            
            // Since the base loop point is in milliseconds...
            float loopPoint = (float) (args.LoopStartPoint / 1000.0); 
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
                    baseSource.time = loopPoint;
                    whenToReturnToLoopPoint += lengthOfTheLoopSegment;
                }

                yield return null;
            }
        }

        protected static double dspTimeScheduleOffset = 0.2;

        protected IEnumerator PlayOnLoopCoroutine(PlayAudioArgs args)
        {
            AudioClip clip = baseSource.clip = args.Clip;
            baseSource.Play();

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
                    baseSource.time = loopPoint;
                    whenToReturnToLoopPoint += lengthOfTheLoopSegment;
                }

                yield return null;
            }
        }

        /// <summary>
        /// On a scale of 0 to 100. 0 = total silence, 100 = max vol
        /// </summary>
        public virtual float CurrentVolume
        {
            get { return baseSource.volume * AudioMath.VolumeConversion; }
            protected set
            {
                // Need to convert to a scale of 0 to 1 since that's what the base
                // audio sources prefer
                float normalizedForAudioSource = value / AudioMath.VolumeConversion;
                float withinLimits = Mathf.Clamp(normalizedForAudioSource, AudioMath.MinVol, AudioMath.MaxVolNormalized);
                baseSource.volume = withinLimits;
            }
        }

        /// <summary>
        /// On a scale of 0 to 200. 0 = min, 100 = base pitch, 200 = double the base pitch
        /// </summary>
        public virtual float CurrentPitch
        {
            get { return baseSource.pitch * AudioMath.VolumeConversion; }
            protected set
            {
                float normalizedForAudioSource = value / AudioMath.VolumeConversion;
                float withinLimits = Mathf.Clamp(normalizedForAudioSource, AudioMath.MinPitch,
                    AudioMath.MaxPitchNormalized); // To cap things at 2x pitch
                baseSource.pitch = withinLimits;
            }
        }

        public virtual void FadeVolume(IAudioArgs args)
        {
            float preFinalTargVol = args.TargetVolume * EffVolScaleNormalized; // 0-100
            _latestTargetVolume = preFinalTargVol / AudioMath.VolumeConversion; // 0-1 (for AudioSources to use)

            if (fadeTween != null)
            {
                fadeTween.Kill();
            }

            fadeTween = baseSource.DOFade(_latestTargetVolume, args.FadeDuration)
                .OnComplete(() => args.OnComplete(args));
        }

        protected Tween fadeTween;

        public virtual void FadeVolume(SetVolumeArgs args)
        {
            float preFinalTargVol = args.TargetVolume * EffVolScaleNormalized; // 0-100
            _latestTargetVolume = preFinalTargVol / AudioMath.VolumeConversion; // 0-1 (for AudioSources to use)

            if (fadeTween != null)
            {
                fadeTween.Kill();
            }

            fadeTween = baseSource.DOFade(_latestTargetVolume, args.FadeDuration)
                .OnComplete(() => args.OnComplete(args));
        }

        public virtual void SetVolume(IAudioArgs args)
        {
            if (!args.WantsVolumeSet)
                return;

            if (args.WantsFade)
            {
                args.OnComplete = SetBeforeOnComplete(args, UpdateSettings);
                FadeVolume(args);
            }
            else
            {
                SetVolumeImmediate(args.TargetVolume);
                args.OnComplete(args);
            }
        }

        public virtual void SetVolume(SetVolumeArgs args)
        {
            if (args.WantsFade)
            {
                FadeVolume(args);
            }
            else
            {
                SetVolumeImmediate(args.TargetVolume);
            }
        }

        public virtual void SetPitch(IAudioArgs args)
        {
            if (!args.WantsPitchSet)
                return;

            if (args.WantsFade)
            {
                FadePitch(args);
            }
            else
            {
                SetPitchWithoutDelay(args);
                args.OnComplete(args);
            }
        }

        protected virtual void FadePitch(IAudioArgs args)
        {
            float startingPitch = CurrentPitch, targetPitch = args.TargetPitch;
            tweeningPitch = true;

            TweenCallback onComplete = () =>
            {
                tweeningPitch = false;
                args.OnComplete(args);
            };

            baseSource.DOPitch(targetPitch, args.FadeDuration)
                .OnComplete(onComplete);
        }

        protected virtual void TweenPitch(float newPitch)
        {
            baseSource.pitch = newPitch;
        }

        protected virtual bool Loop
        {
            get { return baseSource.loop; }
            set { baseSource.loop = value; }
        }

        protected virtual float AtTime
        {
            get { return baseSource.time; }
            set { baseSource.time = value; }
        }

        public virtual void Stop(IAudioArgs args)
        {
            Stop();
            args.OnComplete(args);
        }

        public virtual void Stop()
        {
            baseSource.Stop();
        }

    }
}
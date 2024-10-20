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

        protected IEnumerator _playOnLoop;

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

        protected AudioClip Clip
        {
            get { return baseSource.clip; }
            set { baseSource.clip = value; }
        }

        protected float _latestTargetVolume = 100f;
        protected bool tweeningVolume, tweeningPitch;

        public virtual void SetVolumeImmediate(float targVol)
        {
            _latestTargetVolume = targVol;
            UpdateCurrentVol();
        }

        // ^Need this as a separate object to avoid loop points getting confused when
        // switching from one song to another
        protected virtual AudioSystem AudioSys { get {  return AudioSystem.S; } }

        protected static double dspTimeScheduleOffset = 0.2;

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

        public virtual void SetVolume(float targVol)
        {
            SetVolumeImmediate(targVol);
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

        public virtual void Stop()
        {
            baseSource.Stop();
        }

    }
}
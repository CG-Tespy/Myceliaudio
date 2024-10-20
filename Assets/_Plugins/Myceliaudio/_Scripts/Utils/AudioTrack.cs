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

        public virtual float BaseVolScale
        {
            get { return _baseVolScale; }
            protected set
            {
                _baseVolScale = value;
                UpdateCurrentVol();
            }
        }

        protected float _baseVolScale = 100f;

        public virtual float BaseVolScaleNormalized
        {
            get { return BaseVolScale / AudioMath.VolumeConversion; }
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

        protected float _effVolScale = 100f;

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

        
        protected bool tweeningVolume, tweeningPitch;

        public virtual void SetVolumeImmediate(float targVol)
        {
            BaseVolScale = targVol;
            UpdateCurrentVol();
        }

        // ^Need this as a separate object to avoid loop points getting confused when
        // switching from one song to another
        protected virtual AudioSystem AudioSys { get {  return AudioSystem.S; } }

        protected static double dspTimeScheduleOffset = 0.2;

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

        public virtual void FadeVolume(AlterVolumeArgs args)
        {
            // Need to tween the base vol, not the audio source directly
            if (fadeTween != null)
            {
                fadeTween.Kill();
            }

            fadeTween = DOTween.To(() => BaseVolScale, OnBaseVolScaleValTween, args.TargetVolume, args.FadeDuration)
                .OnComplete(() => args.OnComplete(args));
        }

        protected Tween fadeTween;

        protected virtual void OnBaseVolScaleValTween(float tweenedValue)
        {
            BaseVolScale = tweenedValue;
            UpdateCurrentVol();
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
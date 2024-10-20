using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Myceliaudio
{
    public class TrackManager : MonoBehaviour
    {
        [SerializeField] protected TrackManager _anchor;
        [SerializeField] protected TrackGroup _trackGroup;

        public virtual TrackGroup Set
        {
            get { return _trackGroup; }
        }

        public virtual void Init()
        {
            this.trackHolder = this.gameObject;
            SetUpInitialTracks();
            this.Anchor = _anchor; // To get volumes adjusted properly
        }

        protected GameObject trackHolder;

        protected virtual void SetUpInitialTracks()
        {
            for (int i = 0; i < initTrackCount; i++)
            {
                EnsureTrackExists(i);
            }
        }

        protected int initTrackCount = 2;

        protected virtual void EnsureTrackExists(int id)
        {
            if (!tracks.ContainsKey(id))
            {
                AudioTrack newTrack = new AudioTrack();
                newTrack.Init(trackHolder);
                newTrack.ID = id;
                newTrack.EffVolScale = this.EffVolScale;
                tracks[id] = newTrack;
            }
        }

        protected IDictionary<int, AudioTrack> tracks = new Dictionary<int, AudioTrack>();

        public virtual TrackManager Anchor
        {
            get { return _anchor; }
            set
            {
                TrackManager prevAnchor = _anchor;
                if (prevAnchor != null)
                {
                    prevAnchor.BaseVolScaleChanged -= OnAnchorVolChanged;
                }

                _anchor = value;
                if (_anchor != null)
                {
                    _anchor.BaseVolScaleChanged += OnAnchorVolChanged;
                }
                UpdateTrackVolumeScales();
            }
        }

        public virtual void Play(AudioArgs args)
        {
            EnsureTrackExists(args.Track);
            tracks[args.Track].Play(args);
        }

        public virtual void Play(PlayAudioArgs args)
        {
            EnsureTrackExists(args.Track);
            tracks[args.Track].Play(args);
        }

        public virtual void SetVolume(AudioArgs args)
        {
            EnsureTrackExists(args.Track);
            tracks[args.Track].SetVolume(args);
        }

        public virtual void SetTrackVolume(SetVolumeArgs args)
        {
            EnsureTrackExists(args.Track);
            tracks[args.Track].SetVolume(args);
        }

        public virtual void SetVolume(float newVol, int trackToSetFor = 0)
        {
            EnsureTrackExists(trackToSetFor);
            tracks[trackToSetFor].SetVolumeImmediate(newVol);
        }

        protected virtual AudioArgs WithVolumeScaleApplied(AudioArgs baseArgs)
        {
            AudioArgs result = AudioArgs.CreateCopy(baseArgs);
            result.TargetVolume *= VolumeScaleNormalized;
            return result;
        }

        protected virtual SetVolumeArgs WithVolumeScaleApplied(SetVolumeArgs baseArgs)
        {
            SetVolumeArgs result = SetVolumeArgs.CreateCopy(baseArgs);
            result.TargetVolume *= VolumeScaleNormalized;
            return result;
        }

        /// <summary>
        /// Normalized for AudioSources so we can use it as a multiplier. You know, since those prefer scales of 0 to 1.
        /// </summary>
        protected virtual float VolumeScaleNormalized
        {
            get { return BaseVolumeScale / AudioMath.VolumeConversion; }
        }

        /// <summary>
        /// From 0 to 100. Affects how the volume gets changed. Not scaled by the anchor.
        /// This should be what's considered when showing and changing the vol level in the UI.
        /// </summary>
        public virtual float BaseVolumeScale
        {
            // We go for 0-100 scale here so that users can work with it more intuitively. Hence why this
            // property is public while the normalized ver is not
            get
            {
                return _baseVolumeScale;
            }
            set
            {
                _baseVolumeScale = Mathf.Clamp(value, AudioMath.MinVol, AudioMath.MaxVol);

                BaseVolScaleChanged(_baseVolumeScale);
                AudioEvents.TrackSetVolChanged(Set, _baseVolumeScale);
                UpdateTrackVolumeScales();
            }
        }
        
        protected float _baseVolumeScale = 100f;
        public event UnityAction<float> BaseVolScaleChanged = delegate { };

        // This affects the actual volume values that the tracks play at.
        public virtual float EffVolScale
        {
            get
            {
                float result = _baseVolumeScale;

                if (Anchor != null)
                {
                    result *= Anchor.VolumeScaleNormalized;
                }

                return result;
            }
        }

        public virtual void OnAnchorVolChanged(float newVolScale)
        {
            UpdateTrackVolumeScales();
        }

        protected virtual void UpdateTrackVolumeScales()
        {
            foreach (var trackEl in tracks.Values)
            {
                trackEl.EffVolScale = this.EffVolScale;
            }
        }
        
        public virtual void FadeVolume(SetVolumeArgs args)
        {
            EnsureTrackExists(args.Track);
            args = WithVolumeScaleApplied(args);
            tracks[args.Track].FadeVolume(args);
        }

        public virtual void Stop(AudioArgs args)
        {
            EnsureTrackExists(args.Track);
            tracks[args.Track].Stop(args);
        }

        public virtual void Stop(int track)
        {
            var trackToStop = tracks[track];
            trackToStop.Stop();
        }

        public virtual float GetVolume(int track)
        {
            EnsureTrackExists(track);
            return tracks[track].CurrentVolume;
        }

        public virtual float GetPitch(int track)
        {
            EnsureTrackExists(track);
            return tracks[track].CurrentPitch;
        }

        public virtual string Name { get; set; }

    }
}
#define MYCELIAUDIO
#define MYCELIAUDIO_1_01_01f1_OR_LATER
#define MYCELIAUDIO_1_03_01f1_OR_LATER
using UnityEngine;
using System.Collections.Generic;

namespace CGT.Myceliaudio
{
    public class AudioSystem : MonoBehaviour, IAudioPlayer<IPlayAudioContext>
    {
        public static AudioSystem S
        {
            get
            {
                EnsureExists();
                return _s;
            }
        }

        protected static void EnsureExists()
        {
            // We check here to avoid creating craploads of AudioSyses from lots of
            // AudioCommands being executed in short order
            bool alreadySetUp = _s != null;
            if (alreadySetUp)
            {
                return;
            }

            _s = AudioSystemBuilder.BuildDefault();
        }

        protected virtual void Awake()
        {
            if (_s != null && _s != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _s = this;
            }

            RegisterTrackManagers();
            DontDestroyOnLoad(this.gameObject);
        }

        protected static AudioSystem _s;
        protected AudioClipSplitter _clipSplitter;

        protected virtual void RegisterTrackManagers()
        {
            IList<TrackManager> managersFound = GetComponentsInChildren<TrackManager>();

            foreach (TrackManager manager in managersFound)
            {
                TrackManagers[manager.Group] = manager;
            }
        }

        public IDictionary<TrackGroup, TrackManager> TrackManagers = new Dictionary<TrackGroup, TrackManager>();

        public virtual float GetTrackVol(TrackGroup trackGroup, int track = 0)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            return managerToUse.GetVolume(track);
        }

        public virtual void SetTrackVol(AlterAudioSourceArgs args)
        {
            TrackManager managerToUse = TrackManagers[args.TrackGroup];
            managerToUse.SetTrackVolume(args);
        }

        public virtual void SetTrackVol(TrackGroup trackGroup, int track, float targVol)
        {
            AlterAudioSourceArgs args = new AlterAudioSourceArgs()
            {
                TrackGroup = trackGroup,
                Track = track,
                TargetValue = targVol
            };

            SetTrackVol(args);
        }

        public virtual float GetTrackGroupVol(TrackGroup trackGroup)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            return managerToUse.BaseVolume;
        }

        public virtual void SetTrackGroupVol(TrackGroup trackGroup, float newVol)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            managerToUse.BaseVolume = newVol;
        }

        public virtual void Play(IPlayAudioContext args)
        {
            if (args.OneShot)
            {
                PlayOneShot(args);
            }
            else
            {
                var managerToInvolve = TrackManagers[args.TrackGroup];
                managerToInvolve.Play(args);
            }
        }

        public virtual void PlayOneShot(IPlayAudioContext args)
        {
            PlayOneShot(args.TrackGroup, args.Track, args.Clip);
        }

        public virtual void PlayOneShot(TrackGroup group, int track, AudioClip clip)
        {
            var managerToInvolve = TrackManagers[group];
            managerToInvolve.PlayOneShot(track, clip);
        }

        public virtual void StopPlaying(TrackGroup trackGroup, int track = 0)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            managerToUse.Stop(track);
        }

        public virtual void FadeTrackVol(AlterAudioSourceArgs args)
        {
            TrackManager managerToUse = TrackManagers[args.TrackGroup];
            managerToUse.FadeTrackVolume(args);
        }

        public static string SystemSettingsFileName { get; set; } = "myceliaudioSettings.json";

        public virtual AudioClip GetClipPlayingAt(TrackGroup trackGroup, int track)
        {
            var manager = TrackManagers[trackGroup];
            return manager.GetClipPlayingIn(track);
        }

        public virtual AudioClip GetIntro(AudioClip originalClip, double loopStartPoint)
        {
            return _clipSplitter.GetIntro(originalClip, loopStartPoint);
        }

        public virtual AudioClip GetLoop(AudioClip originalClip, double loopStartPoint, double loopEndPoint)
        {
            return _clipSplitter.GetLoop(originalClip, loopStartPoint, loopEndPoint);
        }

        protected virtual void OnDestroy()
        {
            _clipSplitter.Clear();
        }

    }
}
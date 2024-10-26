using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGT.Myceliaudio.Internal
{
    public class AudioPlayer
    {
        public AudioPlayer(IDictionary<TrackGroup, TrackManager> trackManagers)
        {
            this._trackManagers = trackManagers;
        }

        protected IDictionary<TrackGroup, TrackManager> _trackManagers;

        public virtual void Play(PlayAudioArgs args)
        {
            var managerToInvolve = _trackManagers[args.TrackGroup];
            managerToInvolve.Play(args);
        }

        public virtual void StopPlaying(TrackGroup trackGroup, int track = 0)
        {
            TrackManager managerToUse = _trackManagers[trackGroup];
            managerToUse.Stop(track);
        }

    }
}
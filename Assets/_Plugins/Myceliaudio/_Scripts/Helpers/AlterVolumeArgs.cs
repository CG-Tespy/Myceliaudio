using System;
using UnityEngine.Events;

namespace CGT.Myceliaudio
{
    public class AlterVolumeArgs : EventArgs
    {
        public virtual float TargetVolume { get; set; }
        public virtual TrackGroup TrackSet { get; set; }
        public virtual int Track { get; set; }

        public virtual bool WantsFade
        {
            get { return FadeDuration > 0; }
        }

        public virtual float FadeDuration { get; set; } = 0f;

        public virtual UnityAction<AlterVolumeArgs> OnComplete { get; set; } = delegate { };

        public static AlterVolumeArgs CreateCopy(AlterVolumeArgs other)
        {
            AlterVolumeArgs result = new AlterVolumeArgs()
            {
                TargetVolume = other.TargetVolume,
                TrackSet = other.TrackSet,
                Track = other.Track,
                FadeDuration = other.FadeDuration,
                OnComplete = other.OnComplete
            };

            return result;

        }
    }
}
using System;
using UnityEngine.Events;

namespace CGT.Myceliaudio
{
    public class AlterVolumeArgs : EventArgs
    {
        public virtual float TargetVolume { get; set; }
        public virtual TrackGroup TrackGroup { get; set; }
        public virtual int Track { get; set; }

        public virtual bool WantsFade
        {
            get { return FadeDuration > 0; }
        }

        public virtual float FadeDuration { get; set; } = 0f;

        public virtual UnityAction<AlterVolumeArgs> OnComplete { get; set; } = delegate { };

        /// <summary>
        /// Executes the fading process. If null, the system will go with the default.
        /// </summary>
        public virtual VolumeFadeHandler ApplyCustomFade { get; set; }

        public static AlterVolumeArgs CreateCopy(AlterVolumeArgs other)
        {
            AlterVolumeArgs result = new AlterVolumeArgs()
            {
                TargetVolume = other.TargetVolume,
                TrackGroup = other.TrackGroup,
                Track = other.Track,
                FadeDuration = other.FadeDuration,
                OnComplete = other.OnComplete,
                ApplyCustomFade = other.ApplyCustomFade,
            };

            return result;

        }
    }
}
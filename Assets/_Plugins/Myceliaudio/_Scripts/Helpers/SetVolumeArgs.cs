using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Myceliaudio
{
    public class SetVolumeArgs : EventArgs
    {
        public virtual float TargetVolume { get; set; }
        public virtual TrackGroup TrackSet { get; set; }
        public virtual int Track { get; set; }

        public virtual bool WantsFade
        {
            get { return FadeDuration > 0; }
        }

        public virtual float FadeDuration { get; set; } = 0f;

        public virtual UnityAction<SetVolumeArgs> OnComplete { get; set; } = delegate { };

        public static SetVolumeArgs CreateCopy(SetVolumeArgs other)
        {
            SetVolumeArgs result = new SetVolumeArgs()
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
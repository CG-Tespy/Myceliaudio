using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myceliaudio
{
    public class PlayAudioArgs : EventArgs
    {
        public virtual AudioClip Clip { get; set; }
        public virtual TrackSet TrackSet { get; set; }
        public virtual int Track { get; set; }
        public virtual bool Loop { get; set; }

        public virtual double LoopStartPoint
        {
            get
            {
                return _loopStartPoint;
            }
            set
            {
                _loopStartPoint = Math.Clamp(value, 0, double.MaxValue);
            }
        }
        protected double _loopStartPoint;

        public virtual double LoopEndPoint
        {
            get
            {
                return _loopEndPoint;
            }
            set
            {
                _loopEndPoint = Math.Clamp(value, 0, double.MaxValue);
            }
        }
        protected double _loopEndPoint;

        public virtual bool HasLoopEndPoint
        {
            get { return LoopEndPoint > 0; }
        }

    }
}
using UnityEngine;

namespace CGT.AudManSys
{
    public class AudioPlayArgs : System.EventArgs
    {
        public virtual AudioClip Clip { get; set; }
        public virtual DefaultChannel DefaultChannel { get; set; } = DefaultChannel.Null;
        public virtual int CustomChannel { get; set; }
        public virtual bool Loop { get; set; }

        /// <summary>
        /// In milliseconds.
        /// </summary>
        public virtual float StartPoint { get; set; }

        public virtual bool UseDefaultChannel { get { return DefaultChannel != DefaultChannel.Null; } }

        public virtual bool UseCustomChannel { get { return !UseDefaultChannel; } }

    }
}
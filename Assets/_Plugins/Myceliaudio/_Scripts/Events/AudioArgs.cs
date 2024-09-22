using UnityEngine;

namespace Myceliaudio
{
    public class AudioArgs : System.EventArgs, IAudioArgs
    {
        public virtual bool WantsClipPlayed { get { return Clip != null; } }
        public virtual AudioClip Clip { get; set; }

        public virtual bool WantsVolumeSet { get; set; }

        /// <summary>
        /// On a scale of 0 to 100. 0 = total silence, 100 = max vol
        /// </summary>
        public virtual float TargetVolume { get; set; }

        public virtual bool WantsFade
        {
            get { return !Mathf.Approximately(FadeDuration, 0f); }
        }

        public virtual float FadeDuration { get; set; }

        public virtual bool WantsPitchSet { get; set; }


        /// <summary>
        /// On a scale of 0 to 200. 0 = min, 100 = base pitch, 200 = double the base pitch
        /// </summary>
        public virtual float TargetPitch { get; set; }

        public virtual bool Loop { get; set; }

        public virtual float AtTime { get; set; }
        public virtual bool WantsPlayAtNewTime
        {
            get { return WantsClipPlayed && AtTime > 0; }
        }

        public virtual int Track { get; set; }
        public virtual TrackSet TrackSet { get; set; }

        /// <summary>
        /// By default a func that does nothing.
        /// </summary>
        public virtual AudioHandler OnComplete { get; set; } = (IAudioArgs args) => { };
        public virtual double LoopStartPoint { get; set; }


        /// <summary>
        /// If 0 or less, this is treated as if set to be right at the end of the clip
        /// </summary>
        public virtual double LoopEndPoint { get; set; } = -1;
        public virtual bool HasLoopEndPoint { get { return LoopEndPoint > 0; } }

        public AudioArgs() { }

        public static AudioArgs CreateCopy(AudioArgs source)
        {
            AudioArgs theCopy = new AudioArgs();
            theCopy.SetFrom(source);
            return theCopy;
        }

        public virtual void SetFrom(AudioArgs source)
        {
            this.Clip = source.Clip;
            this.WantsVolumeSet = source.WantsVolumeSet;
            this.TargetVolume = source.TargetVolume;
            this.FadeDuration = source.FadeDuration;
            this.WantsPitchSet = source.WantsPitchSet;
            this.TargetPitch = source.TargetPitch;
            this.Loop = source.Loop;
            this.AtTime = source.AtTime;
            this.Track = source.Track;
            this.OnComplete = source.OnComplete;
            this.TrackSet = source.TrackSet;

            this.LoopStartPoint = source.LoopStartPoint;
            this.LoopEndPoint = source.LoopEndPoint;
        }
    }
}
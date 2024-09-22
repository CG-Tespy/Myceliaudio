using UnityEngine;

namespace Myceliaudio
{
    public interface IAudioTweener
    {
        public void FadeVol(AudioTweenArgs args);
    }

    public class AudioTweenArgs : System.EventArgs
    {
        public virtual AudioSource Source { get; set; }
        public virtual float TargVol { get; set; }
        public virtual float Duration { get; set; }
        public virtual System.Action OnComplete { get; set; } = delegate { };
    }
}
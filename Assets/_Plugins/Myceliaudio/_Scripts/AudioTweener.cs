using UnityEngine;

namespace Myceliaudio
{
    public abstract class AudioTweener : ScriptableObject, IAudioTweener
    {
        public abstract void FadeVol(AudioTweenArgs args);
    }
}
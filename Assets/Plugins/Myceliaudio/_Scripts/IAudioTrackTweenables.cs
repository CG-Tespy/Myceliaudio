using UnityEngine;

namespace CGT.Myceliaudio
{
    public interface IAudioTrackTweenables
    {
        float BaseVolume { get; set; }
        GameObject GameObject { get; }
    }
}
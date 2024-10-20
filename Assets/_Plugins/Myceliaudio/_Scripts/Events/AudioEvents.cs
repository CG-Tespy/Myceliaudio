using UnityEngine.Events;

namespace CGT.Myceliaudio
{
    public static class AudioEvents
    {
        public static UnityAction<TrackGroup, float> TrackSetVolChanged = delegate { };
    }
}
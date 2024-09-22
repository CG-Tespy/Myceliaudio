using UnityEngine.Events;

namespace Myceliaudio
{
    public static class AudioEvents
    {
        public static UnityAction<TrackSet, float> TrackSetVolChanged = delegate { };
    }
}
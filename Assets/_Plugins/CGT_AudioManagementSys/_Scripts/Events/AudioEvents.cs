using UnityEngine.Events;

namespace CGT.AudManSys
{
    public static class AudioEvents
    {
        public static UnityAction<TrackSet, float> TrackSetVolChanged = delegate { };
    }
}
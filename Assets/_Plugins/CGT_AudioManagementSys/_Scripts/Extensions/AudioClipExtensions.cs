using UnityEngine;

namespace CGT.AudManSys
{
    public static class AudioClipExtensions
    {
        public static double PreciseLength(this AudioClip clip)
        {
            double result = (double)clip.samples / clip.frequency;
            return result;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myceliaudio
{
    public interface IAudioArgs
    {
        bool WantsClipPlayed { get; }
        AudioClip Clip { get; }
        bool WantsVolumeSet { get; }
        float TargetVolume { get; }
        bool WantsFade { get; }
        float FadeDuration { get; }
        bool WantsPitchSet { get; }
        float TargetPitch { get; }
        bool Loop { get; }
        float AtTime { get; }
        bool WantsPlayAtNewTime { get; }
        int Track { get; }
        TrackGroup TrackSet { get; }
        AudioHandler OnComplete { get; set; }
        double LoopStartPoint { get; }
        double LoopEndPoint { get; }
        bool HasLoopEndPoint { get; }
    }
}
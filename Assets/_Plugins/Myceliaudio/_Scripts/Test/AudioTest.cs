using Myceliaudio;
using UnityEngine;
using TrackSet = Myceliaudio.TrackSet;

public class AudioTest : MonoBehaviour
{
    [SerializeField] protected AudioClip music;

    protected virtual void Start()
    {
        AudioSystem.EnsureExists();

        AudioArgs args = new AudioArgs()
        {
            Clip = music,
            Loop = true,
            Track = 0,
            TrackSet = TrackSet.BGMusic,
            TargetVolume = 0.3f,
            WantsVolumeSet = true,
        };

        AudioSystem.S.Play(args);
    }

    protected virtual AudioSystem AudioSys { get { return AudioSystem.S; } }
}

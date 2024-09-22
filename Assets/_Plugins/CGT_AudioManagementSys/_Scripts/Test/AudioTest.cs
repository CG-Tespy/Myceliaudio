using CGT.AudManSys;
using UnityEngine;
using TrackSet = CGT.AudManSys.TrackSet;

public class AudioTest : MonoBehaviour
{
    [SerializeField] protected AudioClip music;

    protected virtual void Start()
    {
        AudioManager.EnsureExists();

        AudioArgs args = new AudioArgs()
        {
            Clip = music,
            Loop = true,
            Track = 0,
            TrackSet = TrackSet.BGMusic,
            TargetVolume = 0.3f,
            WantsVolumeSet = true,
        };

        AudioManager.S.Play(args);
    }

    protected virtual AudioManager AudioSys { get { return AudioManager.S; } }
}

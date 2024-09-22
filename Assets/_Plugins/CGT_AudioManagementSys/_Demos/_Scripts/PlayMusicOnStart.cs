using UnityEngine;

namespace CGT.AudManSys.Demos
{
    public class PlayMusicOnStart : MonoBehaviour
    {
        [SerializeField] protected AudioClip _clip;
        [Tooltip("In milliseconds")]
        [SerializeField] protected double _loopStartPoint = 0f, _loopEndPoint = 0f;
        [SerializeField] protected float _startingVol = 50f;

        protected virtual void Awake()
        {
            AudioManager.EnsureExists();

            AudioArgs playMusic = new AudioArgs()
            {
                TrackSet = TrackSet.BGMusic,
                TargetVolume = _startingVol,
                Clip = _clip,
                Loop = true,
                LoopStartPoint = _loopStartPoint,
                LoopEndPoint = _loopEndPoint
            };

            AudioManager.S.Play(playMusic);
        }
    }
}
using UnityEngine;

namespace CGT.Myceliaudio.Demos
{
    public class PlayMusicOnStart : MonoBehaviour
    {
        [SerializeField] protected AudioClip _clip;
        [Tooltip("In milliseconds")]
        [SerializeField] protected double _loopStartPoint = 0f, _loopEndPoint = 0f;
        [SerializeField] protected float _startingVol = 50f;

        protected virtual void Awake()
        {
            PlayAudioArgs playMusic = new PlayAudioArgs()
            {
                Clip = _clip,
                TrackSet = TrackGroup.BGMusic,
                Loop = true,
                LoopStartPoint = _loopStartPoint,
                LoopEndPoint = _loopEndPoint
            };

            AudioSystem.S.Play(playMusic);
        }
    }
}
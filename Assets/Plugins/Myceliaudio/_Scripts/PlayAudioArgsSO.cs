using UnityEngine;

namespace CGT.Myceliaudio
{
    [CreateAssetMenu(fileName = "PlayAudioArgsSO", menuName = "CGT/Myceliaudio/PlayAudioArgsSO")]
    public class PlayAudioArgsSO : ScriptableObject, IPlayAudioContext
    {
        [SerializeField] protected PlayAudioArgs _details;

        public virtual AudioClip Clip
        {
            get => _details.Clip;
            set => _details.Clip = value;
        }

        public virtual TrackGroup TrackGroup
        {
            get => _details.TrackGroup;
            set => _details.TrackGroup = value;
        }

        public virtual int Track
        {
            get => _details.Track;
            set => _details.Track = value;
        }

        public virtual bool Loop
        {
            get => _details.Loop;
            set => _details.Loop = value;
        }

        public virtual double LoopStartPoint
        {
            get => _details.LoopStartPoint;
            set => _details.LoopStartPoint = value;
        }

        public virtual double LoopEndPoint
        {
            get => _details.LoopEndPoint;
            set => _details.LoopEndPoint = value;
        }

        public virtual bool HasEndPointBeforeEndOfClip => _details.HasEndPointBeforeEndOfClip;

        public virtual bool OneShot
        {
            get => _details.OneShot;
            set => _details.OneShot = value;
        }
    }
}

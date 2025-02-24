using CGT.Collections;
using UnityEngine;

namespace CGT.Myceliaudio.Utils
{
    public class QuickPlayAudioRandom : MonoBehaviour
    {
        [SerializeField] protected PlayAudioArgs[] _audioConfigs = new PlayAudioArgs[] { };
        [SerializeField] protected AudioTiming _timing = AudioTiming.Awake;

        protected virtual void Awake()
        {
            PlayIfDesiredTimingIs(AudioTiming.Awake);
        }

        protected PlayAudioArgs _playAudio;

        protected virtual void PlayIfDesiredTimingIs(AudioTiming currentTiming)
        {
            _playAudio = _audioConfigs.GetRandom();

            if ((_timing & currentTiming) > 0)
            {
                AudioSystem.S.Play(_playAudio);
            }
        }

        protected virtual void OnEnable()
        {
            PlayIfDesiredTimingIs(AudioTiming.OnEnable);
        }

        protected virtual void OnDisable()
        {
            PlayIfDesiredTimingIs(AudioTiming.OnDisable);
        }

        protected virtual void OnDestroy()
        {
            PlayIfDesiredTimingIs(AudioTiming.OnDestroy);
        }
    }
}
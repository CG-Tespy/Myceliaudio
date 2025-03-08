using CGT.Collections;
using UnityEngine;

namespace CGT.Myceliaudio.Utils
{
    public class QuickPlayAudioRandom : MonoBehaviour
    {
        [SerializeField] protected PlayAudioArgs[] _audioConfigs = new PlayAudioArgs[] { };
        [SerializeField] protected AudioTiming _timing = AudioTiming.Awake;
        [SerializeField] protected bool _ignoreIfAlreadyPlaying;

        protected virtual void Awake()
        {
            PlayIfDesiredTimingIs(AudioTiming.Awake);
        }

        protected PlayAudioArgs _playAudio;

        protected virtual void PlayIfDesiredTimingIs(AudioTiming currentTiming)
        {
            _playAudio = _audioConfigs.GetRandom();
            bool alreadyPlaying = AudioSystem.S.GetClipPlayingAt(_playAudio.TrackGroup, _playAudio.Track) == _playAudio.Clip;

            if (alreadyPlaying && _ignoreIfAlreadyPlaying)
            {
                return;
            }

            if ((_timing & currentTiming) > 0)
            {
                if (_playAudio.OneShot)
                {
                    AudioSystem.S.PlayOneShot(_playAudio);
                }
                else
                {
                    AudioSystem.S.Play(_playAudio);
                }
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
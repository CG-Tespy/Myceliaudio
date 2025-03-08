using UnityEngine;

namespace CGT.Myceliaudio.Utils
{
    public class QuickPlayAudio : MonoBehaviour
    {
        [SerializeField] protected AudioTiming _timing = AudioTiming.Awake;
        [SerializeField] protected PlayAudioArgs _mainArgs = new PlayAudioArgs();
        [SerializeField] protected bool _ignoreIfAlreadyPlaying;

        protected virtual void Awake()
        {
            if (_mainArgs.Clip == null)
            {
                Debug.LogWarning($"{this.gameObject}'s {this.GetType().Name} has no clip to play.");
                return;
            }

            RegisterOwnerName();
            PlayAudioIfTimingIs(AudioTiming.Awake);
        }

        protected virtual void RegisterOwnerName()
        {
            // For debug messages
            _ownerName = this.name;
            if (this.transform.parent != null)
            {
                _ownerName = this.transform.parent.name;
            }
        }

        protected string _ownerName;

        protected virtual void PlayAudioIfTimingIs(AudioTiming currentTiming)
        {
            bool correctTiming = (_timing & currentTiming) > 0;
            if (!correctTiming)
            {
                return;
            }

            if (ShouldIgnore())
            {
                return;
            }

            if (_mainArgs.Clip != null)
            {
                AudioSystem.S.Play(_mainArgs);
            }
            else
            {
                string errorMessage = $"{_ownerName} cannot play audio when the clip is null.";
                Debug.LogError(errorMessage);
            }
        }

        protected virtual bool ShouldIgnore()
        {
            AudioClip currentlyPlaying = AudioSystem.S.GetClipPlayingAt(_mainArgs.TrackGroup, _mainArgs.Track);
            bool alreadyPlayingThisClip = _mainArgs.Clip == currentlyPlaying;
            bool whetherWeShould = alreadyPlayingThisClip && _ignoreIfAlreadyPlaying;
            if (whetherWeShould)
            {
                string logMessage = $"Not going to play {_ownerName}'s {_mainArgs.Clip.name} in {_mainArgs.TrackGroup} Track {_mainArgs.Track}. Reason: it was already playing there.";
                Debug.Log(logMessage);
            }

            return whetherWeShould;
        }

        protected virtual void OnEnable()
        {
            PlayAudioIfTimingIs(AudioTiming.OnEnable);
        }

        protected virtual void OnDisable()
        {
            PlayAudioIfTimingIs(AudioTiming.OnDisable);
        }

        protected virtual void OnDestroy()
        {
            PlayAudioIfTimingIs(AudioTiming.OnDestroy);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace CGT.AudManSys.Demos
{
    public class PlayAudioOnClick : MonoBehaviour
    {
        [SerializeField] protected Button[] _buttons = new Button[0];
        [SerializeField] protected TrackSet _trackSet;
        [SerializeField] protected AudioClip _clip;
        [SerializeField] protected float _delay = 0.05f;

        protected virtual void Awake()
        {
            _args.TrackSet = _trackSet;
            _args.Clip = _clip;
        }

        protected AudioArgs _args = new AudioArgs();

        protected virtual void OnEnable()
        {
            foreach (var button in _buttons)
            {
                button.onClick.AddListener(OnButtonClicked);
            }
        }

        protected virtual void OnButtonClicked()
        {
            Invoke(nameof(PlayTheClip), _delay);
        }

        protected virtual void PlayTheClip()
        {
            AudioManager.S.Play(_args);
        }

        protected virtual void OnDisable()
        {
            foreach (var button in _buttons)
            {
                button.onClick.RemoveListener(OnButtonClicked);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Myceliaudio
{
    public class VolumeAdjusterUI : MonoBehaviour
    {
        [SerializeField] protected TrackSet _trackType;
        [SerializeField] protected int _track = 0;
        [SerializeField] [Min(1)] protected int adjustmentInterval = 10;
        [SerializeField] protected Button lowerVolButton, raiseVolButton;

        protected virtual void Awake()
        {
            AudioManager.EnsureExists();
            DecideAudioTypeArgs();
            _volAlterationArgs.TrackSet = _forTheAudioType.TrackSet;
            _volAlterationArgs.WantsVolumeSet = true;

            UpdateCurrentVol();
        }

        protected virtual void DecideAudioTypeArgs()
        {
            switch (_trackType)
            {
                case TrackSet.Master:
                    _forTheAudioType = _getMasterMusicVol;
                    break;
                case TrackSet.BGMusic:
                    _forTheAudioType = _getBGMusicVol;
                    break;
                case TrackSet.SoundFX:
                    _forTheAudioType = _getSoundFXVol;
                    break;
                case TrackSet.Voice:
                    _forTheAudioType = _getVoiceVol;
                    break;
                default:
                    Debug.LogError($"Didn't account for AudioType {_trackType}");
                    break;
            }
        }

        protected AudioArgs _forTheAudioType;

        protected static AudioArgs _getMasterMusicVol = new AudioArgs()
        {
            TrackSet = TrackSet.Master,
        };

        protected static AudioArgs _getBGMusicVol = new AudioArgs()
        {
            TrackSet = TrackSet.BGMusic,
        };

        protected static AudioArgs _getSoundFXVol = new AudioArgs()
        {
            TrackSet = TrackSet.SoundFX,
        };

        protected static AudioArgs _getVoiceVol = new AudioArgs()
        {
            TrackSet = TrackSet.Voice,
        };

        protected AudioArgs _volAlterationArgs = new AudioArgs();

        protected virtual void OnEnable()
        {
            lowerVolButton.onClick.AddListener(OnLowerVolButtonClicked);
            raiseVolButton.onClick.AddListener(OnRaiseVolButtonClicked);
        }

        protected virtual void OnLowerVolButtonClicked()
        {
            AlterVol(lowerIt);
        }

        protected static int lowerIt = -1;

        protected virtual void AlterVol(int sign)
        {
            UpdateCurrentVol();
            float volAdjustement = adjustmentInterval * sign;
            float targetVol = _currentVol + volAdjustement;
            targetVol = Mathf.Clamp(targetVol, AudioMath.MinVol, AudioMath.MaxVol);
            AudioSys.SetVolOf(_trackType, targetVol);
            UpdateCurrentVol();
        }

        protected virtual void UpdateCurrentVol()
        {
            _currentVol = AudioSys.GetVolOf(_trackType);
        }

        protected float _currentVol;

        protected virtual AudioManager AudioSys { get { return AudioManager.S; } }

        protected virtual void OnRaiseVolButtonClicked()
        {
            AlterVol(raiseIt);
        }

        protected static int raiseIt = 1;

        protected virtual void OnDisable()
        {
            lowerVolButton.onClick.RemoveListener(OnLowerVolButtonClicked);
            raiseVolButton.onClick.RemoveListener(OnRaiseVolButtonClicked);
        }

    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CGT.AudManSys.Demos
{
    public class SimpleAudioChanges : MonoBehaviour
    {
        [SerializeField] protected AudioClip bgmToPlay, sfx;
        [SerializeField] protected float volChangeInterval = 5f;

        [Tooltip("In milliseconds")]
        [SerializeField] protected double loopPoint = 0f, endPoint = 0f;

        [Tooltip("On a scale of 0 to 100. 0 = total silence, 100 = max volume")]
        [SerializeField] protected float startingMusicVol = 50f, startingSfxVol = 25f;

        [Header("UI")]
        [SerializeField] protected Button raiseMusicVol;
        [SerializeField] protected Button lowerMusicVol, raiseSfxVol, lowerSfxVol, playSfx;
        [SerializeField] protected TMP_Text musicVolLabel, sfxVolLabel;

        protected virtual void Start()
        {
            AudioManager.EnsureExists();
            AudioSys = AudioManager.S;

            AudioSys.SetVolOf(TrackSet.BGMusic, startingMusicVol);
            AudioSys.SetVolOf(TrackSet.SoundFX, startingSfxVol);

            UpdateTextFields();

            AudioArgs playShortBgm = new AudioArgs()
            {
                Clip = bgmToPlay,
                TrackSet = TrackSet.BGMusic,
                Loop = true,
                LoopStartPoint = loopPoint,
                LoopEndPoint = endPoint,
            };
            
            AudioSys.Play(playShortBgm);
        }

        protected virtual void UpdateTextFields()
        {
            float musicVol = AudioSys.GetVolOf(TrackSet.BGMusic);
            musicVol = Mathf.Round(musicVol);
            musicVolLabel.text = $"Music Volume: {musicVol}%";

            float sfxVol = AudioSys.GetVolOf(TrackSet.SoundFX);
            sfxVol = Mathf.Round(sfxVol);
            sfxVolLabel.text = $"SFX Volume: {sfxVol}%";
        }

        protected virtual void OnEnable()
        {
            raiseMusicVol.onClick.AddListener(OnRaiseMusicVolClicked);
            lowerMusicVol.onClick.AddListener(OnLowerMusicVolClicked);
            raiseSfxVol.onClick.AddListener (OnRaiseSfxVolClicked);
            lowerSfxVol.onClick.AddListener(OnLowerSfxVolClicked);

            playSfx.onClick.AddListener(OnPlaySfxClicked);
        }

        protected virtual void OnRaiseMusicVolClicked()
        {
            ChangeVol(TrackSet.BGMusic, raiseIt);
        }

        protected static int raiseIt = 1;

        protected virtual void ChangeVol(TrackSet type, float sign)
        {
            float currentVol = AudioSys.GetVolOf(type);
            currentVol += volChangeInterval * sign;
            currentVol = Mathf.Clamp(currentVol, AudioMath.MinVol, AudioMath.MaxVol);

            AudioArgs setVol = new AudioArgs()
            {
                TrackSet = type,
                TargetVolume = currentVol,
                WantsVolumeSet = true
            };

            AudioSys.SetVolOf(setVol.TrackSet, currentVol);

            UpdateTextFields();
        }

        protected virtual AudioManager AudioSys { get; set; }

        protected virtual void OnLowerMusicVolClicked()
        {
            ChangeVol(TrackSet.BGMusic, lowerIt);
        }

        protected static int lowerIt = -1;

        protected virtual void OnRaiseSfxVolClicked()
        {
            ChangeVol(TrackSet.SoundFX, raiseIt);
        }

        protected virtual void OnLowerSfxVolClicked()
        {
            ChangeVol(TrackSet.SoundFX, lowerIt);
        }

        protected virtual void OnPlaySfxClicked()
        {
            playSfxArgs.Clip = sfx;
            AudioSys.Play(playSfxArgs);
        }

        AudioArgs playSfxArgs = new AudioArgs()
        {
            TrackSet = TrackSet.SoundFX
        };

        protected virtual void OnDisable()
        {
            raiseMusicVol.onClick.RemoveListener(OnRaiseMusicVolClicked);
            lowerMusicVol.onClick.RemoveListener(OnLowerMusicVolClicked);
            raiseSfxVol.onClick.RemoveListener(OnRaiseSfxVolClicked);
            lowerSfxVol.onClick.RemoveListener(OnLowerSfxVolClicked);

            playSfx.onClick.RemoveListener(OnPlaySfxClicked);
        }
    }
}
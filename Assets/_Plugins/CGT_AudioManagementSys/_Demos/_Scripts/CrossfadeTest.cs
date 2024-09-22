using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Myceliaudio.Demos
{
    public class CrossfadeTest : MonoBehaviour
    {
        [SerializeField] protected AudioClip firstClip, secondClip;
        [SerializeField] protected float volChangeInterval = 5f, fadeChangeInterval = 0.5f;
        [SerializeField] protected Button raiseMusicVol, lowerMusicVol;
        [SerializeField] protected Button raiseFadeDurButton, lowerFadeDurButton;
        [SerializeField] protected Button crossfadeButton;
        [SerializeField] protected float startingVol = 5f;
        [SerializeField] protected TMP_Text musicVolLabel, fadeDurLabel;

        [SerializeField] protected float startingMusicVol = 50f, startingFadeDur = 2f;
        [SerializeField] protected float minFadeDur = 0.5f, maxFadeDur = 5f;

        protected virtual void Awake()
        {
            AudioManager.EnsureExists();
            AudioSys = AudioManager.S;

            AudioSys.SetVolOf(TrackSet.BGMusic, startingMusicVol);

            // We want both clips playing at the same time, but with the second being silent
            AudioArgs playFirstClip = new AudioArgs()
            {
                Clip = firstClip,
                TrackSet = TrackSet.BGMusic,
                Loop = true,
            };

            AudioSys.Play(playFirstClip);

            AudioArgs playSecondClip = new AudioArgs()
            {
                Clip = secondClip,
                TrackSet = TrackSet.BGMusic,
                Loop = true,
                Track = 1,
                TargetVolume = 0,
                WantsVolumeSet = true
            };

            AudioSys.Play(playSecondClip);

            clipCurrentlyAudible = firstClip;

            currentFadeDur = startingFadeDur;
            UpdateTextFields();
        }

        protected float currentFadeDur;

        protected AudioClip clipCurrentlyAudible;

        protected virtual void UpdateTextFields()
        {
            float musicVol = AudioSys.GetVolOf(TrackSet.BGMusic);
            musicVol = Mathf.Round(musicVol);
            musicVolLabel.text = $"Music Volume: {musicVol}%";

            // Fade dur
            float fadeDur = currentFadeDur;
            fadeDurLabel.text = $"Fade Dur: {fadeDur}s";
        }

        protected virtual void OnEnable()
        {
            raiseMusicVol.onClick.AddListener(OnRaiseMusicVolClicked);
            lowerMusicVol.onClick.AddListener(OnLowerMusicVolClicked);

            raiseFadeDurButton.onClick.AddListener(OnRaiseFadeDurClicked);
            lowerFadeDurButton.onClick.AddListener(OnLowerFadeDurClicked);

            crossfadeButton.onClick.AddListener(OnCrossfadeButtonClicked);
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

        protected virtual void OnLowerFadeDurClicked()
        {
            ChangeFadeDur(lowerIt);
            UpdateTextFields();
        }

        protected virtual void ChangeFadeDur(int sign)
        {
            currentFadeDur += fadeChangeInterval * sign;
            currentFadeDur = Mathf.Clamp(currentFadeDur, minFadeDur, maxFadeDur);
        }

        protected virtual void OnRaiseFadeDurClicked()
        {
            ChangeFadeDur(raiseIt);
            UpdateTextFields();
        }

        protected virtual void OnCrossfadeButtonClicked()
        {
            AudioArgs fadeOut = new AudioArgs()
            {
                WantsVolumeSet = true,
                TrackSet = TrackSet.BGMusic,
                TargetVolume = 0,
                FadeDuration = currentFadeDur,
            };

            AudioArgs fadeIn = AudioArgs.CreateCopy(fadeOut);
            fadeIn.TargetVolume = 100f; // Note that this gets scaled by the base vol of the track type
            AudioClip nextClipToBeAudible = null;
            if (clipCurrentlyAudible == firstClip)
            {
                // Then we will fade into the second
                nextClipToBeAudible = secondClip;
                fadeOut.Track = firstClipTrack;
                fadeIn.Track = secondClipTrack;
            }
            else
            {
                nextClipToBeAudible = firstClip;
                fadeOut.Track = secondClipTrack;
                fadeIn.Track = firstClipTrack;
            }

            AudioSys.SetTrackVol(fadeOut);
            AudioSys.SetTrackVol(fadeIn);
            clipCurrentlyAudible = nextClipToBeAudible;

            StartCoroutine(DisableCrossfadeButton(currentFadeDur));
        }

        protected static int firstClipTrack = 0, secondClipTrack = 1;

        protected virtual IEnumerator DisableCrossfadeButton(float duration)
        {
            crossfadeButton.enabled = false;

            yield return new WaitForSeconds(duration);

            crossfadeButton.enabled = true;
        }

        protected virtual void OnDisable()
        {
            raiseMusicVol.onClick.RemoveListener(OnRaiseMusicVolClicked);
            lowerMusicVol.onClick.RemoveListener(OnLowerMusicVolClicked);

            raiseFadeDurButton.onClick.RemoveListener(OnRaiseFadeDurClicked);
            lowerFadeDurButton.onClick.RemoveListener(OnLowerFadeDurClicked);

            crossfadeButton.onClick.RemoveListener(OnCrossfadeButtonClicked);
        }
    }
}
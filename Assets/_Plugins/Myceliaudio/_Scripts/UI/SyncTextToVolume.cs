using TMPro;
using UnityEngine;

namespace Myceliaudio
{
    public class SyncTextToVolume : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _textField;
        [SerializeField] protected TrackGroup _trackGroup;
        
        protected virtual void Awake()
        {
            float initVol = AudioSystem.S.GetTrackGroupVolume(_trackGroup);
            SyncText(initVol);
        }

        protected virtual void SyncText(float newVol)
        {
            float rounded = Mathf.Round(newVol);
            _textField.text = $"{rounded}%";
        }

        protected virtual void OnEnable()
        {
            AudioEvents.TrackSetVolChanged += OnTrackSetVolChanged;
        }

        protected virtual void OnTrackSetVolChanged(TrackGroup involved, float newVol)
        {
            if (this._trackGroup == involved)
            {
                SyncText(newVol);
            }
        }

        protected virtual void OnDisable()
        {
            AudioEvents.TrackSetVolChanged -= OnTrackSetVolChanged;
        }

    }
}
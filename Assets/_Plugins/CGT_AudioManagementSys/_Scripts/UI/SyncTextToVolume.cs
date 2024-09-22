using TMPro;
using UnityEngine;

namespace CGT.AudManSys
{
    public class SyncTextToVolume : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _textField;
        [SerializeField] protected TrackSet _trackSet;
        
        protected virtual void Awake()
        {
            float initVol = AudioManager.S.GetVolOf(_trackSet);
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

        protected virtual void OnTrackSetVolChanged(TrackSet involved, float newVol)
        {
            if (this._trackSet == involved)
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
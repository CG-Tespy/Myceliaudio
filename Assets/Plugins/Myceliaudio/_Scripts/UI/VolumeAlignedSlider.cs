using CGT.Myceliaudio;
using UnityEngine;

namespace CGT.Myceliaudio
{
    /// <summary>
    /// Changes the value of a Slider when the volume of
    /// the specified TrackSet changes.
    /// </summary>
    public class VolumeAlignedSlider : AudioSliderComponent
    {
        protected override void InitApply()
        {
            base.InitApply();
            float vol = AudioSystem.S.GetTrackGroupVol(_trackGroup);
            SyncWith(vol);
        }

        protected virtual void OnEnable()
        {
            AudioEvents.TrackSetVolChanged += OnVolumeChanged;
        }

        // We assume a 0-100 scale for the new value
        protected virtual void OnVolumeChanged(TrackGroup setInvolved, float newVol)
        {
            if (setInvolved == this._trackGroup)
            {
                SyncWith(newVol);
            }
        }

        protected virtual void OnDisable()
        {
            AudioEvents.TrackSetVolChanged -= OnVolumeChanged;
        }
    }
}
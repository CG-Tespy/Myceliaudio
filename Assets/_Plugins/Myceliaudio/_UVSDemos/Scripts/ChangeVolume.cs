using Unity.VisualScripting;
using UnityEngine;

namespace CGT.Myceliaudio
{
    public class ChangeVolume : SoundShifterUnit
    {
        protected override void Definition()
        {
            base.Definition();
        }

        protected override void PrepAudioArgs()
        {
            base.PrepAudioArgs();
            _setVolumeArgs.TargetVolume = _targetValueVal;
        }

        protected override ControlOutput OnEnterStart(Flow flow)
        {
            var baseOutput = base.OnEnterStart(flow);
            AudioSystem.S.SetTrackVol(_setVolumeArgs);
            Debug.Log($"ChangeVolume set to {_setVolumeArgs.TargetVolume}");
            return baseOutput;
        }

    }
}
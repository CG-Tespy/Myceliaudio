using Unity.VisualScripting;

namespace Myceliaudio.UVS
{
    public class FadeVolume : SoundShifterUnit
    {
        protected override void PrepAudioArgs()
        {
            base.PrepAudioArgs();
            _setVolumeArgs.TargetVolume = _targetValueVal;
            _setVolumeArgs.FadeDuration = _durationVal;
        }

        protected override ControlOutput OnEnterStart(Flow flow)
        {
            var baseOutput = base.OnEnterStart(flow);
            AudioSystem.S.SetTrackVol(_setVolumeArgs);
            return baseOutput;
        }

    }
}
using Unity.VisualScripting;

namespace Myceliaudio.UVS
{
    public class FadeVolume : SoundShifterUnit
    {
        protected override void PrepAudioArgs()
        {
            base.PrepAudioArgs();
            _audioArgs.WantsVolumeSet = true;
            _audioArgs.TargetVolume = _targetValueVal;
            _audioArgs.FadeDuration = _durationVal;
        }

        protected override ControlOutput OnEnterStart(Flow flow)
        {
            var baseOutput = base.OnEnterStart(flow);
            AudioSystem.S.SetTrackVol(_audioArgs);
            return baseOutput;
        }

    }
}
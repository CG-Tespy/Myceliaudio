using Unity.VisualScripting;

namespace CGT.Myceliaudio.UVS
{
    public class FadeVolume : SoundShifterUnit
    {
        [DoNotSerialize] public ValueInput duration;

        protected override void PrepValuePorts()
        {
            base.PrepValuePorts();
            duration = ValueInput<float>(nameof(duration), 0);
        }

        protected override void PrepValueVars(Flow flow)
        {
            base.PrepValueVars(flow);
            _durationVal = flow.GetValue<float>(duration);
        }

        protected override void PrepAudioArgs()
        {
            base.PrepAudioArgs();
            _setVolumeArgs.TargetVolume = _targetValueVal;
            _setVolumeArgs.FadeDuration = _durationVal;
        }

        protected override ControlOutput OnEnterStart(Flow flow)
        {
            var baseOutput = base.OnEnterStart(flow);
            AudioSystem.S.FadeVolume(_setVolumeArgs);
            return baseOutput;
        }

    }
}
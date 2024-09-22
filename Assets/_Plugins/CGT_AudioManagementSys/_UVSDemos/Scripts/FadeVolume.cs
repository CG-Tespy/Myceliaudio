using Unity.VisualScripting;

namespace CGT.AudManSys.UVS
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
            AudioManager.S.SetTrackVol(_audioArgs);
            return baseOutput;
        }

    }
}
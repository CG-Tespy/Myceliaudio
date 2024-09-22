namespace Myceliaudio
{
    public class ChangeVolume : SoundShifterUnit
    {
        protected override void Definition()
        {
            throw new System.NotImplementedException();
        }

        protected override void PrepAudioArgs()
        {
            base.PrepAudioArgs();
            _audioArgs.WantsVolumeSet = true;
            _audioArgs.TargetVolume = _targetValueVal;
        }

    }
}
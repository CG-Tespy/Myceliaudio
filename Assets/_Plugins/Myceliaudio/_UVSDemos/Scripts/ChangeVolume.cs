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
            _setVolumeArgs.TargetVolume = _targetValueVal;
        }

    }
}
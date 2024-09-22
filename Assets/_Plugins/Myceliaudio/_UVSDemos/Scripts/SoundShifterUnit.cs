using Unity.VisualScripting;
using UnityEngine;

namespace Myceliaudio
{
    public class SoundShifterUnit : AudioUnit
    {
        [DoNotSerialize] public ValueInput track;

        [Tooltip("In terms of percentages. 0 for silent, 100 for max vol.")]
        [DoNotSerialize] public ValueInput targetValue;
        [DoNotSerialize] public ValueInput duration;

        // No need for a WaitForFade field. We can just add an output port that executes
        // when the fading is done

        protected override void Definition()
        {
            base.Definition();
        }

        protected override void PrepValuePorts()
        {
            base.PrepValuePorts();
            track = ValueInput<int>(nameof(track), 0);
            targetValue = ValueInput<float>(nameof(targetValue), 0);
            duration = ValueInput<float>(nameof(duration), 0);
        }

        protected override void PrepValueVars(Flow flow)
        {
            base.PrepValueVars(flow);
            _trackVal = flow.GetValue<int>(track);
            _targetValueVal = flow.GetValue<float>(targetValue);
            _durationVal = flow.GetValue<float>(duration);
        }

        protected int _trackVal;
        protected float _targetValueVal;

        protected override void PrepAudioArgs()
        {
            base.PrepAudioArgs();
            _audioArgs.Track = _trackVal;
        }

    }
}
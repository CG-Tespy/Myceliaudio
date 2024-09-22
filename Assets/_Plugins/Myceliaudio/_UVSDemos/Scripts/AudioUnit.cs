using Unity.VisualScripting;

namespace Myceliaudio
{
    public class AudioUnit : Unit
    {
        // No need to serialize ports.
        [DoNotSerialize] public ControlInput enter;
        [DoNotSerialize] public ControlOutput started;

        [DoNotSerialize] public ValueInput trackSet;

        protected override void Definition()
        {
            PrepValuePorts();
            PrepControlPorts();
            PrepInputRequirements();
        }

        protected virtual void PrepValuePorts()
        {
            trackSet = ValueInput<TrackSet>(nameof(trackSet), TrackSet.Null);
        }

        protected virtual void PrepControlPorts()
        {
            //enter = ControlInputCoroutine(nameof(enter), OnEnter);
            enter = ControlInput(nameof(enter), OnEnterStart);
            started = ControlOutput(nameof(started));

            Succession(enter, started);
        }

        protected virtual void PrepInputRequirements()
        {

        }

        /// <summary>
        /// What happens as soon as the enter port is reached.
        /// </summary>
        protected virtual ControlOutput OnEnterStart(Flow flow)
        {
            PrepValueVars(flow);
            // ^We can only do this when a ControlInput executes, making
            // us have to go with this clunky approach
            PrepAudioArgs();

            return started;
        }

        protected virtual void PrepValueVars(Flow flow)
        {
            // For example, assigning the value of a ValueInput<int> to an int var
            _trackSetVal = flow.GetValue<TrackSet>(trackSet);

            // Leaving _durationVal uninitialized here since not all audio units will
            // handle pauses the same way
        }

        protected TrackSet _trackSetVal;
        protected float _durationVal;

        protected virtual void PrepAudioArgs()
        {
            _audioArgs.TrackSet = _trackSetVal;
        }

        protected AudioArgs _audioArgs = new AudioArgs();

    }
}
using Unity.VisualScripting;

namespace CGT
{
    public class CGTUnit : Unit
    {
        // No need to serialize ports.
        [DoNotSerialize] public ControlInput enter;
        [DoNotSerialize] public ControlOutput exit;

        protected override void Definition()
        {
            PrepValuePorts();
            PrepControlPorts();
            PrepInputRequirements();
        }

        protected virtual void PrepValuePorts()
        {

        }

        protected virtual void PrepControlPorts()
        {
            enter = ControlInput(nameof(enter), OnEnterLogic);
            exit = ControlOutput("outputTrigger");
            Succession(enter, exit);
        }

        protected virtual void PrepInputRequirements()
        {
            
        }

        protected virtual ControlOutput OnEnterLogic(Flow flow)
        {
            if (!ValueVarsPrepped)
            {
                PrepValueVars(flow);
                // ^We can only do this when a ControlInput executes, making
                // us have to go with this clunky approach
            }

            return exit;
        }

        protected bool ValueVarsPrepped { get; set; }

        /// <summary>
        /// If this func is overridden, best have it execute last in the overridER func
        /// </summary>
        protected virtual void PrepValueVars(Flow flow)
        {
            // For example, assigning the value of a ValueInput<int> to an int var
            ValueVarsPrepped = true;
        }
    }
}
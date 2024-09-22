using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SetInteractableAlt : Unit
{
    [DoNotSerialize] // No need to serialize ports.
    public ControlInput enter;

    [DoNotSerialize] // No need to serialize ports.
    public ControlOutput exit;

    [DoNotSerialize]
    public ValueInput target;

    [DoNotSerialize]
    public ValueInput value;

    [DoNotSerialize]
    public ValueInput includeChildren;

    protected override void Definition()
    {
        PrepValuePorts();
        PrepControlPorts();
        PrepInputRequirements();
    }

    protected virtual void PrepValuePorts()
    {
        target = ValueInput<Transform>(nameof(target), null);
        value = ValueInput<bool>(nameof(value), false);
        includeChildren = ValueInput<bool>(nameof(includeChildren), false);
    }

    protected virtual void PrepControlPorts()
    {
        enter = ControlInput(nameof(enter), OnEnterLogic);
        exit = ControlOutput("outputTrigger");
        Succession(enter, exit);
    }

    protected virtual void PrepInputRequirements()
    {
        Requirement(target, enter);
    }

    protected virtual ControlOutput OnEnterLogic(Flow flow)
    {
        Transform targTForm = flow.GetValue<Transform>(target);
        bool interactable = flow.GetValue<bool>(value);

        Selectable attachedToTarg = targTForm.GetComponent<Selectable>();
        if (attachedToTarg != null)
        {
            attachedToTarg.interactable = interactable;
        }

        if (targTForm != null)
        {
            bool childrenToo = flow.GetValue<bool>(includeChildren);

            if (childrenToo)
            {
                IList<Selectable> children = targTForm.GetComponentsInChildren<Selectable>();

                foreach (var child in children)
                {
                    child.interactable = interactable;
                }
            }
        }

        return exit;
    }
}

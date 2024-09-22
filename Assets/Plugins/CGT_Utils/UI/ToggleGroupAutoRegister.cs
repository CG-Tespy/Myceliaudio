using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CGT.UI
{
    // Can automatically register toggles to a group so you don't have to 
    // mess with the individual Toggles' fields in the Inspector. This 
    // allows for a more programmatic way to manage those
    public class ToggleGroupAutoRegister : MonoBehaviour
    {
        [SerializeField] protected ToggleGroup baseGroup;
        [SerializeField] protected Transform holdsToggles;
        
        protected virtual void Start()
        {
            // Need to set up in Start so we can be sure that the programmatically-instantiated
            // toggles are... well, already instantiated
            if (holdsToggles == null)
            {
                holdsToggles = this.transform;
            }

            RegisterTogglesInHolder();
        }

        protected virtual void RegisterTogglesInHolder()
        {
            IList<Toggle> togglesFound = holdsToggles.GetComponentsInChildren<Toggle>();

            foreach (var item in togglesFound)
            {
                item.group = baseGroup;
                baseGroup.RegisterToggle(item);
                _toggles.Add(item);
            }
        }

        protected IList<Toggle> _toggles = new List<Toggle>();

    }
}
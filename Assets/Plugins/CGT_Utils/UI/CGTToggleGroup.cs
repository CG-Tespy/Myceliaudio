using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CGT.UI
{
    // Can automatically register toggles to itself so you don't have to 
    // mess with the individual Toggles' fields. This allows for a more
    // programmatic way to manage those
    public class CGTToggleGroup : ToggleGroup
    {
        [SerializeField] protected Transform holdsToggles;
        [SerializeField] protected bool autoRegister = true;

        protected override void Start()
        {
            // Need to set up in Start so we can be sure that the programmatically-instantiate
            // toggles are... well, already instantiated
            base.Start();
            if (holdsToggles == null)
            {
                holdsToggles = this.transform;
            }

            if (autoRegister)
            {
                RegisterTogglesInHolder();
            }

            EnsureJustFirstToggleIsOn();
        }

        protected virtual void RegisterTogglesInHolder()
        {
            IList<Toggle> togglesFound = holdsToggles.GetComponentsInChildren<Toggle>();

            // Need to iterate over them backwards to make sure that the first option is the default
            //for (int i = togglesFound.Count - 1; i >= 0; i--)
            //{
            //    Toggle toggle = togglesFound[i];
            //    RegisterToggle(toggle);
            //}

            foreach (var item in togglesFound)
            {
                RegisterToggle(item);  
            }
        }

        public new void RegisterToggle(Toggle toRegister)
        {
            toRegister.group = this;
            base.RegisterToggle(toRegister);
        }

        protected virtual void EnsureJustFirstToggleIsOn()
        {
            // Since for some reason, without this, it's the last toggle that
            // gets set to on
            Toggle first = m_Toggles[0];
            first.isOn = true;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace CGT
{
    public class BehaviourOrdering : MonoBehaviour
    {
        [SerializeField] protected Transform[] submoduleHolders = new Transform[0];

        protected virtual void Awake()
        {
            GetSubmodules();
            GetSubmodulesSorted();

            foreach (var submodule in sortedSubmodules)
            {
                submodule.AwakeInit();
            }
        }

        protected virtual void GetSubmodules()
        {
            foreach (Transform holder in submoduleHolders)
            {
                IOrderableBehaviour[] found = holder.GetComponents<IOrderableBehaviour>();
                // ^We don't fetch the ones in children here. Reason being to make it easier
                // for the dev to get submodules of their choice ignored
                unsortedSubmodules.AddRange(found);
            }
        }

        protected List<IOrderableBehaviour> unsortedSubmodules = new List<IOrderableBehaviour>();

        protected virtual void GetSubmodulesSorted()
        {
            sortedSubmodules.AddRange(unsortedSubmodules);

            // Higher priority = earlier execution
            sortedSubmodules.Sort(IOrderableSorting.ByPriorityDescending);
        }

        protected List<IOrderableBehaviour> sortedSubmodules = new List<IOrderableBehaviour>();

        protected virtual void Start()
        {
            foreach (var submodule in sortedSubmodules)
            {
                submodule.StartInit();
            }
        }

        protected virtual void Update()
        {
            if (!this.enabled || !this.gameObject.activeInHierarchy)
            {
                return;
            }

            foreach (var submodule in sortedSubmodules)
            {
                if (submodule.Enabled)
                {
                    submodule.OnEarlyUpdate();
                }
            }

            foreach (var submodule in sortedSubmodules)
            {
                if (submodule.Enabled)
                {
                    submodule.OnUpdate();
                }
            }
        }

        public virtual void OnLateUpdate()
        {
            if (!this.enabled || !this.gameObject.activeInHierarchy)
            {
                return;
            }

            foreach (var submodule in sortedSubmodules)
            {
                if (submodule.Enabled)
                {
                    submodule.OnLateUpdate();
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!this.enabled || !this.gameObject.activeInHierarchy)
            {
                return;
            }

            foreach (var submodule in sortedSubmodules)
            {
                if (submodule.Enabled)
                {
                    submodule.OnFixedUpdate();
                }
            }
        }
    }
}
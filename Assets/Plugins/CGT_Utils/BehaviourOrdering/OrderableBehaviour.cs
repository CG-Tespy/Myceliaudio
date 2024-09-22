using UnityEngine;

namespace CGT
{
    public class OrderableBehaviour : MonoBehaviour, IOrderableBehaviour
    {
        [SerializeField] protected int priority;

        public virtual int Priority { get { return priority; } }

        protected virtual void Update() { } // So we can enable and disable this in the Inspector

        public virtual void AwakeInit() { }
        public virtual void StartInit() { }

        public virtual void OnEarlyUpdate() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }

        public virtual bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
    }
}
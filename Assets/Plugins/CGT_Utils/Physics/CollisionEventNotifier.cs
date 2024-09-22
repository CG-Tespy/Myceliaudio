using UnityEngine;
using UnityEngine.Events;

namespace CGT.Events
{
    public class CollisionEventNotifier : MonoBehaviour, ICollisionEventNotifier
    {
        #region TwoDim
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEnter2D(other);
        }

        public event UnityAction<Collider2D> TriggerEnter2D = delegate { };

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            TriggerExit2D(other);
        }

        public event UnityAction<Collider2D> TriggerExit2D = delegate { };

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            TriggerStay2D(other);
        }

        public event UnityAction<Collider2D> TriggerStay2D = delegate { };

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEnter2D(collision);
        }

        public event UnityAction<Collision2D> CollisionEnter2D = delegate { };

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            CollisionExit2D(collision);
        }

        public event UnityAction<Collision2D> CollisionExit2D = delegate { };

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            CollisionStay2D(collision);
        }

        public event UnityAction<Collision2D> CollisionStay2D = delegate { };

        #endregion

        #region ThreeDim
        protected virtual void OnTriggerEnter(Collider other)
        {
            TriggerEnter(other);
        }

        public event UnityAction<Collider> TriggerEnter = delegate { };

        protected virtual void OnTriggerExit(Collider other)
        {
            TriggerExit(other);
        }

        public event UnityAction<Collider> TriggerExit = delegate { };

        protected virtual void OnTriggerStay(Collider other)
        {
            TriggerStay(other);
        }

        public event UnityAction<Collider> TriggerStay = delegate { };

        protected virtual void OnCollisionEnter(Collision collision)
        {
            CollisionEnter(collision);
        }

        public event UnityAction<Collision> CollisionEnter = delegate { };

        protected virtual void OnCollisionExit(Collision collision)
        {
            CollisionExit(collision);
        }

        public event UnityAction<Collision> CollisionExit = delegate { };

        protected virtual void OnCollisionStay(Collision collision)
        {
            CollisionStay(collision);
        }

        public event UnityAction<Collision> CollisionStay = delegate { };
        #endregion

        protected virtual void OnParticleCollision(GameObject other)
        {
            ParticleCollision(other);
        }

        public event UnityAction<GameObject> ParticleCollision = delegate { };
    }
}
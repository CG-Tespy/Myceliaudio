using UnityEngine;

namespace RPG
{
    public class DestroyOnDelay : MonoBehaviour
    {
        public float lifetime = 1;

        protected virtual void Start()
        {
            Destroy(gameObject, lifetime);
        }
    } 
}
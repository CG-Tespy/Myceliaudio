using UnityEngine;

namespace RPG
{
    public class SetInactiveOnDelay : MonoBehaviour
    {
        [SerializeField] protected GameObject toSet;
        [SerializeField] protected float duration = 3f;

        protected virtual void OnEnable()
        {
            Invoke(nameof(TriggerDisable), duration);
        }

        protected virtual void TriggerDisable()
        {
            toSet.SetActive(false);
        }
    }
}
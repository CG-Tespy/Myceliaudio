using UnityEngine;

namespace CGT
{
    public class SetGroupActive : MonoBehaviour
    {
        [SerializeField] protected GameObject[] toSetActiveFor = new GameObject[0];
        [SerializeField] protected bool defaultActive = false;

        protected virtual void Awake()
        {
            foreach (GameObject go in toSetActiveFor)
            {
                go.SetActive(defaultActive);
            }
        }
    }
}
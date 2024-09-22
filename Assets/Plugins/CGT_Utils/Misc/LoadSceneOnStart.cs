using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGT
{
    public class LoadSceneOnStart : MonoBehaviour
    {
        [SerializeField] protected string sceneToLoad;
        [SerializeField] protected float _delay = 1f;
        [SerializeField] protected LoadSceneMode _mode = LoadSceneMode.Additive;

        protected virtual void Start()
        {
            Invoke(nameof(DoTheLoading), _delay);
        }

        protected virtual void DoTheLoading()
        {
            SceneManager.LoadSceneAsync(sceneToLoad, _mode);
        }

    }
}
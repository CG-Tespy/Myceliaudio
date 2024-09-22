using UnityEngine;

namespace Myceliaudio
{
    public class MyceliaudioSettingsSO : ScriptableObject
    {
        [SerializeField] protected AudioSystem _mainPrefab;

        public virtual AudioSystem MainPrefab { get { return _mainPrefab; } }
    }
}
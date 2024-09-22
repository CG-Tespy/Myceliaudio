using UnityEngine;

namespace Myceliaudio
{
    [CreateAssetMenu(fileName = "NewMyceliaudioSettings", menuName = "Myceliaudio/New Settings")]
    public class MyceliaudioSettingsSO : ScriptableObject
    {
        [SerializeField] protected AudioSystem _mainPrefab;
        [Header("Default Volumes")]
        [SerializeField] protected int masterVol = 50;
        [SerializeField] protected int bgMusicVol = 100, soundFxVol = 100, voiceVol = 100;

        public virtual AudioSystem MainPrefab { get { return _mainPrefab; } }
        public virtual int MasterVol { get { return masterVol; } }
        public virtual int BGMusicVol { get { return bgMusicVol; } }
        public virtual int SoundFxVol { get { return soundFxVol; } }
        public virtual int VoiceVol { get { return voiceVol; } }
    }
}
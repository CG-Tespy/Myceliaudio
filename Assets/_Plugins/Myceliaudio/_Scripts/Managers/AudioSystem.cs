#define CGT_AUD_MAN_SYS
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using CGT.Amanita.Myceliaudio;

namespace Myceliaudio
{
    /// <summary>
    /// Alternative to Fungus's built-in MusicManager that works with commands that alter
    /// music or sfx properties separately.
    /// </summary>
    public class AudioSystem : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (_s != null && _s != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _s = this;
            }

            PrepTrackManagers();
            PrepSettingsFile();
            SetDefaultVolumeLevels();
            DontDestroyOnLoad(this.gameObject);
        }

        protected virtual void PrepTrackManagers()
        {
            IList<TrackManager> managersFound = GetComponentsInChildren<TrackManager>();

            foreach (TrackManager manager in managersFound)
            {
                _trackManagers[manager.Set] = manager;
                manager.Init();
            }

            masterManager = _trackManagers[TrackSet.Master];
            musicManager = _trackManagers[TrackSet.BGMusic];
            soundFXManager = _trackManagers[TrackSet.SoundFX];
            voiceManager = _trackManagers[TrackSet.Voice];
        }

        protected IDictionary<TrackSet, TrackManager> _trackManagers = new Dictionary<TrackSet, TrackManager>();
        protected TrackManager masterManager, musicManager, soundFXManager, voiceManager;

        public static void EnsureExists()
        {
            // We check here to avoid creating craploads of AudioSyses from lots of
            // AudioCommands being executed in short order
            bool alreadySetUp = _s != null;
            if (alreadySetUp)
            {
                return;
            }

            MyceliaudioSettingsSO _settings = Resources.Load<MyceliaudioSettingsSO>("MyceliaudioSettings");
            _s = Instantiate(_settings.MainPrefab);
            _s.gameObject.name = _settings.MainPrefab.name;
        }

        public static AudioSystem S
        {
            get
            {
                EnsureExists();
                return _s;
            }
        }

        protected static AudioSystem _s;

        protected virtual void PrepSettingsFile()
        {
            var filePath = Path.Combine(Application.dataPath, SystemSettingsFileName);

            if (!File.Exists(filePath))
            {
                systemSettings = new MyceliaudioSettings();
                string whatToWrite = JsonUtility.ToJson(systemSettings);
                File.WriteAllText(filePath, whatToWrite);
            }
            else
            {
                string jsonString = File.ReadAllText(filePath);
                systemSettings = JsonUtility.FromJson<MyceliaudioSettings>(jsonString);
            }
        }

        protected MyceliaudioSettings systemSettings;
        protected VolumeSettings VolumeSettings { get { return systemSettings.Volume; } }

        public static string SystemSettingsFileName { get; protected set; } = "myceliaudioSettings.json";

        protected virtual void SetDefaultVolumeLevels()
        {
            masterManager.BaseVolumeScale = VolumeSettings.master;
            musicManager.BaseVolumeScale = VolumeSettings.bgMusic;
            soundFXManager.BaseVolumeScale = VolumeSettings.soundFX;
            voiceManager.BaseVolumeScale = VolumeSettings.voice;

            musicManager.Anchor = masterManager;
            soundFXManager.Anchor = masterManager;
            voiceManager.Anchor = masterManager;
            // ^ So that things are scaled relative to the master volume
        }

        /// <summary>
        /// On a scale of 0 to 100. 
        /// </summary>
        /// <param name="args"></param>
        public virtual float GetTrackVol(AudioArgs args)
        {
            return GetTrackVol(args.TrackSet, args.Track);
        }

        public virtual float GetTrackVol(TrackSet trackSet, int track = 0)
        {
            var managerToUse = _trackManagers[trackSet];
            return managerToUse.GetVolume(track);
        }

        /// <summary>
        /// On a scale of 0 to 200.
        /// </summary>
        public float GetTrackPitch(AudioArgs args)
        {
            return GetTrackPitch(args.TrackSet, args.Track);
        }

        public virtual float GetTrackPitch(TrackSet trackSet, int track = 0)
        {
            var managerToUse = _trackManagers[trackSet];
            return managerToUse.GetPitch(track);
        }

        public virtual void SetTrackVol(AudioArgs args)
        {
            var managerToUse = _trackManagers[args.TrackSet];
            managerToUse.SetVolume(args);
        }

        public virtual float GetVolOf(TrackSet trackSet)
        {
            var managerToUse = _trackManagers[trackSet];
            return managerToUse.BaseVolumeScale;
        }

        public virtual void SetVolOf(TrackSet trackSet, float newVol)
        {
            var managerToUse = _trackManagers[trackSet];
            managerToUse.BaseVolumeScale = newVol;
        }

        protected float _masterVol = 100f;

        public virtual void Play(AudioArgs args)
        {
            var managerToInvolve = _trackManagers[args.TrackSet];
            managerToInvolve.Play(args);
        }

        public virtual void PlayVoice(AudioArgs args)
        {
            voiceManager.Play(args);
        }

    }
}
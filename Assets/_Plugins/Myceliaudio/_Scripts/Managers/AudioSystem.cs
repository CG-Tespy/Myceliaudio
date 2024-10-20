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
                TrackManagers[manager.Set] = manager;
                manager.Init();
            }

            masterManager = TrackManagers[TrackSet.Master];
            musicManager = TrackManagers[TrackSet.BGMusic];
            soundFXManager = TrackManagers[TrackSet.SoundFX];
            voiceManager = TrackManagers[TrackSet.Voice];
        }

        public IDictionary<TrackSet, TrackManager> TrackManagers = new Dictionary<TrackSet, TrackManager>();
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

        public virtual float GetTrackVol(TrackSet trackSet, int track = 0)
        {
            var managerToUse = TrackManagers[trackSet];
            return managerToUse.GetVolume(track);
        }

        public virtual float GetTrackPitch(TrackSet trackSet, int track = 0)
        {
            var managerToUse = TrackManagers[trackSet];
            return managerToUse.GetPitch(track);
        }

        public virtual void SetTrackVol(AudioArgs args)
        {
            var managerToUse = TrackManagers[args.TrackSet];
            managerToUse.SetVolume(args);
        }

        public virtual void SetTrackVol(SetVolumeArgs args)
        {
            var managerToUse = TrackManagers[args.TrackSet];
            managerToUse.SetVolume(args);
        }

        public virtual float GetTrackGroupVolume(TrackSet trackSet)
        {
            var managerToUse = TrackManagers[trackSet];
            return managerToUse.BaseVolumeScale;
        }

        public virtual void SetTrackGroupVol(TrackSet trackSet, float newVol)
        {
            var managerToUse = TrackManagers[trackSet];
            managerToUse.BaseVolumeScale = newVol;
        }

        protected float _masterVol = 100f;

        public virtual void Play(AudioArgs args)
        {
            var managerToInvolve = TrackManagers[args.TrackSet];
            managerToInvolve.Play(args);
        }

        public virtual void Play(PlayAudioArgs args)
        {
            var managerToInvolve = TrackManagers[args.TrackSet];
            managerToInvolve.Play(args);
        }

        public virtual void Stop(TrackSet trackSet, int track = 0)
        {
            var managerToUse = TrackManagers[trackSet];
            managerToUse.Stop(track);
        }
    }
}
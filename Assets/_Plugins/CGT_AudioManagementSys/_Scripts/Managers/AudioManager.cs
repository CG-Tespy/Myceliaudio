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
    public class AudioManager : MonoBehaviour
    {
        public static void EnsureExists()
        {
            // We check here to avoid creating craploads of AudioSyses from lots of
            // AudioCommands being executed in short order
            bool alreadySetUp = _s != null;
            if (alreadySetUp)
            {
                return;
            }

            GameObject sysGO = new GameObject(sysName);
            AudioManager theSys = sysGO.AddComponent<AudioManager>();
            _s = theSys;
        }

        protected static string sysName = "CGT_AudioManagementSystem";

        public static AudioManager S
        {
            get
            {
                EnsureExists();
                return _s;
            }
        }

        protected static AudioManager _s;

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

            PrepSettingsFile();
            EnsureTrackManagersAreThere();
            SetDefaultVolumeLevels();
            DontDestroyOnLoad(this.gameObject);
        }

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

        protected virtual void EnsureTrackManagersAreThere()
        {
            string masterManagerName = "MasterSet";
            EnsureThereIsManagerFor(TrackSet.Master, masterManagerName);

            string musicManagerName = "BGMusicTracks";
            EnsureThereIsManagerFor(TrackSet.BGMusic, musicManagerName);

            string sfxManagerName = "SoundFXTracks";
            EnsureThereIsManagerFor(TrackSet.SoundFX, sfxManagerName);

            string voiceManagerName = "VoiceTracks";
            EnsureThereIsManagerFor(TrackSet.Voice, voiceManagerName);

            masterManager = trackManagers[TrackSet.Master];
            musicManager = trackManagers[TrackSet.BGMusic];
            soundFXManager = trackManagers[TrackSet.SoundFX];
            voiceManager = trackManagers[TrackSet.Voice];
        }

        protected virtual void EnsureThereIsManagerFor(TrackSet setType, string managerName)
        {
            bool itIsThere = trackManagers.ContainsKey(setType) && trackManagers[setType] != null;
            if (!itIsThere)
            {
                trackManagers[setType] = CreateTrackManager(setType, managerName);
            }
        }

        protected virtual TrackManager CreateTrackManager(TrackSet setType, string name)
        {
            // We have separate game objects for the managers so we can check the
            // track-counts and such in the Scene view
            GameObject holdsManager = new GameObject(name);
            holdsManager.transform.SetParent(this.transform);
            TrackManager manager = new TrackManager(holdsManager);
            manager.Set = setType;
            manager.Name = name;
            
            return manager;
        }

        protected IDictionary<TrackSet, TrackManager> trackManagers = new Dictionary<TrackSet, TrackManager>();

        protected TrackManager masterManager, musicManager, soundFXManager, voiceManager;

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

        protected virtual void OnEnable()
        {
            EnsureTrackManagersAreThere();
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
            var managerToUse = trackManagers[trackSet];
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
            var managerToUse = trackManagers[trackSet];
            return managerToUse.GetPitch(track);
        }

        public virtual void SetTrackVol(AudioArgs args)
        {
            var managerToUse = trackManagers[args.TrackSet];
            managerToUse.SetVolume(args);
        }

        public virtual float GetVolOf(TrackSet trackSet)
        {
            var managerToUse = trackManagers[trackSet];
            return managerToUse.BaseVolumeScale;
        }

        public virtual void SetVolOf(TrackSet trackSet, float newVol)
        {
            var managerToUse = trackManagers[trackSet];
            managerToUse.BaseVolumeScale = newVol;
        }


        protected float _masterVol = 100f;

        public virtual void Play(AudioArgs args)
        {
            var managerToInvolve = trackManagers[args.TrackSet];
            managerToInvolve.Play(args);
        }

        public virtual void PlayVoice(AudioArgs args)
        {
            voiceManager.Play(args);
        }

    }
}
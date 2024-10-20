using System.Collections.Generic;
using UnityEngine;

namespace CGT.Myceliaudio
{
    /// <summary>
    /// What you use to alter and check the volumes of different audio tracks
    /// </summary>
    public class VolumeManager : MonoBehaviour
    {
        [SerializeField] protected AudioSystem _main;

        protected virtual void Awake()
        {
            if (_main == null)
            {
                _main = GetComponent<AudioSystem>();
            }
        }

        /// <summary>
        /// Sets the volume of the given track in the given set to the given target volume. 
        /// Said volume should be between 0 for silent and 100 for max.
        /// </summary>
        public virtual void SetTrackVol(TrackGroup set, int track, float targVol)
        {
            TrackManager relevantManager = TrackManagers[set];
            relevantManager.SetTrackVolume(targVol, track);
        }

        protected virtual IDictionary<TrackGroup, TrackManager> TrackManagers { get { return _main.TrackManagers; } }

        /// <summary>
        /// Changes the volume of a specific track set as opposed to an individual
        /// track within one. The volumes of the indiv tracks are scaled off those
        /// applied to their corresponding sets.
        /// </summary>
        public virtual void SetTrackSetVol(TrackGroup trackGroup, float newVol)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            managerToUse.BaseVolumeScale = newVol;
        }

        /// <summary>
        /// Gets the volume of the given track belonging to the given set.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public virtual float GetTrackVol(TrackGroup trackGroup, int track)
        {
            TrackManager relevantManager = TrackManagers[trackGroup];
            float result = relevantManager.GetVolume(track);
            return result;
        }

        public virtual float GetTrackSetVol(TrackGroup trackGroup)
        {
            TrackManager relevantManager = TrackManagers[trackGroup];
            float result = relevantManager.BaseVolumeScale;
            return result;
        }

    }

}
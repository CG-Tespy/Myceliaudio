using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myceliaudio
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
        public virtual void SetTrackVol(TrackSet set, int track, float targVol)
        {
            TrackManager relevantManager = TrackManagers[set];
            relevantManager.SetVolume(targVol, track);
        }

        protected virtual IDictionary<TrackSet, TrackManager> TrackManagers { get { return _main.TrackManagers; } }

        /// <summary>
        /// Changes the volume of a specific track set as opposed to an individual
        /// track within one. The volumes of the indiv tracks are scaled off those
        /// applied to their corresponding sets.
        /// </summary>
        public virtual void SetTrackSetVol(TrackSet trackSet, float newVol)
        {
            TrackManager managerToUse = TrackManagers[trackSet];
            managerToUse.BaseVolumeScale = newVol;
        }

        /// <summary>
        /// Gets the volume of the given track belonging to the given set.
        /// </summary>
        /// <returns>A value between 0 and 100</returns>
        public virtual float GetTrackVol(TrackSet trackSet, int track)
        {
            TrackManager relevantManager = TrackManagers[trackSet];
            float result = relevantManager.GetVolume(track);
            return result;
        }

        public virtual float GetTrackSetVol(TrackSet trackSet)
        {
            TrackManager relevantManager = TrackManagers[trackSet];
            float result = relevantManager.BaseVolumeScale;
            return result;
        }

    }

}
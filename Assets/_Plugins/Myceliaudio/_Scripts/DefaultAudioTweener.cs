using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Myceliaudio
{
    public class DefaultAudioTweener : AudioTweener
    {
        public override void FadeVol(AudioTweenArgs args)
        {
            IEnumerator process = HandleTweening(args);
            GameObject client = args.Source.gameObject;
            throw new System.NotImplementedException();
        }

        protected IDictionary<GameObject, IEnumerator> tweensOngoing = new Dictionary<GameObject, IEnumerator>();

        protected virtual IEnumerator HandleTweening(AudioTweenArgs args)
        {
            throw new System.NotImplementedException ();
        }

    }
}
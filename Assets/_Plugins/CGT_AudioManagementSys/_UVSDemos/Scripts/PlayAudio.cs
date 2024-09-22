using Unity.VisualScripting;
using UnityEngine;

namespace CGT.AudManSys
{
    public class PlayAudio : AudioUnit
    {
        [DoNotSerialize] public ValueInput track;
        [DoNotSerialize] public ValueInput clip;

        [DoNotSerialize] public ValueInput loopStartPoint, loopEndPoint;

        [DoNotSerialize] public ValueInput loop;

        protected override void Definition()
        {
            base.Definition(); // Need this to make sure UVS recognizes this unit
        }

        protected override void PrepValuePorts()
        {
            base.PrepValuePorts();
            track = ValueInput<int>(nameof(track), 0);
            clip = ValueInput<AudioClip>(nameof(clip), null);

            loopStartPoint = ValueInput<double>(nameof(loopStartPoint), 0);
            loopEndPoint = ValueInput<double>(nameof(loopEndPoint), -1);

            loop = ValueInput<bool>(nameof(loop), false);
        }

        protected override void PrepInputRequirements()
        {
            base.PrepInputRequirements();
            Requirement(clip, enter);
        }

        protected override void PrepValueVars(Flow flow)
        {
            base.PrepValueVars(flow);
            _trackVal = flow.GetValue<int>(track);
            _clipVal = flow.GetValue<AudioClip>(clip);
            _loopStartPointVal = flow.GetValue<double>(loopStartPoint);
            _loopEndPointVal = flow.GetValue<double>(loopEndPoint);
            _loopVal = flow.GetValue<bool>(loop);
            _durationVal = _clipVal.length;
        }

        protected int _trackVal;
        protected AudioClip _clipVal;
        protected double _loopStartPointVal, _loopEndPointVal;
        protected bool _loopVal;

        protected override void PrepAudioArgs()
        {
            base.PrepAudioArgs();
            _audioArgs.Track = _trackVal;
            _audioArgs.Clip = _clipVal;
            _audioArgs.Loop = _loopVal;
            _audioArgs.LoopStartPoint = _loopStartPointVal;
            _audioArgs.LoopEndPoint = _loopEndPointVal;
        }

        protected override ControlOutput OnEnterStart(Flow flow)
        {
            var baseOutput = base.OnEnterStart(flow);
            AudioManager.S.Play(_audioArgs);
            return baseOutput;
        }

    }
}
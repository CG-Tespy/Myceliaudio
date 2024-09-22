using Unity.VisualScripting;

namespace Myceliaudio.Editor
{
    [Descriptor(typeof(PlayAudio))]
    public class PlayAudioNodeDescs : UnitDescriptor<PlayAudio>
    {
        public PlayAudioNodeDescs(PlayAudio target) : base(target)
        {
        }

        protected override void DefinedPort(IUnitPort port, UnitPortDescription description)
        {
            base.DefinedPort(port, description);

            switch (port.key)
            {
                case "finished":
                    description.summary = "Triggers when the clip is done playing once. Does NOT work if Loop is set to false";
                    break;
                case "loopStartPoint":
                    description.summary = loopPointDescs;
                    break;
                case "loopEndPoint":
                    description.summary = loopPointDescs;
                    break;
                case "fadeDuration":
                    description.summary = "How long to take to fade into the new clip.";
                    break;

            }
        }

        protected static string loopPointDescs = "Only considered if Loop is set to true.";
    }
}
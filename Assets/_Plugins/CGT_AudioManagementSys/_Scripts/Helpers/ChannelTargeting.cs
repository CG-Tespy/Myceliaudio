using UnityEngine;

namespace CGT.AudManSys
{
    [System.Serializable]
    public class ChannelTargeting: ITargetsChannel
    {
        [SerializeField] protected ChannelType channelType;
        [SerializeField] protected DefaultChannel defaultChannel;
        [SerializeField] protected int customChannel = -1;

        public virtual ChannelType ChannelType
        {
            get { return channelType; }
            set { channelType = value; }
        }

        public virtual DefaultChannel DefaultChannel
        {
            get { return defaultChannel; }
            set { defaultChannel = value; }
        }

        public virtual int CustomChannel
        {
            get { return customChannel; }
            set { customChannel = value; }
        }
    }
}
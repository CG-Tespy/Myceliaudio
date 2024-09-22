namespace CGT.AudManSys
{
    public interface ITargetsChannel
    {
        public ChannelType ChannelType { get; }
        public DefaultChannel DefaultChannel { get; }
        public int CustomChannel { get; }
    }
}
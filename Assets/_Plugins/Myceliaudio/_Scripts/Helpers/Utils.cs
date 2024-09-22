using UnityEngine;

namespace Myceliaudio
{
    public static class Utils 
    {
        public static bool TargetSameChannel(ITargetsChannel firstTargeter, ITargetsChannel secondTargeter)
        {
            bool result = false;

            if (firstTargeter.ChannelType == secondTargeter.ChannelType)
            {
                switch (firstTargeter.ChannelType)
                {
                    case ChannelType.Default:
                        result = firstTargeter.DefaultChannel == secondTargeter.DefaultChannel;
                        break;
                    case ChannelType.Custom:
                        result = firstTargeter.CustomChannel == secondTargeter.CustomChannel;
                        break;
                    default:
                        Debug.LogError($"Did not account for channel type {firstTargeter.ChannelType}");
                        result = false;
                        break;
                }
            }

            return result;
        }
    }
}
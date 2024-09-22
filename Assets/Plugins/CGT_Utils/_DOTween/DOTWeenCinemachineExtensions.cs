using Cinemachine;
using DG.Tweening;

namespace CGT.Utils.DGDoTween
{
    public static class DOTWeenCinemachineExtensions
    {
        public static Tween DOShiftPriority(this CinemachineVirtualCamera cam, int priority, float duration)
        {
            return DOTween.To(() => cam.Priority,
                newVal =>
                {
                    cam.Priority = priority;
                },
                priority,
                duration);
        }
    }
}
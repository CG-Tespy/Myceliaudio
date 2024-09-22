using UnityEngine;
using DG.Tweening;

namespace CGT.Utils.DGDoTween
{
    public static class DOTweenExtensions
    {
        public static Tween DOFaceTowards(this Transform tform, Vector3 targetForward, float duration)
        {
            return DOTween.To(() => tform.forward,
                newVal => tform.forward = newVal,
                targetForward,
                duration);
        }

        // Mainly for UVS convenience
        public static void DOMoveAlt(Transform target, Vector3 endValue, float duration = 0,
            bool snapping = false, float timeScale = 1)
        {
            Tween movement = target.DOMove(endValue, duration, snapping).SetAutoKill().SetUpdate(true);
            movement.timeScale = timeScale;
        }

        public static void DOScaleAlt(Transform target, Vector3 endValue, float duration = 0, float timeScale = 1)
        {
            Tween movement = target.DOScale(endValue, duration).SetAutoKill().SetUpdate(true);
            movement.timeScale = timeScale;
        }

    }
}
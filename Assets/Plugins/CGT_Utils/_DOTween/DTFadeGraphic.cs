using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace RPG
{
    public class DTFadeGraphic : MonoBehaviour, IDOTweenEffect
    {
        [SerializeField] protected Graphic graphic;
        [SerializeField] protected float targetAlpha;
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected Ease easing = Ease.Linear;

        public virtual void ApplyEffect()
        {
            fadeTween?.Kill();
            fadeTween = graphic.DOFade(duration, targetAlpha).SetEase(easing);
        }

        protected Tween fadeTween;
    }
}
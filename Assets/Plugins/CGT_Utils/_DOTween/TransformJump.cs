using UnityEngine;
using DG.Tweening;

namespace RPG
{
    public interface IDOTweenEffect
    {
        void ApplyEffect();
    }

    public class TransformJump : MonoBehaviour, IDOTweenEffect
    {
        [SerializeField] protected Transform toJump;
        [SerializeField] protected float jumpPower = 1f;
        [SerializeField] protected int numJumps = 1;
        [SerializeField] protected float jumpDuration = 1f;
        [SerializeField] protected Ease easing = Ease.Linear;

        protected virtual void Awake()
        {
            basePos = toJump.position;
        }

        protected Vector3 basePos;

        public virtual void ApplyEffect()
        {
            if (jumpTween != null) { jumpTween.Kill(); }

            jumpTween = toJump.DOJump(
                endValue: basePos,
                jumpPower: jumpPower,
                numJumps: numJumps,
                duration: jumpDuration).
                SetEase(easing);
        }

        protected Tween jumpTween;
    }
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CGT.UI
{
    // For UI elements that swap positions and parents with stuff they're dropped on
    public class DropSwappable : MonoBehaviour
    {
        [SerializeField] protected Transform toMove;
        [Range(0f, float.MaxValue)]
        [SerializeField] protected float moveDuration = 0.5f;
        [SerializeField] protected Ease easing = Ease.Linear;
        [SerializeField] protected UIPointerEvents pointerEvents;

        public virtual Transform ToMove {  get { return toMove; } }

        protected virtual void OnEnable()
        {
            pointerEvents.PointerClick += OnPointerClick;
            pointerEvents.PointerDown += OnPointerDown;
            pointerEvents.BeginDrag += OnBeginDrag;
            pointerEvents.Drop += OnDrop;
        }

        protected virtual void OnPointerClick(PointerEventData eventData)
        {
            PointerClick(this, eventData);
        }

        public event UnityAction<DropSwappable, PointerEventData> PointerClick = delegate { };

        protected virtual void OnPointerDown(PointerEventData eventData)
        {
            PointerDown(this, eventData);
        }

        public event UnityAction<DropSwappable, PointerEventData> PointerDown = delegate { };

        protected virtual void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag(this, eventData);
            AnyBeginDrag(this, eventData);
        }

        public event UnityAction<DropSwappable, PointerEventData> BeginDrag = delegate { };
        public static event UnityAction<DropSwappable, PointerEventData> AnyBeginDrag = delegate { };

        protected virtual void OnEndDrag(PointerEventData eventData)
        {
            EndDrag(this, eventData);
            AnyEndDrag(this, eventData);
        }

        public event UnityAction<DropSwappable, PointerEventData> EndDrag = delegate { };
        public static event UnityAction<DropSwappable, PointerEventData> AnyEndDrag = delegate { };

        protected virtual void OnDrop(PointerEventData eventData)
        {
            Drop(this, eventData);
        }

        public event UnityAction<DropSwappable, PointerEventData> Drop = delegate { };

        protected virtual void OnDisable()
        {
            pointerEvents.PointerClick -= OnPointerClick;
            pointerEvents.PointerDown -= OnPointerDown;
            pointerEvents.BeginDrag -= OnBeginDrag;
            pointerEvents.Drop -= OnDrop;
        }
    }
}
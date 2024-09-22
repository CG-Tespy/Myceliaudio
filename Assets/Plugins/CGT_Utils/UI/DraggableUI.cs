using UnityEngine;
using UnityEngine.EventSystems;

namespace CGT.UI
{
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] protected UIPointerEvents pointerEvents;
        [SerializeField] protected Transform toDrag;

        [SerializeField] [Min(1f)] protected float _catchupSpeed = 10f;

        protected virtual void Awake()
        {
            if (toDrag == null)
            {
                Debug.LogWarning($"ToDrag of DraggableUI on {this.transform.name} not set. Defaulting to {this.transform.name}");
                toDrag = this.transform;
            }

            ResetBases();
        }

        public virtual void ResetBases()
        {
            BasePos = toDrag.position;
            BaseSiblingIndex = toDrag.GetSiblingIndex();
            BaseHolder = toDrag.parent;
        }

        protected virtual void OnEnable()
        {
            pointerEvents.BeginDrag += OnBeginDrag;
            pointerEvents.Drag += OnDrag;
            pointerEvents.EndDrag += OnEndDrag;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            ResetBases();

            // We want this rendered over other UI elements while it's being dragged
            toDrag.SetParent(toDrag.root);
            toDrag.SetAsLastSibling();
            _isDragging = true;
        }

        protected bool _isDragging;

        public virtual Vector3 BasePos { get; set; }
        protected int BaseSiblingIndex { get; set; }
        public virtual Transform BaseHolder { get; set; }

        public virtual void OnDrag(PointerEventData eventData)
        {
            //toDrag.position = Vector3.Lerp(toDrag.position, Input.mousePosition, CatchupMultiplier * Time.deltaTime);
        }

        protected virtual void Update()
        {
            if (_isDragging)
            {
                toDrag.position = Vector3.Lerp(toDrag.position, Input.mousePosition, _catchupSpeed * Time.deltaTime);
            }
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            toDrag.SetParent(BaseHolder);
            toDrag.SetSiblingIndex(BaseSiblingIndex);
            toDrag.position = BasePos;
            _isDragging = false;
        }

        protected virtual void OnDisable()
        {
            pointerEvents.BeginDrag -= OnBeginDrag;
            pointerEvents.Drag -= OnDrag;
            pointerEvents.EndDrag -= OnEndDrag;
        }
    }
}
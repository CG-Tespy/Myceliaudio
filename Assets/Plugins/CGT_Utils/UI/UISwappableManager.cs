using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CGT.UI
{
    public class UISwappableManager : MonoBehaviour
    {
        [SerializeField] protected Transform holdsSwappables;
        [SerializeField] protected Transform holdsLayouts;
        [SerializeField] protected float moveDur = 0.5f;

        protected virtual void Awake()
        {
            swappables = holdsSwappables.GetComponentsInChildren<DropSwappable>();
        }

        protected IList<DropSwappable> swappables;

        protected virtual void OnEnable()
        {
            PrepPreSwapStates();
            ListenForEvents();
        }

        protected virtual void PrepPreSwapStates()
        {
            // When clicking or dragging, Flexalon Interactables might reparent the thing that was clicked or dragged
            // before it even gets moved. Thus, we need to keep in mind the pre-drag states and update them when needed.
            // This way, we can make sure each swapee gets moved and parented to the right stuff
            foreach (var swappableEl in swappables)
            {
                preSwapStates[swappableEl.ToMove] = new TransformState();
            }
        }

        protected IDictionary<Transform, TransformState> preSwapStates = new Dictionary<Transform, TransformState>();

        protected virtual void ListenForEvents()
        {
            foreach (var swappableEl in swappables)
            {
                swappableEl.PointerDown += UpdatePreSwapState;
                swappableEl.PointerClick += OnSwappableClicked;
                swappableEl.Drop += OnSwappableDrop;
            }
        }

        protected virtual void UpdatePreSwapState(DropSwappable toUpdateFor, PointerEventData weIgnore)
        {
            TransformState toUpdate = preSwapStates[toUpdateFor.ToMove];
            toUpdate.SetFrom(toUpdateFor.ToMove);
        }

        protected virtual void OnSwappableClicked(DropSwappable swappable, PointerEventData eventData)
        {
            if (firstToSwap == null)
            {
                firstToSwap = swappable.ToMove;
            }
            else if (secondToSwap == null && firstToSwap != swappable)
            {
                secondToSwap = swappable.ToMove;
            }

            bool shouldApplySwap = firstToSwap != null && secondToSwap != null;

            if (shouldApplySwap)
            {
                DoSwap();
            }
        }

        protected Transform firstToSwap, secondToSwap;

        protected virtual void DoSwap()
        {
            DealWithDraggableFor(firstToSwap);
            DealWithDraggableFor(secondToSwap);

            stateOfFirst = preSwapStates[firstToSwap];
            stateOfSecond = preSwapStates[secondToSwap];

            // We want them rendered above the rest of the UI while swapping
            
            firstToSwap.SetParent(transform.root, true);
            firstToSwap.SetAsLastSibling();
            secondToSwap.SetParent(transform.root, true);
            secondToSwap.SetAsLastSibling();

            targetStateForFirst = stateOfSecond;
            targetStateForSecond = stateOfFirst; // State-swapping

            var sequence = DOTween.Sequence();

            BeginSwap();
            moveFirst = firstToSwap.DOMove(targetStateForFirst.Position, moveDur);
            moveSecond = secondToSwap.DOMove(targetStateForSecond.Position, moveDur).SetEase(Ease.Linear);
            
            // We need to use Insert instead of Append to make sure that the tweens run at the same time
            sequence.Insert(0, moveFirst);
            sequence.Insert(0, moveSecond);
            sequence.OnComplete(OnBothDoneMoving);
        }

        protected virtual void DealWithDraggableFor(Transform toSwap)
        {
            DraggableUI draggable = toSwap.GetComponentInChildren<DraggableUI>();
            if (draggable != null)
            {
                draggable.ResetBases();
            }
        }

        protected TransformState stateOfFirst = new TransformState(), stateOfSecond = new TransformState(),
            targetStateForFirst, targetStateForSecond;

        protected Tween moveFirst, moveSecond;

        protected virtual void OnBothDoneMoving()
        {
            Debug.Log($"First item ({firstToSwap.name}) finished moving!");
            firstToSwap.SetParent(targetStateForFirst.Parent, true);
            firstToSwap.SetSiblingIndex(targetStateForFirst.SiblingIndex);

            Debug.Log($"Second item ({secondToSwap.name}) finished moving!");
            secondToSwap.SetParent(targetStateForSecond.Parent, true);
            secondToSwap.SetSiblingIndex(targetStateForSecond.SiblingIndex);

            firstToSwap = secondToSwap = null;

            UISwapArgs args = new UISwapArgs()
            {
                First = firstToSwap,
                Second = secondToSwap,
            };
            EndSwap(args);
        }

        protected virtual void OnSwappableDrop(DropSwappable dropTarget, PointerEventData eventData)
        {
            GameObject droppedOn = eventData.pointerDrag;
            DropSwappable whatWasBeingDragged = droppedOn.GetComponentInChildren<DropSwappable>();

            if (whatWasBeingDragged != null)
            {
                UpdatePreSwapState(dropTarget, null);
                firstToSwap = dropTarget.ToMove;
                secondToSwap = whatWasBeingDragged.ToMove;
                DoSwap();
            }
        }

        protected virtual void OnDisable()
        {
            UNlistenForEvents();
        } 

        protected virtual void UNlistenForEvents()
        {
            foreach (var swappableEl in swappables)
            {
                swappableEl.PointerDown -= UpdatePreSwapState;
                swappableEl.PointerClick -= OnSwappableClicked;
                swappableEl.Drop -= OnSwappableDrop;
            }
        }

        public event UnityAction BeginSwap = delegate () { };
        public event UnityAction<UISwapArgs> EndSwap = delegate { };
    }

    public class UISwapArgs
    {
        public virtual Transform First { get; set; }
        public virtual Transform Second { get; set; }
    }
}
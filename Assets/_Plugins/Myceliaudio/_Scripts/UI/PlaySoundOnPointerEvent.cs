using CGT.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PointerEventData = UnityEngine.EventSystems.PointerEventData;

namespace Myceliaudio
{
    public class PlaySoundOnPointerEvent : MonoBehaviour
    {
        [SerializeField] protected UIPointerEventType _toRespondTo;
        [SerializeField] protected UIPointerEvents _events;
        [SerializeField] protected TrackSet _trackSet;
        [SerializeField] protected int _track;
        [SerializeField] protected AudioClip _soundToPlay;

        protected virtual void Awake()
        {
            _audioArgs.TrackSet = _trackSet;
            _audioArgs.Track = _track;
            _audioArgs.Clip = _soundToPlay;
        }

        protected AudioArgs _audioArgs = new AudioArgs();

        protected List<UnityAction<PointerEventData>> toListenFor = new List<UnityAction<PointerEventData>>();

        protected virtual void OnEnable()
        {
            ListenForEvents();
        }

        protected virtual void ListenForEvents()
        {
            if ((_toRespondTo & UIPointerEventType.Up) == UIPointerEventType.Up)
            {
                _events.PointerUp += OnPointerEventTriggered;
            }

            if ((_toRespondTo & UIPointerEventType.Down) == UIPointerEventType.Down)
            {
                _events.PointerDown += OnPointerEventTriggered;
            }

            if ((_toRespondTo & UIPointerEventType.Click) == UIPointerEventType.Click)
            {
                _events.PointerClick += OnPointerEventTriggered;
            }

            //////

            if ((_toRespondTo & UIPointerEventType.Enter) == UIPointerEventType.Enter)
            {
                _events.PointerEnter += OnPointerEventTriggered;
            }

            if ((_toRespondTo & UIPointerEventType.Exit) == UIPointerEventType.Exit)
            {
                _events.PointerExit += OnPointerEventTriggered;
            }

            //////

            if ((_toRespondTo & UIPointerEventType.BeginDrag) == UIPointerEventType.BeginDrag)
            {
                _events.BeginDrag += OnPointerEventTriggered;
            }

            if ((_toRespondTo & UIPointerEventType.Drag) == UIPointerEventType.Drag)
            {
                _events.Drag += OnPointerEventTriggered;
            }

            if ((_toRespondTo & UIPointerEventType.EndDrag) == UIPointerEventType.EndDrag)
            {
                _events.EndDrag += OnPointerEventTriggered;
            }

            if ((_toRespondTo & UIPointerEventType.Drop) == UIPointerEventType.Drop)
            {
                _events.Drop += OnPointerEventTriggered;
            }

        }

        protected virtual void OnPointerEventTriggered(PointerEventData eventData)
        {
            AudioSystem.S.Play(_audioArgs);
        }

        protected virtual void OnDisable()
        {
            UNlistenForEvents();
        }

        protected virtual void UNlistenForEvents()
        {
            _events.PointerUp -= OnPointerEventTriggered;
            _events.PointerDown -= OnPointerEventTriggered;
            _events.PointerClick -= OnPointerEventTriggered;

            _events.PointerEnter -= OnPointerEventTriggered;
            _events.PointerExit -= OnPointerEventTriggered;

            _events.BeginDrag -= OnPointerEventTriggered;
            _events.Drag -= OnPointerEventTriggered;
            _events.EndDrag -= OnPointerEventTriggered;
            _events.Drop -= OnPointerEventTriggered;
        }

    }
}
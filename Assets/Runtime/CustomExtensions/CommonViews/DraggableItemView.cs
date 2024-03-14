using System;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Core.Runtime.CommonViews
{
    [RequireComponent(typeof(Collider2D))]
    public class DraggableItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<Vector2> DragBegin = delegate { };

        public event Action<Vector2> Dragging = delegate { };

        public event Action DragEnd = delegate { };

        private Camera _mainCamera;

        public void AssignCamera(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragBegin.Invoke(GetWorldPosition(eventData.position));
        }

        public void OnDrag(PointerEventData eventData)
        {
            Dragging.Invoke(GetWorldPosition(eventData.position));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragEnd.Invoke();
        }

        private Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            Assert.IsNotNull(_mainCamera);

            return _mainCamera.ScreenToWorldPoint(screenPosition);
        }
    }
}
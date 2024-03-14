using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core.Runtime.CommonViews
{
    [RequireComponent(typeof(Collider2D))]
    public class GameObjectButtonView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent onClick;
        public UnityEvent onPointerDown;
        public UnityEvent onPointerUp;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            onPointerUp.Invoke();
        }
    }
}
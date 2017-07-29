using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ARPP {
	public class Joystick : MonoBehaviour,IDragHandler, IPointerUpHandler, IPointerDownHandler {

		public Vector2 axis { get; private set; }

		public bool lockX = false;
		public bool lockY = false;

		private RectTransform container;
		private RectTransform joystick;

		void Start() {
			container = GetComponent<RectTransform>();
			joystick = transform.GetChild(0).GetComponent<RectTransform>();
			axis = Vector2.zero;
		}

		public void OnPointerDown(PointerEventData ped) {
			OnDrag(ped);
		}

		public void OnDrag(PointerEventData ped){
			Vector2 position = Vector2.zero;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				container, ped.position, ped.pressEventCamera, out position);

			// zero-out the changes in a locked axis or set the position
			position.x = lockX ? 0 : (position.x / container.sizeDelta.x);
			position.y = lockY ? 0 : (position.y / container.sizeDelta.y);

			axis = new Vector2(position.x * 2, position.y * 2);
			axis = (axis.magnitude > 1) ? axis.normalized : axis;

			// set the area that the joystick can move around in
			joystick.anchoredPosition = new Vector2(
				axis.x * container.sizeDelta.x / 3,
				axis.y * container.sizeDelta.y / 3);
		}

		public void OnPointerUp(PointerEventData ped) {
			// reset the joystick to (0,0)
			axis = Vector2.zero;
			joystick.anchoredPosition = Vector2.zero;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPP {	
	public class ObjectManipulator : MonoBehaviour {

		public static ObjectManipulator Instance { get; private set; }

		public GameObject controls;
		public bool useJoysticks;
		public float translationalSpeed = 0.5f;
		public Vector3 scalingStep = Vector3.one * 0.5f;
		public float rotationSpeed = 45f;

		InteractableObject focusedObject = null;

		public Joystick joystickHorizontalPos;
		public Joystick joystickScale;
		public Joystick joystickVerticalPos;
		public Joystick joystickRotation;

		void Start() {
			Instance = this;
		}

		void Update() {
			if (useJoysticks && controls.activeInHierarchy && focusedObject != null) {
				if (joystickHorizontalPos != null && joystickHorizontalPos.axis != Vector2.zero) {
					Vector3 direction = new Vector3(joystickHorizontalPos.axis.x, 0, joystickHorizontalPos.axis.y);
					float distance = translationalSpeed * Time.deltaTime;
					focusedObject.Move(direction, distance);
				}
				if (joystickScale != null && joystickScale.axis != Vector2.zero) {
					// one joystick axis will be locked, so just add em together
					// and make sure the value is less than 1
					float intensity = joystickScale.axis.x + joystickScale.axis.y;
					if (intensity > 1) { intensity = 1; }
					focusedObject.ScaleLinear(intensity * scalingStep * Time.deltaTime);
				}
				if (joystickVerticalPos != null && joystickVerticalPos.axis != Vector2.zero) {
					focusedObject.Move(joystickVerticalPos.axis.y * Vector3.up * Time.deltaTime);
				}
				if (joystickRotation != null && joystickRotation.axis != Vector2.zero) {
					// one joystick axis will be locked, so just add em together
					// and make sure the value is less than 1
					float intensity = joystickRotation.axis.x + joystickScale.axis.y;
					if (intensity > 1) { intensity = 1; }
					focusedObject.RotateCW(intensity * rotationSpeed * Time.deltaTime);
				}
			}

		}

		public void ToggleManipulation(InteractableObject obj) {
			if (focusedObject == obj) {
				// already focused on this object, toggle off
				DisableManipulation();
				return;
			}
			// focusedObject is either different or null, toggle on controls
			EnableManipulation(obj);
		}

		public void EnableManipulation(InteractableObject obj) {
			// set the focus of the controls to obj
			// and toggle on the controls (if not on already)
			focusedObject = obj;
			controls.SetActive(true);
		}

		public void DisableManipulation() {
			focusedObject = null;
			controls.SetActive(false);
		}

		public void OnClickForward() {
			if (focusedObject == null) { return; }
			focusedObject.Move(Vector3.forward, translationalSpeed);
		}

		public void OnClickBackwards() {
			if (focusedObject == null) { return; }
			focusedObject.Move(Vector3.back, translationalSpeed);
		}

		public void OnClickLeft() {
			if (focusedObject == null) { return; }
			focusedObject.Move(Vector3.left, translationalSpeed);
		}

		public void OnClickRight() {
			if (focusedObject == null) { return; }
			focusedObject.Move(Vector3.right, translationalSpeed);
		}

		public void OnClickUp() {
			if (focusedObject == null) { return; }
			focusedObject.Move(Vector3.up, translationalSpeed);
		}

		public void OnClickDown() {
			if (focusedObject == null) { return; }
			focusedObject.Move(Vector3.down, translationalSpeed);
		}

		public void OnClickRotateCW() {
			if (focusedObject == null) { return; }
			focusedObject.RotateCW();
		}

		public void OnClickRotateCCW() {
			if (focusedObject == null) { return; }
			focusedObject.RotateCCW();
		}

		public void OnClickScaleUp() {
			if (focusedObject == null) { return; }
			focusedObject.ScaleLinear(scalingStep);
		}

		public void OnClickScaleDown() {
			if (focusedObject == null) { return; }
			focusedObject.ScaleLinear(-scalingStep);
		}

		public void OnClickDuplicate() {
			if (focusedObject == null) { return; }
			Instantiate(focusedObject.gameObject);
		}

		public void OnClickDestroy() {
			if (focusedObject == null) { return; }
			Destroy(focusedObject.gameObject);
			DisableManipulation();
		}

	}
}

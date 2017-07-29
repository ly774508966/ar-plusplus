using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ARPP {
	public class TouchListener : MonoBehaviour {

		public static TouchListener Instance { get; private set; }

		public bool isListening = true;

		Camera cam;

		void Start() {
			Instance = this;
			cam = Camera.main;
		}

		void Update() {
			// allow for in-editor debugging
			#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(0)) {
				TryRaycast(Input.mousePosition);
			}
			#endif

			if (Input.touchCount > 0) {
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began) {
					TryRaycast(touch.position);
				}
			}
		}

		void TryRaycast(Vector3 touch) {
			if(IsTouchOverUIObject(touch)) {
				// touched a UI element, now prevent the raycast from passing through
				Debug.Log("TouchListener: UI element blocked raycast");
				return;
			}
			// send a raycast from the point touched on the screen
			Ray ray = cam.ScreenPointToRay(touch);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				// check if any Interaction components are hit
				List<Interaction> hitInteractions = new List<Interaction>();
				hitInteractions.AddRange(hit.collider.gameObject.GetComponents<Interaction>());
				if (hitInteractions.Count > 0) {
					// object hit does contain Interactions
					foreach (Interaction interaction in hitInteractions) {
						interaction.OnTouchDown();
						Debug.Log("TouchListener: Interaction interaction.OnTouchDown()");
					}
				}
			}
		}


		// Determines whether the specified touch is over a UI object
		bool IsTouchOverUIObject(Vector3 touch) {
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = touch;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}
	}
}
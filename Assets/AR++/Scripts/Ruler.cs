using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARPP {
	public class Ruler : MonoBehaviour {

		const float INCHES_PER_METER = 39.3701f;

		public bool isRulerEnabled = true;
		public bool isMetric = true;
		public Text measurementText;
		public Material rulerMaterial;

		Vector3 point1, point2;
		bool isMeasuring;
		GameObject line;

		void Update() {
			if (!isRulerEnabled) { return; }

			// allow for in-editor debugging
			#if UNITY_EDITOR
			if (Input.GetMouseButtonDown(0)) {
				StartMeasurement(GetPointFromTouch(Input.mousePosition));
			} else if (Input.GetMouseButton(0)) {
				UpdateMeasurement(GetPointFromTouch(Input.mousePosition));
			} else if (Input.GetMouseButtonUp(0)) {
				StopMeasurement(GetPointFromTouch(Input.mousePosition));
			}
			#endif

			if (Input.touchCount > 0) {
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began) {
					StartMeasurement(GetPointFromTouch(touch.position));
				} else if (touch.phase == TouchPhase.Moved) {
					UpdateMeasurement(GetPointFromTouch(touch.position));
				} else if (touch.phase == TouchPhase.Ended) {
					StopMeasurement(GetPointFromTouch(touch.position));
				}
			}
		}

		// try to project the from a point on the touchscreen to a point in the world
		Vector3 GetPointFromTouch(Vector3 touch) {
			// send a raycast from the point touched on the screen
			Ray ray = Camera.main.ScreenPointToRay(touch);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				// we hit a collider somewhere out in 3D space
				// must have tapped on a virtual object
				return hit.point;
			}
			// didn't hit anything with the raycast
			// convert the tap on the screen to world coordinates
			touch.z = 0.1f; // project the tap 0.1m in front of camera
			return Camera.main.ScreenToWorldPoint(touch);
		}

		public void StartMeasurement(Vector3 point) {
			point1 = point;
			Debug.Log(point);
			isMeasuring = true;
			if (line != null) { DestroyImmediate(line); }
			line = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Destroy(line.GetComponent<BoxCollider>());
			if (rulerMaterial != null) {
				line.GetComponent<MeshRenderer>().material = rulerMaterial;
			}
			line.transform.position = point1;
			line.transform.localScale = Vector3.one * 0.01f;
			UpdateText();
			Debug.Log("Ruler: StartMeasurement()");
		}

		public void UpdateMeasurement(Vector3 point) {
			if (!isMeasuring) { return; }
			point2 = point;
			// put the line at the midpoint between p1 & p2
			float x = (point1.x + point2.x) / 2;
			float y = (point1.y + point2.y) / 2;
			float z = (point1.z + point2.z) / 2;
			line.transform.position = new Vector3(x, y, z);
			// scale the line to be Distance(p1,p2) in length
			line.transform.localScale = new Vector3(
				line.transform.localScale.x , line.transform.localScale.y, GetDistance());
			// rotate the line to be connecting p1 & p2
			Vector3 rotDirection = (point - line.transform.position).normalized;
			line.transform.rotation = Quaternion.LookRotation(rotDirection);

			UpdateText();
		}

		public void StopMeasurement(Vector3 point) {
			UpdateMeasurement(point); // one last time
			UpdateText();
			isMeasuring = false;
			Debug.Log("Ruler: StopMeasurement()");
		}

		// returns distance between p1 & p2
		public float GetDistance() {
			if (point1 == null || point2 == null) {
				return 0;
			}
			float distance = Vector3.Distance(point1, point2);
			return isMetric ?  distance : distance * INCHES_PER_METER;
		}

		// returns distance between p1 & p2 w/ the specified number of decimal places
		public float GetDistance(int decimalPlaces) {
			return (float)Math.Round(GetDistance(), decimalPlaces);
		}

		void UpdateText() {
			if (measurementText == null) { return; }
			string units = isMetric ? " m" : " in";
			measurementText.text = GetDistance(3) + units;
		}

	}
}

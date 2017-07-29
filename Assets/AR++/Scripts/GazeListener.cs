using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPP {
	public class GazeListener : MonoBehaviour {

		public bool isReticleSystemEnabled;

		public static GazeListener Instance { get; private set; }

		Transform cam;
		GameObject reticleImage;

		List<Interaction> focusedInteractions = new List<Interaction>();

		void Start() {
			Instance = this;
			cam = Camera.main.transform;
			reticleImage = transform.Find("ReticleImage").gameObject;
			reticleImage.SetActive(isReticleSystemEnabled);
		}

		void Update() {
			if (!isReticleSystemEnabled) {
				return;
			}

			RaycastHit hit;
			float distance;
			// send a raycast and see what is hit
			if (Physics.Raycast(new Ray(cam.position, cam.rotation * Vector3.forward), out hit)) {
				// check if any Interaction components are hit
				List<Interaction> hitInteractions = new List<Interaction>();
				hitInteractions.AddRange(hit.collider.gameObject.GetComponents<Interaction>());
				if (hitInteractions.Count > 0) {
					// object hit does contain Interactions
					foreach (Interaction interaction in hitInteractions) {
						if (interaction.hasReticleFocus) {
							interaction.OnGazeStay();
							continue;
						}
						interaction.OnGazeEnter();
						Debug.Log("GazeListener: Interaction interaction.OnGazeEnter()");
						// cleanup previously focused interactions
						ClearFocusedInteractions();
					}
					focusedInteractions = hitInteractions;
				}
				else {
					// didn't hit a GazeInteraction w/ this raycast
					// but our last raycast might have
					ClearFocusedInteractions();
				}

				// Calculate how where the reticle should be placed
				distance = hit.distance;
				transform.position = cam.position
					+ cam.rotation * Vector3.forward * distance
					+ hit.normal * 0.01f;
				transform.rotation = Quaternion.LookRotation(hit.normal);
				if (distance < 10.0f) {
					distance *= 1 + 5 * Mathf.Exp(-distance);
				}
				// Scale the reticle depending on how far away the object is
				transform.localScale = Vector3.one * distance;

				reticleImage.SetActive(true);
			}
			else {
				// didn't hit anything, send OnGazeReticleExit message if necessary
				ClearFocusedInteractions();
				reticleImage.SetActive(false);
			}
		}

		void ClearFocusedInteractions() {
			foreach (Interaction interaction in focusedInteractions) {
				interaction.OnGazeExit();
				Debug.Log("GazeListener: Interaction interaction.OnGazeExit()");
			}
			focusedInteractions.Clear();
		}
	}
}

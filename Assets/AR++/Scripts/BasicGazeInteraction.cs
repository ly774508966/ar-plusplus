using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ARPP {
	public class BasicGazeInteraction : Interaction {

		public UnityEvent eventsOnGazeEnter;
		public UnityEvent eventsOnGazeStay;
		public UnityEvent eventsOnGazeExit;

		public override void OnGazeEnter() {
			base.OnGazeEnter();
			eventsOnGazeEnter.Invoke();
		}

		public override void OnGazeStay() {
			base.OnGazeStay();
			eventsOnGazeStay.Invoke();
		}

		public override void OnGazeExit() {
			base.OnGazeExit();
			eventsOnGazeExit.Invoke();
		}

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ARPP {
	public class BasicTouchInteraction : Interaction {

		public UnityEvent eventsOnTouchDown;
		public UnityEvent eventsOnTouchStay;
		public UnityEvent eventsOnTouchUp;

		public override void OnTouchDown() {
			base.OnTouchDown();
			eventsOnTouchDown.Invoke();
		}

		public override void OnTouchStay() {
			base.OnTouchStay();
			eventsOnTouchStay.Invoke();
		}

		public override void OnTouchUp() {
			base.OnTouchUp();
			eventsOnTouchUp.Invoke();
		}
	}
}

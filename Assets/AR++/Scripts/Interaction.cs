using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPP {
public class Interaction : MonoBehaviour {
		
		public bool isTouched { get; private set; }
		public bool hasReticleFocus { get; private set; }

		public virtual void OnTouchDown() 	{ isTouched = true; }
		public virtual void OnTouchStay() 	{ }
		public virtual void OnTouchUp() 	{ isTouched = false; }
			
		public virtual void OnGazeEnter()	{ hasReticleFocus = true; }
		public virtual void OnGazeStay()	{ }
		public virtual void OnGazeExit()	{ hasReticleFocus = false; }

	}
}

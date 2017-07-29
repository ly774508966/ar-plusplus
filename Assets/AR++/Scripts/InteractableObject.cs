using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPP {
	public class InteractableObject : Interaction {

		public bool moveRelativeToPlayer = true;

		Transform player;

		public void Move(Vector3 direction, float stepSize = 0.5f) {
			// lazy load the player if needed
			if (moveRelativeToPlayer && player == null) {
				player = Camera.main.transform;
			}

			// if movement is in vertical axis
			if (direction.normalized == Vector3.up || direction.normalized == Vector3.down) {
				// no need to worry about rotation
				transform.position += direction * stepSize;
				return;
			}

			// if movement is in horizontal plane
			Quaternion rotation = Quaternion.Euler(0, player.rotation.eulerAngles.y, 0);
			Vector3 displacement = moveRelativeToPlayer ?
				rotation * direction * stepSize :
				direction * stepSize;
			transform.position += new Vector3(
				displacement.x, 0, displacement.z);
		}

		public void ScaleLinear(Vector3 step) {
			transform.localScale += step;
		}

		public void RotateCW(float stepSize = 10f) {
			transform.Rotate(0, stepSize, 0);
		}

		public void RotateCCW(float stepSize = 10f) {
			transform.Rotate(0, -stepSize, 0);
		}

	}
}

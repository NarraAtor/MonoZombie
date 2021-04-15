using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

// Author : Frank Alfano
// Purpose : This class makes sure one game object is at the center of the screen and all the others
//			 move as it moves, creating a "camera effect". 

namespace MonoZombie {
	public class Camera {
		public GameObject Target {
			get;
			private set;
		}

		public Camera (GameObject target) {
			Target = target;
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Calculate the position of another game object based on the target game object
		 * 
		 * GameObject other				: The other game object to calculate the position of
		 * 
		 * return Vector2				: The screen position to set the object to
		 */
		public Vector2 CalculateScreenPosition (GameObject other) {
			if (other == Target) {
				return Game1.ScreenDimensions / 2;
			}

			// Calculate the pixel difference between the target and other game object camera positions
			Vector2 cameraOffset = new Vector2(other.CamX - Target.CamX, other.CamY - Target.CamY);

			// The position of other objects on the screen are relative to the target game object, so the position of the other game
			// object would be the target position plus the camera difference
			return new Vector2(Target.X + cameraOffset.X, Target.Y + cameraOffset.Y);
		}
	}
}

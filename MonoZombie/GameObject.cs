using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

// Author : Frank Alfano
// Purpose : The base class for all objects in the game

namespace MonoZombie {
	public abstract class GameObject {
		protected Texture2D texture;
		protected Vector2 centerPosition;
		protected bool canRotate;
		protected bool canMove;

		public Rectangle Rect {
			get {
				return SpriteManager.GetBoundingRect(texture, centerPosition, SpriteManager.ObjectScale);
			}
		}

		public int X {
			get {
				return (int) centerPosition.X;
			}

			set {
				if (canMove) {
					centerPosition.X = value;
				}
			}
		}

		public int Y {
			get {
				return (int) centerPosition.Y;
			}

			set {
				if (canMove) {
					centerPosition.Y = value;
				}
			}
		}

		public float Angle {
			get;
			private set;
		}

		public GameObject (Texture2D texture, Vector2 centerPosition, bool canRotate = false, bool canMove = true) {
			this.texture = texture;
			this.centerPosition = centerPosition;
			this.canRotate = canRotate;
			this.canMove = canMove;
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * An overridable method that is used to update the game object
		 * 
		 * GameTime gameTime		: Used to get the current time in the game
		 * MouseState mouse			: The current state of the mouse
		 * KeyboardState keyboard	: The current state of the keyboard
		 * 
		 * return					:
		 */
		public virtual void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {

		}

		/*
		 * Author : Frank Alfano
		 * 
		 * An overridable method that is used to draw the game object
		 * * This method needs to be called within a SpriteBatch Begin() and End() draw methods
		 * 
		 * SpriteBatch spriteBatch	: The SpriteBatch object used to draw textures for the game
		 * 
		 * return					:
		 */
		public virtual void Draw (SpriteBatch spriteBatch) {
			// Make sure the texture is not null before trying to draw it
			if (texture != null) {
				SpriteManager.DrawImage(spriteBatch, texture, Rect, angle: Angle);
			}
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Rotate the game object to face a certain object
		 * 
		 * Point face				: The point to face the object towards
		 * 
		 * return					: 
		 */
		public void RotateTo (Vector2 face) {
			// Make sure the object can rotate before doing the calculations to rotate it
			if (canRotate) {
				// Get the position of the other object with the game object as the origin instead of the top left corner of the screen
				Vector2 posObjectAsOrigin = new Vector2(face.X - X, Y - face.Y);

				// If the other point is not directly on top of the player, continue with the calculations
				// This is to make sure we don't divide by 0 and crash the game
				if (posObjectAsOrigin != Vector2.Zero) {
					// Get this distance between the player and the other position
					float distanceToPoint = Distance(face, centerPosition);

					// Calculate the angle between the other point and the player
					// The reason this is done twice is because Cos is always positive and Sin is both positive and negative.
					// Sin is used to determine how much to adjust the Cos angle because Cos only goes from 0-3.14 (PI) when we need it
					// to go all the way from 0-6.28 (2PI)
					float sinAngle = (float) Math.Asin(posObjectAsOrigin.Y / distanceToPoint);
					float cosAngle = (float) Math.Acos(posObjectAsOrigin.X / distanceToPoint);

					// The is used to determine whether the sin angle is positive or negative
					int sinMod = (int) -(sinAngle / Math.Abs(sinAngle));

					// If either of the angles are negative, do not calculate the angle because it will just be 0
					if (sinAngle != 0 && cosAngle != 0) {
						// Set the rotation based on the calculated angle
						Angle = (float) Math.PI + (sinMod * cosAngle);
					}
				}
			}
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Check collision as well as update the position of both colliding gameobjects based on the collision
		 * 
		 * GameObject other			: The other game object to check collision with
		 */
		public bool CheckUpdateCollision (GameObject other) {
			// Make sure this game object can move before trying to check collision
			// If a game object cannot move, other objects can still check collision with it (for example, map tiles cant move but still have collision)
			//if (canMove) {
			// Get the intersect rectangle between this game objects rectangle collider and the other game object's rectangle collider
			Rectangle intersectRect = Rectangle.Intersect(other.Rect, Rect);

			if (intersectRect.Size == Point.Zero) {
				return false;
			}

			// *** When objects collide, it might be better to have both objects (if they can both move) to move in opposite directions. This would give a
			// sort of "pushing" effect which could add to more realistic collisions

			if (intersectRect.Width <= intersectRect.Height) {
				// If the rectangle X coordinates are equal, then this game object needs to move to the right
				// If the intersect rectangle X coordinate is greater than this game objects X position, then this game object needs to move to the left
				int mod = (intersectRect.X == Rect.X) ? 1 : -1;

				if (other.canMove) {
					X += mod * (intersectRect.Width / 2);
					other.X -= mod * (intersectRect.Width / 2);
				} else {
					X += mod * intersectRect.Width;
				}
			} else {
				// If the rectangle Y coordinates are equal, then this game object needs to move down
				// If the intersect rectangle Y coordinate is greater than this game object Y position, then this game object needs to move up
				int mod = (intersectRect.Y == Rect.Y) ? 1 : -1;

				if (other.canMove) {
					Y += mod * (intersectRect.Height / 2);
					other.Y -= mod * (intersectRect.Height / 2);
				} else {
					Y += mod * intersectRect.Height;
				}
			}
			//}

			return true;
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Get the distance (in pixels) between 2 points
		 * 
		 * Point point1				: The first point
		 * Point point2				: The second point
		 * 
		 * return double			: The distance (in pixels) between the two points
		 */
		protected float Distance (Vector2 point1, Vector2 point2) {
			return MathF.Sqrt(MathF.Pow(point1.X - point2.X, 2) + MathF.Pow(point1.Y - point2.Y, 2));
		}
	}
}

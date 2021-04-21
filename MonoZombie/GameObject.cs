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

		protected float moveSpeed;

		public Rectangle Rect {
			get {
				return SpriteManager.GetBoundingRect(texture, centerPosition, SpriteManager.ObjectScale);
			}
		}

		public int CamX {
			get;
			private set;
		}

		public int CamY {
			get;
			private set;
		}

		public Vector2 Position {
			get {
				return centerPosition;
			}
		}

		// The game object's pixel position X value on the screen
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

		// The game object's pixel position Y value on the screen
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

		// The angle that the game object is rotated to in radians
		public float Angle {
			get;
			protected set;
		}

		public GameObject (Texture2D texture, Vector2 centerPosition, GameObject parent = null, float moveSpeed = 0, bool canRotate = false, bool canMove = true) {
			this.texture = texture;
			this.centerPosition = centerPosition;
			this.canRotate = canRotate;
			this.canMove = canMove;
			this.moveSpeed = moveSpeed;

			// Calculate the initial camera position
			if (parent != null) {
				CamX = parent.CamX;
				CamY = parent.CamY;
			} else {
				CamX = X - (int) (Main.SCREEN_DIMENSIONS / 2).X;
				CamY = Y - (int) (Main.SCREEN_DIMENSIONS / 2).Y;
			}
		}

		/*
		 * * NOTE * All game objects need to call the Update and Draw methods in order to function correctly
		 */

		/*
		 * Author : Frank Alfano
		 * 
		 * An overridable method that is used to update the game object
		 * 
		 * GameTime gameTime		: Used to get the current time in the game
		 * MouseState mouse			: The current state of the mouse
		 * KeyboardState keyboard	: The current state of the keyboard
		 * Camera camera			: The camera object in the Main class
		 * 
		 * return					:
		 */
		public virtual void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Update this game objects position based on the camera
		 * 
		 * Camera camera			: The camera object
		 * 
		 * return					:
		 */
		public void UpdateCameraScreenPosition (Camera camera) {
			// Update the position of the game object based on the target game object
			centerPosition = camera.CalculateScreenPosition(this);
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
					float distanceToPoint = Main.Distance(face, centerPosition);

					// Calculate the angle between the other point and the player
					// The reason this is done twice is because Cos is always positive and Sin is both positive and negative.
					// Sin is used to determine how much to adjust the Cos angle because Cos only goes from 0-3.14 (PI) when we need it
					// to go all the way from 0-6.28 (2PI)
					float sinAngle = MathF.Asin(posObjectAsOrigin.Y / distanceToPoint);
					float cosAngle = MathF.Acos(posObjectAsOrigin.X / distanceToPoint);

					// The is used to determine whether the sin angle is positive or negative
					int sinMod = (int) -(sinAngle / MathF.Abs(sinAngle));

					// If either of the angles are negative, do not calculate the angle because it will just be 0
					if (sinAngle != 0 && cosAngle != 0) {
						// Set the rotation based on the calculated angle
						Angle = (MathF.PI / 2) + (sinMod * cosAngle);
					}
				}
			}
		}

		public void MoveBy (Vector2 movement) {
			if (canMove) {
				X += (int) movement.X;
				Y += (int) movement.Y;

				CamX += (int) movement.X;
				CamY += (int) movement.Y;
			}
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Removes (or destroys) an object from its list in the Main class
		 * 
		 * return bool							: Whether or not the object was successfully destroyed
		 */
		public bool Destroy ( ) {
			if (typeof(Bullet).IsInstanceOfType(this)) {
				Main.ListOfBullets.Remove((Bullet) this);
				Console.WriteLine($"{Position}");
			} else if (typeof(Enemy).IsInstanceOfType(this)) {
				Main.ListOfZombies.Remove((Enemy) this);
			} else if (typeof(Turret).IsInstanceOfType(this)) {
				Main.ListOfTurrets.Remove((Turret) this);
			} else {
				return false;
			}

			return true;
		}

		/*
		 * Author : Frank Alfano, Matthew Sorrentino
		 * 
		 * Check collision as well as update the position of both colliding gameobjects based on the collision
		 * 
		 * GameObject other			: The other game object to check collision with
		 * 
		 * return bool				: Whether or not the game object has collided with something
		 */
		public bool CheckUpdateCollision (GameObject other) {
			// Get the intersect rectangle between this game objects rectangle collider and the other game object's rectangle collider
			Rectangle intersectRect = Rectangle.Intersect(other.Rect, Rect);

			if (intersectRect.Size == Point.Zero) {
				return false;
			}

			// *** When objects collide, it might be better to have both objects (if they can both move) to move in opposite directions. This would give a
			// sort of "pushing" effect which could add to more realistic collisions

			// Vectors that hold the values that each of the game objects involved in the collision move by
			Vector2 moveThisBy = Vector2.Zero;
			Vector2 moveOtherBy = Vector2.Zero;

			if (intersectRect.Width <= intersectRect.Height) {
				// If the rectangle X coordinates are equal, then this game object needs to move to the right
				// If the intersect rectangle X coordinate is greater than this game objects X position, then this game object needs to move to the left
				int mod = (intersectRect.X == Rect.X) ? 1 : -1;

				if (canMove && other.canMove) {
					moveThisBy.X += mod * (intersectRect.Width / 2f);
					moveOtherBy.X -= mod * (intersectRect.Width / 2f);
				} else if (canMove) {
					moveThisBy.X += mod * intersectRect.Width;
				} else {
					moveOtherBy.X -= mod * intersectRect.Width;
				}
			} else {
				// If the rectangle Y coordinates are equal, then this game object needs to move down
				// If the intersect rectangle Y coordinate is greater than this game object Y position, then this game object needs to move up
				int mod = (intersectRect.Y == Rect.Y) ? 1 : -1;

				if (canMove && other.canMove) {
					moveThisBy.Y += mod * (intersectRect.Height / 2f);
					moveOtherBy.Y -= mod * (intersectRect.Height / 2f);
				} else if (canMove) {
					moveThisBy.Y += mod * intersectRect.Height;
				} else {
					moveOtherBy.Y -= mod * intersectRect.Height;
				}
			}

			// Move the objects based on the calculated values above
			MoveBy(moveThisBy);
			other.MoveBy(moveOtherBy);

			return true;
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Just checks to see if this game object is colliding with another one without update its position
		 */
		public bool CheckCollision (GameObject other) {
			return (Rectangle.Intersect(other.Rect, Rect).Size != Point.Zero);
		}
	}
}

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

		protected bool canRotate;
		protected bool canMove;
		protected float moveSpeed;

		public Rectangle Rect {
			get {
				return SpriteManager.GetBoundingRect(texture, Position, SpriteManager.OBJECT_SCALE);
			}
		}
		
		public bool IsOnScreen {
			get {
				return (Rect.Right >= 0 && Rect.Left <= Main.SCREEN_DIMENSIONS.X && Rect.Bottom >= 0 && Rect.Top <= Main.SCREEN_DIMENSIONS.Y);
			}
		}

		public Vector2 Position {
			get;
			private set;
		}

		public Vector2 CameraPosition {
			get;
			private set;
		}

		public float Angle {
			get;
			protected set;
		}

		public GameObject (Texture2D texture, Vector2 centerPosition, GameObject parent = null, float moveSpeed = 0, bool canRotate = false, bool canMove = true) {
			this.texture = texture;
			Position = centerPosition;

			this.canRotate = canRotate;
			this.canMove = canMove;
			this.moveSpeed = moveSpeed;

			// Calculate the initial camera position
			if (parent != null) {
				CameraPosition = parent.CameraPosition;
			} else {
				CameraPosition = centerPosition - Main.SCREEN_DIMENSIONS / 2;
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
		 * An overridable method that is used to draw the game object
		 * * This method needs to be called within a SpriteBatch Begin() and End() draw methods
		 * 
		 * SpriteBatch spriteBatch	: The SpriteBatch object used to draw textures for the game
		 * 
		 * return					:
		 */
		public virtual void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			if (IsOnScreen) {
				SpriteManager.DrawImage(spriteBatch, texture, Rect, Color.White, angle: Angle);
			}
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
			Position = camera.CalculateScreenPosition(this);
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
			if (canRotate && IsOnScreen) {
				Vector2 A = Position + new Vector2(0, Vector2.Distance(Position, face));
				Vector2 B = face;
				Vector2 C = Position;

				float dirC2A = MathF.Atan2(A.Y - C.Y, A.X - C.X);
				float dirC2B = MathF.Atan2(B.Y - C.Y, face.X - C.X);
				float angleABC = dirC2B - dirC2A;

				Angle = angleABC + MathF.PI;
			}
		}

		public void MoveBy (Vector2 moveBy) {
			if (canMove) {
				Position += moveBy;
				CameraPosition += moveBy;
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
			} else if (typeof(Zombie).IsInstanceOfType(this)) {
				Main.ListOfZombies.Remove((Zombie) this);

				// CHANGE 10 TO LIKE A RANDOM NUMBER OR SOMETHING
				Main.currency += 10;
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

			// Whether or not the move the objects in either the x and y direction. These variables depend on the width and height of the intersecting rectangle
			bool doMoveX = (intersectRect.Width < intersectRect.Height);
			bool doMoveY = (intersectRect.Width > intersectRect.Height);

			if (doMoveX) {
				// If the rectangle X coordinates are equal, then this game object needs to move to the right
				// If the intersect rectangle X coordinate is greater than this game objects X position, then this game object needs to move to the left
				int mod = ((intersectRect.X == Rect.X) ? 1 : -1);

				if (canMove && other.canMove) {
					moveThisBy.X += mod * (intersectRect.Width / 2f);
					moveOtherBy.X -= mod * (intersectRect.Width / 2f);
				} else if (canMove) {
					moveThisBy.X += mod * intersectRect.Width;
				} else {
					moveOtherBy.X -= mod * intersectRect.Width;
				}
			}

			if (doMoveY) {
				// If the rectangle Y coordinates are equal, then this game object needs to move down
				// If the intersect rectangle Y coordinate is greater than this game object Y position, then this game object needs to move up
				int mod = ((intersectRect.Y == Rect.Y) ? 1 : -1);

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

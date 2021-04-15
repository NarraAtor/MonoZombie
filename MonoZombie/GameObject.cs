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
		protected Vector2 position;
		protected bool canRotate;
		protected float angle;

		public Rectangle Rect {
			get {
				// Calculate the dimensions and position of the collision rectangle
				int rectWidth = (int) (texture.Width * SpriteManager.ObjectScale);
				int rectHeight = (int) (texture.Height * SpriteManager.ObjectScale);
				int rectX = X;
				int rectY = Y;

				// If the object can rotate, the position that it is drawn at is going to be at the center of the image
				// because of how Monogame rotates images. Therefore, if the image cant be rotated (for example, a map tile),
				// we need to adjust the rectangle position so this rectangle correctly detects collision.
				if (canRotate) {
					rectX -= rectWidth / 2;
					rectY -= rectHeight / 2;
				}

				return new Rectangle(rectX, rectY, rectWidth, rectHeight);
			}
		}

		public int X {
			get {
				return (int) position.X;
			}

			set {
				position.X = value;
			}
		}

		public int Y {
			get {
				return (int) position.Y;
			}

			set {
				position.Y = value;
			}
		}

		//In radians
		public float Angle {
			get {
				return angle;
			}
		}

		public GameObject (Texture2D texture, Vector2 position, bool canRotate = false) {
			this.texture = texture;
			this.position = position;
			this.canRotate = canRotate;
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
		 * GameTime gameTime		: Used to get the current time in the game
		 * SpriteBatch spriteBatch	: The SpriteBatch object used to draw textures for the game
		 * 
		 * return					:
		 */
		public virtual void Draw (SpriteBatch spriteBatch) {
			SpriteManager.DrawImage(spriteBatch, texture, position, angle: Angle, scale: SpriteManager.ObjectScale);
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
			// Get the position of the other object with the game object as the origin instead of the top left corner of the screen
			Vector2 posObjectAsOrigin = new Vector2(face.X - X, Y - face.Y);

			// If the other point is not directly on top of the player, continue with the calculations
			// This is to make sure we don't divide by 0 and crash the game
			if (posObjectAsOrigin != Vector2.Zero) {
				// Get this distance between the player and the other position
				float distanceToPoint = Distance(face, position);

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
					angle = (float) Math.PI + (sinMod * cosAngle);
				}
			}
		}

		public bool CheckCollision (GameObject other) {
			return other.Rect.Intersects(Rect);
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

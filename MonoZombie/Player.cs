using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Author : Frank Alfano, Eric Fotang, Jack Shyshko
// Purpose : Manages player values and mechanics like shooting

namespace MonoZombie {
	public class Player : GameObject {
		protected float timeSinceLastDamage;
		protected float timeSinceLastAttack;
		protected float attacksPerSecond;

		public int Health {
			get;
			private set;
		}

		public bool IsDead {
			get {
				return (Health <= 0);
			}
		}

		public bool CanAttack {
			get {
				return (timeSinceLastAttack >= 1 / attacksPerSecond);
			}
		}

		public Player (Texture2D texture, Vector2 position, int health, float playerSpeed, float attacksPerSecond) : base(texture, position, moveSpeed: playerSpeed, canRotate: true) {
			Health = health;
			this.attacksPerSecond = attacksPerSecond;
		}

		public void TakeDamage (int damage) {
			Health -= damage;

			timeSinceLastDamage = 0;
		}

		/*
		 * Author : Frank Alfano, Jack Shyshko
		 * 
		 * * Overridden from the base GameObject class
		 */
		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			// Update the last time since this game object has attacked
			timeSinceLastAttack += (float) gameTime.ElapsedGameTime.TotalSeconds;
			timeSinceLastDamage += (float) gameTime.ElapsedGameTime.TotalSeconds;

			// Move the player based on keyboard input
			Move(keyboard);

			// Rotate the player to look at the mouse
			RotateTo(mouse.Position.ToVector2( ));

			// If the player can attack and they are pressing the left mouse button, shoot a bullet
			if (CanAttack) {
				if (mouse.LeftButton == ButtonState.Pressed) {
					Main.ListOfBullets.Add(new Bullet(Main.bulletTexture, Position, this, Angle, 15));

					timeSinceLastAttack = 0;
				}
			}
        }

		public new void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			Color damageTint = (timeSinceLastDamage < Main.DAMAGE_INDIC_TIME) ? Color.Red : Color.White;

			SpriteManager.DrawImage(spriteBatch, texture, Rect, damageTint, angle: Angle);
		}

		/*
         * Author : Frank Alfano, Jack Shyshko
         * 
         * Move the player based on keyboard input
         * 
         * KeyboardState keyboard               : The current keyboard state
         * 
         * return                               :
         */
		public void Move (KeyboardState keyboard) {
			// Get which direction the player is trying to move
			int moveX = (keyboard.IsKeyDown(Keys.D) ? 1 : 0) + (keyboard.IsKeyDown(Keys.A) ? -1 : 0);
			int moveY = (keyboard.IsKeyDown(Keys.W) ? -1 : 0) + (keyboard.IsKeyDown(Keys.S) ? 1 : 0);

			// Get the movement vector of the player and make sure it is normalized
			// Normalizing the vector makes sure that when the player is moving diagonally they are moving the same
			// speed as if the player was just moving in 1 direction
			Vector2 normMovement = new Vector2(moveX, moveY);
			if (normMovement != Vector2.Zero) {
				normMovement.Normalize( );
			}

			// Move the position of the player
			MoveBy(normMovement * moveSpeed);
		}
	}
}

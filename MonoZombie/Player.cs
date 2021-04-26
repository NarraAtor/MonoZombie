using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Author : Frank Alfano, Eric Fotang, Jack Shyshko
// Purpose : Manages player values and mechanics like shooting

namespace MonoZombie {
	public class Player : Entity {

		public Player (Texture2D texture, Vector2 position, int health, float playerSpeed, float attacksPerSecond)
			: base(texture, position, health, attacksPerSecond, moveSpeed: playerSpeed, canRotate: true) {
		}

		/*
		 * Author : Frank Alfano, Jack Shyshko
		 * 
		 * * Overridden from the base GameObject class
		 */
		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			base.Update(gameTime, mouse, keyboard);

			// Move the player
			Move(keyboard);

			// Rotate the player to look at the mouse
			RotateTo(mouse.Position.ToVector2( ));

			// If the entity can attack and they are pressing the left mouse button, shoot a bullet
			if (CanAttack) {
				if (mouse.LeftButton == ButtonState.Pressed) {
					ShootBullet(Main.PLAYER_BULLET_DAMAGE);
				}
			}
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

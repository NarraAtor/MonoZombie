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
		private int health;
		private double attackSpdTimer;
		private double attacksPerSecond;

		public int Health {
			get {
				return health;
			}
			set {
				health = value;
			}
		}

		public Player (Texture2D texture, Vector2 position, int health, double attacksPerSecond, int playerSpeed) : base(texture, position, moveSpeed: playerSpeed, canRotate: true) {
			this.health = health;
			this.attacksPerSecond = attacksPerSecond;
		}

		/*
		 * Author : Frank Alfano, Jack Shyshko
		 * 
		 * Overridden from the base GameObject class
		 */
		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			// Move the player based on keyboard input
			Move(keyboard);

			// Rotate the player to look at the mouse
			RotateTo(mouse.Position.ToVector2( ));

            attackSpdTimer += gameTime.ElapsedGameTime.TotalSeconds;

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
			MoveBy(normMovement * MoveSpeed);
		}


        /// <summary>
        /// This is going to generate a bullet 
        /// in the direction that the player is facing 
        /// Use the player's angle to transform into
        /// bullet speed in different axis
        /// </summary>
        /// <param name="bulletTexture"> Bullet texture parameter </param>
        /// <returns> </returns>
        public void Shoot(Texture2D bulletTexture, MouseState mouse, GameTime gameTime)
        {
            /*
             * Possible implementations:
             *  → Have a firerate timer
             *  → Do it by click
             * Timer implementation - in Main() or here?
             * Probably here
             * 
             * Check mouse state in Main()
             */

            if(attackSpdTimer >= 1/attacksPerSecond)
            {
                Main.ListOfBullets.Add( new Bullet(bulletTexture, new Vector2(X, Y), Angle, 15));
                attackSpdTimer = 0;
            }

        }

		public void TakeDamage (int damage) { health -= damage; }

		public bool IsDead ( ) {
			if (health <= 0)
				return true;
			return false;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie
{
    public class Player : GameObject
    {
        private int playerSpeed;              // defines how much player moves in one direction
        private int health;
        private int attackSpd;

        public int Health { get { return health; } set { health = value; } }

        public int PlayerSpeed { get { return playerSpeed; } set { playerSpeed = value; } }
        public Player(int health, int attackSpd, Texture2D texture, int x, int y, int playerSpeed) 
            : base (texture, x, y)
        { 
            this.health = health;
            this.attackSpd = attackSpd;
            this.playerSpeed = playerSpeed;
        }

        public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
            // Move the player based on keyboard input
            Move(keyboard);

            // Rotate the player to look at the mouse
            RotateTo(mouse.Position);            
		}


        /// <summary>
        /// Moves the player depending on keys pressed;
        /// Note that playerSpeed squared is the speed
        /// of the player if they were to move in one direction,
        /// and playerSpeed will acount for movement in both directions
        /// </summary>
        /// <param name="key"></param>
        public void Move(KeyboardState key)
        {
            // Get which direction the player is trying to move
            int moveX = (key.IsKeyDown(Keys.D) ? 1 : 0) + (key.IsKeyDown(Keys.A) ? -1 : 0);
            int moveY = (key.IsKeyDown(Keys.W) ? -1 : 0) + (key.IsKeyDown(Keys.S) ? 1 : 0);

            // Get the movement vector of the player and make sure it is normalized
            // Normalizing the vector makes sure that when the player is moving diagonally they are moving the same
            // speed as if the player was just moving in 1 direction
            Vector2 normMovement = new Vector2(moveX, moveY);
            if (normMovement != Vector2.Zero) {
                normMovement.Normalize( );
            }

            // Move the position of the player
            X += (int) (normMovement.X * playerSpeed);
            Y += (int) (normMovement.Y * playerSpeed);
            RectangleCollider = new Rectangle(RectangleCollider.X, RectangleCollider.Y, RectangleCollider.Width, RectangleCollider.Height);
        }


        /// <summary>
        /// This is going to generate a bullet 
        /// in the direction that the player is facing 
        /// Use the player's angle to transform into
        /// bullet speed in different axis
        /// </summary>
        /// <param name="bulletTexture"> Bullet texture parameter </param>
        /// <returns> </returns>
        public Bullet Shoot(Texture2D bulletTexture)
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


            double speedX = Math.Cos(angle);
            double speedY = Math.Sin(angle);

            return new Bullet(bulletTexture, X, Y, speedX, speedY, 15);
        }

        public void TakeDamage(int damage) { health -= damage; }

        public bool IsDead()
        {
            if (health <= 0)
                return true;
            return false;
        }
    }
}

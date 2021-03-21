using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie
{
    class Player : GameObject
    {
        private int playerSpeed;              // defines how much player moves in one direction
        private int health;
        private int attackSpd;

        public int Health { get { return health; } }


        public Player(int health, int attackSpd, Texture2D texture, int x, int y, int playerSpeed) 
            : base (texture, x, y)
        { 
            this.health = health;
            this.attackSpd = attackSpd;
            this.playerSpeed = playerSpeed;
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
            /* Check for collision in Main()
             * Cycle through objects, can't here */

            if (key.IsKeyDown(Keys.D))
            {
                if (key.IsKeyDown(Keys.W))
                {
                    X += playerSpeed;
                    Y -= playerSpeed;
                }
                else if (key.IsKeyDown(Keys.S))
                {
                    X += playerSpeed;
                    Y += playerSpeed;
                }
                else
                    X += playerSpeed * playerSpeed;
            }
            if(key.IsKeyDown(Keys.A))
            {
                if (key.IsKeyDown(Keys.W))
                {
                    X -= playerSpeed;
                    Y -= playerSpeed;
                }
                else if (key.IsKeyDown(Keys.S))
                {
                    X -= playerSpeed;
                    Y += playerSpeed;
                }
                else
                {
                    X -= playerSpeed * playerSpeed;
                }
            }
            if (key.IsKeyDown(Keys.S))
                Y -= playerSpeed * playerSpeed;
            if (key.IsKeyDown(Keys.W))
                Y += playerSpeed * playerSpeed;
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

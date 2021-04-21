using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie
{

    /*
     * An instance of Bullet class does not detect collision 
     * (beyond as implemented in the parent class)
     * and does not delete itself - do it on collision 
     * in Main(). 
     */

    public class Bullet : GameObject {
        private Vector2 movement;

        private int bulletDamage;

        /// <summary>
        /// Instantiates a Bullet object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speedX"> How much the bullet will be moving in the horizontal direction </param>
        /// <param name="speedY"> How much the bullet will be moving in the vertical direction </param>
        /// <param name="angle"> The angle the player was facing when the bullet was shot </param>
        /// <param name="bulletSpeed"> How fast the bullet is going to be moving</param>
        public Bullet (Texture2D texture, Vector2 position, GameObject parent, float angle, int bulletSpeed = Main.BULLET_SPEED, int bulletDamage = Main.PLAYER_BULLET_DAMAGE) : base(texture, position, parent: parent, moveSpeed: bulletSpeed, canRotate: true) {
            this.bulletDamage = bulletDamage;

            movement = new Vector2(MathF.Sin(angle), -MathF.Cos(angle));
            movement.Normalize( );
            movement *= moveSpeed;
        }

        /*
         * * Overridden from the GameObject Class
         */
        public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
            // Rotate the bullets to face towards the 
            RotateTo(Position + movement);

            // Move the bullet in the direction it is travelling
            MoveBy(movement);
        }

        /*
         * Author : Frank Alfano, Ken Adachi-Bartholomay
         * 
         * * Overridden from the GameObject Class
         * 
         * return bool                  : If the bullet has hit a zombie
         */
        public new bool CheckCollision (GameObject other) {
            bool didCollide = base.CheckCollision(other);

            // If the bullet has collided with something, then destroy it
            if (didCollide) {
                // If the bullet collides with an enemy, we want to destroy the bullet and decrease the zombie health
                if (typeof(Enemy).IsInstanceOfType(other)) {
                    // 10 can be changed later, its just the number I found in the code in the Main class
                    ((Enemy) other).TakeDamage(bulletDamage);

                    Destroy( );
                } else if (typeof(Tile).IsInstanceOfType(other)) {
                    // If the bullet collides with a wall, we want to destroy the bullet without doing anything else
                    if (!((Tile) other).IsWalkable) {
                        Destroy( );
                    }
                }
            }

            return didCollide;
        }
    }
}

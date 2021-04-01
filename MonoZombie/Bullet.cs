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

    public class Bullet : GameObject
    {
        
        int bulletSpeed;                    // the default bullet speed, as if it were moving in one direction
        double speedX;                         // how much the bullet moves in x axis;
        double speedY;                         // how much the bullet moves in y axis;

        int damage;
        // Making a damage function would prove impossible, at least at the moment, as Enemy has nothing within it


        /// <summary>
        /// Instantiates a Bullet object
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="speedX"> How much the bullet will be moving in the horizontal direction </param>
        /// <param name="speedY"> How much the bullet will be moving in the vertical direction </param>
        /// <param name="bulletSpeed"> How fast the bullet is going to be moving</param>
        public Bullet(Texture2D texture, Vector2 position, double speedX, double speedY, int bulletSpeed) 
            : base(texture, position)
        {
            this.bulletSpeed = bulletSpeed;
            this.speedX = bulletSpeed * speedX;
            this.speedY = bulletSpeed * speedY;
        }


        /// <summary>
        /// Moves the bullet;
        /// Call the Move method from Main()
        /// this is for convenience purposes, so that we could remove it from the list from there
        /// </summary>
        public void Move()
        {
            X += (int)speedX;
            Y += (int)speedY;
        }

        /// Don't check for collision here
        /// Because we will need to remove
        /// Bullets from list in Game1
    }
}

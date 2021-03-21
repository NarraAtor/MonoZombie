using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie
{
    class Enemy : GameObject
    {
        int health;
        int speed;
        int attSpeed;
        double attTime;

        public Enemy(Texture2D texture, int x, int y, int health, int speed, int attSpeed) 
            : base(texture, x, y)
        {
            this.health = health;
            this.speed = speed;
            this.attSpeed = attSpeed;
        }

        public void Attack(GameObject objToAtt)
        {
            if (Distance(new Point(X, Y), new Point(objToAtt.X, objToAtt.Y)) < radius)
            {

            }
        }


        /// <summary>
        /// Update, make sure the time works 
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {

        }
    }
}

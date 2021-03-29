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
        double timeAtLastFrame;
        double attTime;
        double timer;

        public bool IsAlive { get; set; }

        public Enemy(Texture2D texture, int x, int y, int health, int speed, int attSpeed) 
            : base(texture, x, y)
        {
            this.health = health;
            this.speed = speed;
            this.attSpeed = attSpeed;
            timer = 0;
        }

        public void Attack(Player player)
        {
            if (Distance(new Point(X, Y), new Point(player.X, player.Y)) < radius)
            {
                player.TakeDamage(10);
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public bool IsAlive { get { return IsAlive; } }

        public void Die() { isAlive = false; }


        /// <summary>
        /// Update, make sure the time works 
        /// </summary>
        /// <param name="time"></param>
/*        public void Update(GameTime time)
        {
            double timeBetweenFrames = Double.TryParse(time.ToString()); //- timeAtLastFrame;
            timer += (double)time.ElapsedGameTime;

            Double.TryParse(time.ToString(), timeAtLastFrame);
        }*/
    }
}

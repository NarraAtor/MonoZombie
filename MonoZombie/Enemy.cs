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

        bool isAlive;

        public Enemy(Texture2D texture, int x, int y, int health, int speed, int attSpeed) 
            : base(texture, x, y)
        {
            this.health = health;
            this.speed = speed;
            this.attSpeed = attSpeed;
            timer = 0;
        }


        /// <summary>
        /// Attacks the player every few seconds
        /// Uses timer to calculate whether it can attack or not
        /// Variable attack speed to be implemented
        /// </summary>
        /// <param name="player"></param>
        public void Attack(Player player)
        {
            if (Distance(new Point(X, Y), new Point(player.X, player.Y)) < radius)
            {
                if (timer > 1) 
                {
                    player.TakeDamage(10);
                    timer = 0;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public bool IsAlive { get { return isAlive; } }
        public int Health { get { return health; } set { health = value; } }

        public void Die() { isAlive = false; }


        /// <summary>
        /// Update, make sure the time works 
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            double currentTime;
            Double.TryParse(time.ToString(), out currentTime); //- timeAtLastFrame;
            double timeBetweenFrames = currentTime - timeAtLastFrame;
            timer += timeBetweenFrames;

            timeAtLastFrame = currentTime;
        }
    }
}

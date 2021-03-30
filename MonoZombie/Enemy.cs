﻿using System;
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
        //testing property
        public double Timer { get { return timer; } }

        bool isAlive;

        public Enemy(Texture2D texture, int x, int y, int health, int speed, int attSpeed) 
            : base(texture, x, y)
        {
            this.health = health;
            this.speed = speed;
            this.attSpeed = attSpeed;
            isAlive = true;
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
            //Eric: GameObject never assigns to radius so I simply replaced the if statment with a rectangle intersects bool
            //if (Distance(new Point(X, Y), new Point(player.X, player.Y)) < radius)
            if(RectangleCollider.Intersects(player.RectangleCollider))
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
        public void Update(GameTime time, Player player)
        {
            //double currentTime;
            //Double.TryParse(time.ToString(), out currentTime); //- timeAtLastFrame;
            timer += time.ElapsedGameTime.TotalSeconds;
            //double timeBetweenFrames = currentTime - timeAtLastFrame;
            //timer += timeBetweenFrames;
            Attack(player);
            //timeAtLastFrame = currentTime;
        }
    }
}

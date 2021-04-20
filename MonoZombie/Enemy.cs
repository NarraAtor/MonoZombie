﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie
{
    public class Enemy : GameObject
    {
        private int health;
        private int speed;
        private int attSpeed;
        private double timeAtLastFrame;
        private double attTime;
        private double timer;
        private bool isAlive;

        //testing property
        public double Timer { get { return timer; } }

        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
        public int Health { get { return health; } set { health = value; } }

        public Enemy(Texture2D texture, Vector2 position, int health, int speed, int attSpeed)
            : base(texture, position, canRotate: true)
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
            if (Rect.Intersects(player.Rect))
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

        /// <summary>
        /// Update, make sure the time works 
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time, Player player)
        {
            if (isAlive)
            {
                canMove = true;
                timer += time.ElapsedGameTime.TotalSeconds;
                Attack(player);
            }
            else
            {
                canMove = false;
            }
        }

        /// <summary>
        /// Purpose: Only draw the zombie if it is alive.
        /// </summary>
        /// <param name="spriteBatch">the zombie to draw to</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive)
                base.Draw(spriteBatch);
        }
    }
}

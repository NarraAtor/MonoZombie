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
        int playerSpeed;                                                // defines how much player moves in one direction
        public Player(int health, int attackSpd, Texture2D texture) 
            : base (health, attackSpd, texture)
            { }

        public override Move(KeyboardState key)
        {
            if (key.IsKeyDown(Keys.D))
            {
                if (key.IsKeyDown(Keys.W))
                    base(playerSpeed, playerSpeed);
                if (key.IsKeyDown(Keys.S))
                    base(playerSpeed, -playerSpeed);
                else
                    base(playerSpeed * playerSpeed, 0);
            }
            if(key.IsKeyDown(Keys.A))
            {
                if (key.IsKeyDown(Keys.W))
                    base(-playerSpeed, playerSpeed);
                if (key.IsKeyDown(Keys.S))
                    base(-playerSpeed, -playerSpeed);
                else
                    base(-playerSpeed * playerSpeed, 0);
            }
            if (key.IsKeyDown(Keys.S))
                base(0, -playerSpeed * playerSpeed);
            if (key.IsKeyDown(Keys.W))
                base(0, playerSpeed * playerSpeed);
        }

/*        public void Shoot(MouseState ms, MouseState prevMS)
        {
            if (!ms.LeftButton.Equals(prevMS.LeftButton))
                new Bullet()
        }*/
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie
{
    
    class WallTile
    {
        private Random rng=new Random();
        private TileType type;
        private Rectangle location;
        private Texture2D Image;

        public TileType Type
        {
            get { return type; }
        }

        public WallTile(TileType other, int x, int y,int width,int height)
        {
            type = other;
            location.X = x;
            location.Y = y;
            location.Width = width;
            location.Height = height;
            switch (other)
            {

                case TileType.Wall:
                    {
                        //Image = Game1.WallProperty1;
                        break;
                    }
                case TileType.Grass:
                    {
                        
                          //  Image = Game1.GrassProperty1;
                            break;                      

                    }
            }
        }

        /// <summary>
        /// checks for collision and then moves the player acordingly
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Collision(Player other)
        {
            float dx = Math.Abs( other.X- location.X);
            float dy = Math.Abs(other.Y - location.Y);
            switch (type)
            {

                case TileType.Wall:
                    {
                        if (dx<= (location.Width/2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        if(dy <= (location.Height / 2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        return false;
                    }
                case TileType.Gravel:
                    {
                        if (dx <= (location.Width / 2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        if (dy <= (location.Height / 2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        return false;
                    }

                case TileType.Lava:
                {
                        if (dx <= (location.Width / 2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        if (dy <= (location.Height / 2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        return false;
                    }

                case TileType.Speed:
                    {
                        if (dx <= (location.Width / 2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        if (dy <= (location.Height / 2))
                        {
                            System.Diagnostics.Debug.WriteLine("DONE");
                            return true;
                        }
                        return false;
                    }

                case TileType.Grass:
                    {
                        return false;
                    }
            }
            return false;
        }

        public void Draw(SpriteBatch sb, Color tint)
        {
            sb.Draw(Image,location , tint);
        }

        public void Update()
        {
            this.Collision(Game1.Player);
        }
    }
}

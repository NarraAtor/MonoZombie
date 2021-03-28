using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie
{
    public enum Tile
    {
        Grass,
        Wall,
        Gravel,
        Lava,
        Speed,
        ZombieSpawn
    }
    class WallTile
    {
        private Random rng=new Random();
        private Tile type;
        private Rectangle location;
        private Texture2D Image;

        public Tile Type
        {
            get { return type; }
        }

        public WallTile(Tile other, int x, int y,int width,int height)
        {
            type = other;
            location.X = x;
            location.Y = y;
            location.Width = width;
            location.Height = height;
            switch (other)
            {

                case Tile.Wall:
                    {
                        Image = Game1.TESTWallProperty;
                        break;
                    }
                case Tile.Grass:
                    {
                        if(rng.Next(4)==1)
                        {
                            Image = Game1.TESTGrassProperty;
                            break;
                        }
                        else if(rng.Next(4) == 2)
                        {
                           // Image = Game1.TESTGrassProperty2;
                            break;
                        }
                        else
                        {
                           // Image = Game1.TESTGrassProperty3;
                            break;
                        }

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
            float dx = location.X - other.X;
            float dy = location.Y - other.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            switch (type)
            {

                case Tile.Wall:
                    {
                        if (dist < other.Radius + location.Width)
                        {

                            return true;
                        }
                        return false;
                    }
                case Tile.Gravel:
                    {
                        if (dist < other.Radius + location.Width)
                        {
                            other.PlayerSpeed = other.PlayerSpeed - 5;
                        }
                        return true;
                    }

                case Tile.Lava:
                {
                        if (dist < other.Radius + location.Width)
                        {
                            other.Health = other.Health - 5;
                        }
                    return true;
                }

                case Tile.Speed:
                    {
                        if (dist < other.Radius + location.Width)
                        {
                            other.PlayerSpeed = other.PlayerSpeed + 5;
                        }
                        return true;
                    }

                case Tile.Grass:
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
    }
}

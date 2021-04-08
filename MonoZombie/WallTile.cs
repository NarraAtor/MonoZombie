using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
//Matthew Sorrentino
//Creates the walls and floors of the game map
/*namespace MonoZombie
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
                        Image = Game1.WallProperty1;
                        break;
                    }
                case Tile.Grass:
                    {
                        
                            Image = Game1.GrassProperty1;
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

            switch (type)
            {
                
                case Tile.Wall:
                    {
                      //checks for collision between player and walls
                      if(location.Intersects(other.RectangleCollider))
                        {
                            //checks if collsion is on the left and right
                            if (Rectangle.Intersect(other.RectangleCollider, location).Width <= Rectangle.Intersect(other.RectangleCollider, location).Height)
                            {
                                //left case
                                if (other.RectangleCollider.X >= location.X)
                                {
                                    other.X += Rectangle.Intersect(other.RectangleCollider, location).Width;
                                    
                                }
                                //right case
                                else
                                {
                                    other.X -= Rectangle.Intersect(other.RectangleCollider, location).Width;
                                    
                                }
                            }
                            //vertical collsion
                            else
                            {
                                //top case
                                if (other.Y >= location.Y)
                                {
                                    other.Y += Rectangle.Intersect(other.RectangleCollider, location).Height;
                                }
                                //bottom case
                                else if (other.Y <= location.Y)
                                {
                                    other.Y -= Rectangle.Intersect(other.RectangleCollider, location).Height;
                                }
                            }
                            return true;
                        }
                        return false;
                    }
                case Tile.Gravel:
                    {
                        
                        return false;
                    }

                case Tile.Lava:
                {
                        return false;
                    }

                case Tile.Speed:
                    {
                        return false;
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

        public void Update()
        {
            this.Collision(Game1.Player);
        }
    }
}*/

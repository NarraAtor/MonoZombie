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
        private Tile type;
        private Rectangle location;
        private Texture2D Image;
        private Texture2D RandImage;//To Give varaity and randomness to the stage

        public Tile Type
        {
            get { return type; }
        }

        public WallTile(Tile other, int x, int y)
        {
            type = other;
            location.X = x;
            location.Y = y;
            switch (other)
            {

                case Tile.Wall:
                    {
                        //Image = Game1.TESTGrassProperty;
                        break;
                    }
                case Tile.Grass:
                    {
                        Image = Game1.TESTGrassProperty;
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
            float dx = location.X - other.X;
            float dy = location.Y - other.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            switch (type)
            {
                case 0:
                    return false;


                case Tile.Wall:
                    if(dist>other.Radius+location.Width)
                    {
                        return true;
                    }
                    return false;
            }
            return false;
        }

        public void Draw(SpriteBatch sb, Color tint)
        {
            sb.Draw(Image,location , tint);
        }
    }
}

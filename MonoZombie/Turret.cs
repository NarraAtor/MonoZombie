using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie
{
    public class Turret
    {
        private int range;
        private int timer;//use later
        private int damage;
        private int price;
        private Texture2D turret;//the image of the turret
        private Rectangle Holder;//the location and size of the turret
        public enum TurretType
        {
            Cannon,
            Archer,
            Magic,
            Trap,
            Buff,//these work diffrently than the rest 
            DeBuff//these work diffrently then the rest 
        }

        public int X
        {
            get { return Holder.X; }
            set { Holder.X = value; }
        }

        public int Y
        {
            get { return Holder.Y; }
            set { Holder.Y = value; }
        }

        public int Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public int Price
        {
            get { return price; }
        }
        public Turret(TurretType type,Texture2D Image)
        {
            //goes through each of the diffrent turret types and then sets stats accordingly 

            Holder.X=X;
            Holder.Y=Y;
            turret = Image;
            switch (type)
            {
                
                case TurretType.Cannon:
                    {
                        range = 50;
                        damage = 100;
                        price = 200;
                        break;
                    }
                case TurretType.Archer:
                    {
                        range = 50;
                        damage = 100;
                        price = 300;
                        break;
                    }

                case TurretType.Buff:
                    {
                        range = 50;
                        damage = 100;
                        price = 400;
                        break;
                    }

                case TurretType.DeBuff:
                    {
                        range = 50;
                        damage = 100;
                        price = 500;
                        break;
                    }

                case TurretType.Magic:
                    {
                        range = 50;
                        damage = 100;
                        price = 500;
                        break;
                    }

                case TurretType.Trap:
                    {
                        range = 50;
                        damage = 100;
                        price = 500;
                        break;
                    }

            }
        }

        /*
        public Attack(Zombie target)
        {
            float dx = Holder.X - target.X;
            float dy = Holder.Y - target.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy); 
            if(dist<=this.range)
              {
                 Zombie.Health-=damage; 
              }
           else
            {
              break;
            }
        }
        */

        public void Draw(SpriteBatch sb, Color tint)
        {
            sb.Draw(turret, Holder, tint);
        }
    }
}

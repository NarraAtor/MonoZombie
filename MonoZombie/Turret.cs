using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie
{
    public enum TurretType
    {
        Cannon,
        Archer,
        Magic,
        Trap,
        Buff,//these work diffrently than the rest 
        DeBuff//these work diffrently then the rest 
    }

     class Turret :GameObject
    {
        private int range;
        private int timer;//use later
        private int damage;
        private int price;
        private Texture2D turret;//the base image of the turret
        private Texture2D GunPart;//The  rotating head of the turret
        private Rectangle Holder;//the location and size of the turret

        //Eric
        private Rectangle detector;//the detection range of the turret
        
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
        public Turret(TurretType type,Texture2D Base, Texture2D Head,int X,int Y) :base(Base,X,Y)
        {
            //goes through each of the diffrent turret types and then sets stats accordingly 

            Holder.X=X;
            Holder.Y=Y;
            Holder.Width = 50;
            Holder.Height = 50;
            turret = Base;
            GunPart = Head;
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
                        //TODO: Adjust the rectangle so it is centered
                        detector = new Rectangle(Holder.X, Holder.Y, range, range);
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

        /// <summary>
        /// Edited by Eric
        /// Purpose: Causes the turret to check if the target zombie is in this turret's range by using rectangle colliders.
        /// Restrictions: 
        /// </summary>
        /// <param name="target">the enemy to be checked</param>
        public void Detect(Enemy target)
        {
           if(detector.Intersects(target.RectangleCollider))
             {
                Point center = new Point(target.X,target.Y);
                RotateTo(center);
                target.Health-=damage; 
             }
           else
           {
             return;
           }

        }

        public void Draw(SpriteBatch sb, Color tint)
        {
            sb.Draw(turret, Holder, tint);
            sb.Draw(GunPart, Holder, tint);
        }

        public override void Update(GameTime gameTime, MouseState mouse, KeyboardState keyboard)
        {
           // Detect();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//Matthew Sorrentino / Eric Fotang
//creates a turret object w a draw and update method
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

    /// <summary>
    /// Authors:Eric, Matthew
    /// Purpose: Manages turrets and their function.
    /// Restrictions:
    /// </summary>
    class Turret : GameObject
    {
        private int range;
        private int timer;//use later
        private int damage;
        private int price;
        private Texture2D turret;//the base image of the turret
        private Texture2D GunPart;//The  rotating head of the turret
        private Rectangle Holder;//the location and size of the turret

        private Enemy target; // the target to shoot at

        //public int TurretX
        //{
        //    get { return Holder.X; }
        //    set { Holder.X = value; }
        //}
        //
        //public int TurretY
        //{
        //    get { return Holder.Y; }
        //    set { Holder.Y = value; }
        //}

        public int Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public int Price
        {
            get { return price; }
        }
        public Turret(TurretType type, Texture2D Base, Texture2D Head, Vector2 position) : base(Base, position, canRotate: true)
        {
            //goes through each of the diffrent turret types and then sets stats accordingly 

            Holder.X = X;
            Holder.Y = Y;
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


        public void DetectPlus(List<Enemy> enemies)
        {
            float closestRange = float.MaxValue;
            float distancetoZombie;
            foreach (Enemy zombie in enemies)
            {
                distancetoZombie = Distance(new Vector2(zombie.X, zombie.Y), new Vector2(X, Y));
                if (distancetoZombie <= range && distancetoZombie < closestRange)
                {
                    closestRange = distancetoZombie;
                    target = zombie;
                }
            }

            if(!(target is null))
            target.Health -= damage;
        }

        public void Draw(SpriteBatch sb, Color tint)
        {
            sb.Draw(turret, Holder, tint);
            sb.Draw(GunPart, Holder, tint);
        }

        public void Update(Enemy target)
        {
            DetectPlus(Game1.ListOfZombies);
        }
    }
}


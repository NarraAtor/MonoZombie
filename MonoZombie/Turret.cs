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
        private int damage;
        private int price;
        private double attackSpdTimer;
        private double attacksPerSecond;
        private Texture2D turret;//the base image of the turret
        private Texture2D GunPart;//The  rotating head of the turret
        private Rectangle Holder;//the location and size of the turret
        private Enemy target; // the target to shoot at

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
                        attacksPerSecond = 5;
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

        /// <summary>
        /// Purpose: Detects the closest zombie in this turret's range and fires a bullet at it.
        /// Restrictions:
        /// </summary>
        /// <param name="enemies">the list of enemies to attack.</param>
        /// <param name="bulletTexture">the texture of the bullets</param>
        /// <param name="gameTime">the information on time in game.</param>
        public void Detect(List<Enemy> enemies, Texture2D bulletTexture, GameTime gameTime)
        {
            if (attackSpdTimer >= 1 / attacksPerSecond)
            {
                float closestRange = float.MaxValue;
                float distancetoZombie;
                foreach (Enemy zombie in enemies)
                {
                    distancetoZombie = Main.Distance(new Vector2(zombie.X, zombie.Y), new Vector2(X, Y));
                    if (distancetoZombie <= range && distancetoZombie < closestRange)
                    {
                        closestRange = distancetoZombie;
                        target = zombie;
                    }
                }

                if (!(target is null))
                {
                    RotateTo(new Vector2(target.X, target.Y));
                    Main.ListOfBullets.Add(new Bullet(bulletTexture, new Vector2(X, Y), Angle, 15));
                    attackSpdTimer = 0;
                }
                //target.Health -= damage;
            }

        }

        public void Draw(SpriteBatch sb, Color tint)
        {
            //base.Draw(turret, Holder, tint);
            //base.Draw(GunPart, Holder, tint);
            SpriteManager.DrawImage(sb, turret, Rect, angle: 0);
            //Change the angle the gun is drawn at since the asset is drawn a bit differently 
            //(about 90 degrees off from where it's actually facing).
            SpriteManager.DrawImage(sb, GunPart, Rect, angle: Angle + (MathF.PI/2));
        }

        public void UpdateTurret(Enemy target, Texture2D bulletTexture, GameTime gameTime)
        {
            attackSpdTimer += gameTime.ElapsedGameTime.TotalSeconds;
            Detect(Main.ListOfZombies, bulletTexture, gameTime);
        }
    }
}


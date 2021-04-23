using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//Matthew Sorrentino / Eric Fotang
//creates a turret object w a draw and update method
namespace MonoZombie {
	public enum TurretType {
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
	public class Turret : GameObject {
		protected float timeSinceLastAttack;
		protected float attacksPerSecond;

		private Texture2D turretBaseTexture; // The base image of the turret
		private Texture2D turretHeadTexture; // The rotating head of the turret

		private int range;
		private int damage;
		private Enemy target; // the target to shoot at

		public int Price {
			get;
			private set;
		}

		public int RoundTimer
		{
			get;
			set;
		}

		public bool CanAttack {
			get {
				return (timeSinceLastAttack >= 1 / attacksPerSecond);
			}
		}

		public Turret (TurretType type, Texture2D turretBaseTexture, Texture2D turretHeadTexture, Vector2 position, GameObject parent = null) : base(turretHeadTexture, position, parent: parent, canRotate: true) {
			// Goes through each of the diffrent turret types and then sets stats accordingly 

			this.turretBaseTexture = turretBaseTexture;
			this.turretHeadTexture = turretHeadTexture;
			RoundTimer = 1;
			switch (type) {

				case TurretType.Cannon: {
						range = 50;
						damage = 100;
						Price = 200;
						break;
					}
				case TurretType.Archer: {
						range = 50;
						damage = 100;
						Price = 300;
						attacksPerSecond = 5;
						break;
					}

				case TurretType.Buff: {
						range = 50;
						damage = 100;
						Price = 400;
						break;
					}

				case TurretType.DeBuff: {
						range = 50;
						damage = 100;
						Price = 500;
						break;
					}

				case TurretType.Magic: {
						range = 50;
						damage = 100;
						Price = 500;
						break;
					}

				case TurretType.Trap: {
						range = 50;
						damage = 100;
						Price = 500;
						break;
					}


			}

			// Turret test values
			range = 400;
			attacksPerSecond = 1;
			damage = 40;
		}

		/// <summary>
		/// Purpose: Detects the closest zombie in this turret's range.
		/// Restrictions:
		/// </summary>
		/// <param name="enemies">the list of enemies to attack.</param>
		/// <param name="bulletTexture">the texture of the bullets</param>
		/// <param name="gameTime">the information on time in game.</param>
		public void DetectTarget ( ) {
			// Reset the target just in case the current target has moved out of its range
			target = null;

			// The closest zombie in range of the turret
			float closestRange = range;

			// Loop through each of the enemies currently on the map to find the closest one
			foreach (Enemy zombie in Main.ListOfZombies) {
				// Get the distance from this turret to the current zombie
				float distancetoZombie = Vector2.Distance(zombie.Position, Position);

				// Check to see if the current zombie is the closest one discovered
				if (distancetoZombie < closestRange) {
					closestRange = distancetoZombie;
					target = zombie;
				}
			}
		}

		/*
		 * Author : Frank Alfano, Eric Fotang
		 * 
		 * * Overriden from GameObject class
		 */
		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			// Update the last time since this game object has attacked
			timeSinceLastAttack += (float) gameTime.ElapsedGameTime.TotalSeconds;

			// Detect nearby targets
			DetectTarget( );

			// If the current target is not equal to null, then rotate the turret to look towards it
			if (target != null) {
				RotateTo(target.Position);

				// If the turret can shoot a bullet, shoot a bullet
				if (timeSinceLastAttack >= 1 / attacksPerSecond) {
					Main.ListOfBullets.Add(new Bullet(Main.bulletTexture, centerPosition, this, Angle, bulletDamage: Main.CANNON_BULLET_DAMAGE));

					timeSinceLastAttack = 0;
				}
			}

			base.Update(gameTime, mouse, keyboard);
		}

		public new void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			SpriteManager.DrawImage(spriteBatch, turretBaseTexture, Rect, Color.White);

			//Change the angle the gun is drawn at since the asset is drawn a bit differently 
			//(about 90 degrees off from where it's actually facing).
			SpriteManager.DrawImage(spriteBatch, turretHeadTexture, Rect, Color.White, angle: Angle);
		}
	}
}


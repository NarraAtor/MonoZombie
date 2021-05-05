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
	public class Turret : Entity {
		private Texture2D turretBaseTexture; // The base image of the turret
		private Texture2D turretHeadTexture; // The rotating head of the turret

		private TurretType type;
		private int turretRange;
		private int turretDamage;
		private Zombie target; // the target to shoot at

		public int Price {
			get;
			private set;
		}

		public int Range { get { return range; } }

		public int RoundTimer
		{
			get;
			set;
		}

		public Turret (TurretType turretType, Texture2D turretBaseTexture, Texture2D turretHeadTexture, Vector2 centerPosition, GameObject parent = null)
			: base(turretHeadTexture, centerPosition, parent: parent, canRotate: true) {
			// Goes through each of the diffrent turret types and then sets stats accordingly 
			this.turretBaseTexture = turretBaseTexture;
			this.turretHeadTexture = turretHeadTexture;
			RoundTimer = 1;

			this.type = type;
			switch (type) {

				case TurretType.Cannon: {
						range = 100;
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
						range = 100;
						damage = 100;
						Price = 400;
						break;
					}

					break;
				case TurretType.Magic:
					turretRange = 50;
					turretDamage = 100;
					Price = 500;

					break;
				case TurretType.Trap:
					turretRange = 50;
					turretDamage = 100;
					Price = 500;

					break;
			}

			// Turret test values
			AttacksPerSecond = 1;
			turretRange = 400;
			turretDamage = 40;
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
			float closestRange = turretRange;

			// Loop through each of the enemies currently on the map to find the closest one
			for (int i = Main.ListOfZombies.Count - 1; i >= 0; i--) {
				// Get the distance from this turret to the current zombie
				float distancetoZombie = Vector2.Distance(Main.ListOfZombies[i].Position, Position);

				// Check to see if the current zombie is the closest one discovered
				if (distancetoZombie < closestRange) {
					closestRange = distancetoZombie;
					target = Main.ListOfZombies[i];
				}
			}
		}

		/*
		 * Author : Frank Alfano, Eric Fotang
		 * 
		 * * Overriden from GameObject class
		 */
		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			base.Update(gameTime, mouse, keyboard);

			// Detect nearby targets
			DetectTarget();

			if (type == TurretType.Archer)
			{
				// If the current target is not equal to null, then rotate the turret to look towards it
				if (target != null)
				{
					RotateTo(target.Position);

					// If the turret can shoot a bullet, shoot a bullet
					if (timeSinceLastAttack >= 1 / attacksPerSecond)
					{
						Main.ListOfBullets.Add(new Bullet(Main.bulletTexture, centerPosition, this, Angle, bulletDamage: Main.ARCHER_BULLET_DAMAGE));

						timeSinceLastAttack = 0;
					}
				}
			}
			else if (type == TurretType.Cannon)
			{
				if (target != null)
				{
					RotateTo(target.Position);

				if (CanAttack) {
					ShootBullet(turretDamage);
				}
			}
		}


		public new void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			if (IsOnScreen) {
				SpriteManager.DrawImage(spriteBatch, turretBaseTexture, Rect, Color.White);
				SpriteManager.DrawImage(spriteBatch, turretHeadTexture, Rect, Color.White, angle: Angle);
			}
		}
	}
}


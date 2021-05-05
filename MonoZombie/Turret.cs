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

		private Zombie target; // the target to shoot at

		private TurretType turretType;

		public int Range {
			get;
			private set;
		}

		public int Damage {
			get;
			private set;
		}

		public Turret (TurretType turretType, Vector2 centerPosition, GameObject parent = null)
			: base(Main.turretBaseTexture, centerPosition, health: 3, parent: parent, canRotate: true) {
			// Goes through each of the diffrent turret types and then sets stats accordingly
			this.turretType = turretType;

			turretBaseTexture = Main.turretBaseTexture;

			switch (this.turretType) {
				case TurretType.Cannon:
					Range = 250;
					Damage = Main.CANNON_BULLET_DAMAGE;
					AttacksPerSecond = 1;

					turretHeadTexture = Main.turretCannonHeadTexture;

					break;
				case TurretType.Archer:
					Range = 350;
					Damage = Main.ARCHER_BULLET_DAMAGE;
					AttacksPerSecond = 2;

					turretHeadTexture = Main.turretArcherHeadTexture;

					break;

				case TurretType.Buff:
					Range = 150;
					Damage = 0;

					turretHeadTexture = Main.turretBuffHeadTexture;

					break;
			}
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
			float closestRange = Range;

			// Loop through each of the enemies currently on the map to find the closest one
			for (int i = Main.Zombies.Count - 1; i >= 0; i--) {
				// Get the distance from this turret to the current zombie
				float distancetoZombie = Vector2.Distance(Main.Zombies[i].Position, Position);

				// Check to see if the current zombie is the closest one discovered
				if (distancetoZombie < closestRange) {
					closestRange = distancetoZombie;
					target = Main.Zombies[i];
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

			if (turretType == TurretType.Buff) {
				for (int i = Main.Turrets.Count - 1; i >= 0; i--) {
					Main.Turrets[i].IsBuffed = (Vector2.Distance(Main.Turrets[i].Position, Position) <= Range);
				}

				Main.Player.IsBuffed = (Vector2.Distance(Main.Player.Position, Position) <= Range);
			} else {
				// Detect nearby targets
				DetectTarget( );

				if (target != null) {
					RotateTo(target.Position);

					if (CanAttack) {
						ShootBullet(Damage);
					}
				}
			}
		}

		public new void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
			if (IsOnScreen) {
				SpriteUtils.DrawImage(spriteBatch, turretBaseTexture, Rect, ((WasDamaged) ? Color.Red : BaseTint));
				SpriteUtils.DrawImage(spriteBatch, turretHeadTexture, Rect, ((WasDamaged) ? Color.Red : BaseTint), angle: Angle);

				DrawHealthBar(gameTime, spriteBatch, graphics);
			}
		}
	}
}


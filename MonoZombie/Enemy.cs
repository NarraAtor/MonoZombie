using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie {
	public class Enemy : GameObject {
		protected float timeSinceLastDamage;
		protected float timeSinceLastAttack;
		protected float attacksPerSecond;

		public int Health {
			get;
			private set;
		}

		public bool IsDead {
			get {
				return (Health <= 0);
			}
		}

		public bool CanAttack {
			get {
				return (timeSinceLastAttack >= 1 / attacksPerSecond);
			}
		}

		public Enemy (Texture2D texture, Vector2 position, int health, int moveSpeed, float attacksPerSecond) : base(texture, position, moveSpeed: moveSpeed, canRotate: true) {
			Health = health;
			this.attacksPerSecond = attacksPerSecond;
		}

		public void TakeDamage (int damage) {
			Health -= damage;

			timeSinceLastDamage = 0;
		}

		/// <summary>
		/// Update, make sure the time works 
		/// </summary>
		/// <param name="time"></param>
		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			// Update the last time since this game object has attacked
			timeSinceLastAttack += (float) gameTime.ElapsedGameTime.TotalSeconds;
			timeSinceLastDamage += (float) gameTime.ElapsedGameTime.TotalSeconds;

			// If the zombie is dead, then destroy it and add some currency to the player
			if (IsDead) {
				// CHANGE 10 TO LIKE A RANDOM NUMBER OR SOMETHING
				Main.currency += 10;

				Destroy( );
			}
		}

		public new bool CheckUpdateCollision (GameObject other) {
			bool didCollide = base.CheckUpdateCollision(other);

			// If the zombie has collided with the player and can attack, attack the player
			if (didCollide && typeof(Player).IsInstanceOfType(other)) {
				if (CanAttack) {
					((Player) other).TakeDamage(10);

					timeSinceLastAttack = 0;
				}
			}

			return didCollide;
		}

		public new void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			Color damageTint = (timeSinceLastDamage < Main.DAMAGE_INDIC_TIME) ? Color.Red : Color.White;

			SpriteManager.DrawImage(spriteBatch, texture, Rect, damageTint, angle: Angle);
		}
	}
}

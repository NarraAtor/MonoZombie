using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public abstract class Entity : GameObject {
		private float secondsSinceLastDamage;
		private float secondsSinceLastAttack;

		public int Health {
			get;
			private set;
		}

		protected float AttacksPerSecond {
			get;
			set;
		}

		public bool CanAttack {
			get {
				return (secondsSinceLastAttack >= 1 / AttacksPerSecond);
			}
		}

		public bool WasDamaged {
			get {
				return (secondsSinceLastDamage < Main.DAMAGE_INDIC_TIME);
			}
		}

		public bool IsDead {
			get {
				return (Health <= 0);
			}
		}

		public Entity (Texture2D texture, Vector2 centerPosition, int health = 1, float attacksPerSecond = 1, GameObject parent = null, float moveSpeed = 0, bool canRotate = false, bool canMove = true)
			: base(texture, centerPosition, parent: parent, moveSpeed: moveSpeed, canRotate: canRotate, canMove: canMove) {
			Health = health;
			AttacksPerSecond = attacksPerSecond;

			secondsSinceLastDamage = Main.DAMAGE_INDIC_TIME + 1;
		}

		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			base.Update(gameTime, mouse, keyboard);

			secondsSinceLastAttack += (float) gameTime.ElapsedGameTime.TotalSeconds;
			secondsSinceLastDamage += (float) gameTime.ElapsedGameTime.TotalSeconds;
		}

		public new void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			if (IsOnScreen) {
				SpriteManager.DrawImage(spriteBatch, texture, Rect, ((WasDamaged) ? Color.Red : Color.White), angle: Angle);
			}
		}

		protected void ShootBullet (int bulletDamage) {
			if (CanAttack) {
				Main.ListOfBullets.Add(new Bullet(Main.bulletTexture, Position, this, Angle, bulletDamage: bulletDamage));

				secondsSinceLastAttack = 0;
			}
		}

		protected void AttackEntityDirectly (Entity other) {
			if (CanAttack) {
				other.TakeDamage(10);

				secondsSinceLastAttack = 0;
			}
		}

		public void TakeDamage (int damage) {
			Health -= damage;
			secondsSinceLastDamage = 0;

			if (Health <= 0) {
				Destroy( );
			}
		}
	}
}

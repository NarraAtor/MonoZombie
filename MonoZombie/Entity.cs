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

		private Rectangle healthBarBorder;
		private Rectangle healthBar;
		private int maxHealth;

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
			Health = maxHealth = health;
			AttacksPerSecond = attacksPerSecond;

			secondsSinceLastDamage = Main.DAMAGE_INDIC_TIME + 1;
			
			healthBarBorder = new Rectangle(Rect.X, Rect.Y - Main.HEALTHBAR_OFFSET, Rect.Width, Rect.Height / 4);
			healthBar = new Rectangle(healthBarBorder.X + Main.HEALTHBAR_PADDING, healthBarBorder.Y + Main.HEALTHBAR_PADDING, healthBarBorder.Width - (Main.HEALTHBAR_PADDING * 2), healthBarBorder.Height - (Main.HEALTHBAR_PADDING * 2));
		}

		public new void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			base.Update(gameTime, mouse, keyboard);

			secondsSinceLastAttack += (float) gameTime.ElapsedGameTime.TotalSeconds;
			secondsSinceLastDamage += (float) gameTime.ElapsedGameTime.TotalSeconds;

			// Update health bar position and width
			healthBarBorder.Location = new Point(Rect.X, Rect.Y - Main.HEALTHBAR_OFFSET);
			healthBar.Location = new Point(healthBarBorder.X + Main.HEALTHBAR_PADDING, healthBarBorder.Y + Main.HEALTHBAR_PADDING);
			healthBar.Width = (int) ((healthBarBorder.Width * ((float) Health / maxHealth)) - (Main.HEALTHBAR_PADDING * 2));
		}

		public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics) {
			if (IsOnScreen) {
				SpriteUtils.DrawImage(spriteBatch, texture, Rect, ((WasDamaged) ? Color.Red : Color.White), angle: Angle);

				// Draw the health bar
				SpriteUtils.DrawRect(spriteBatch, graphics, healthBarBorder, Color.Black);

				if (healthBar.Width > 0) {
					SpriteUtils.DrawRect(spriteBatch, graphics, healthBar, Color.Red);
				}
			}
		}

		protected void ShootBullet (int bulletDamage) {
			if (CanAttack) {
				Main.Bullets.Add(new Bullet(Main.bulletTexture, Position, this, Angle, bulletDamage: bulletDamage));

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

			Main.Particles.Add(new Particle($"-{damage}", Color.Red, Position, Main.DAMAGE_INDIC_TIME * 2, this));
		}
	}
}

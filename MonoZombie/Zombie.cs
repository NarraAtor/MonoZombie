using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoZombie {
	public class Zombie : Entity {
		private Vector2 toMove;

		public Zombie (Vector2 position, int health, float attacksPerSecond, int zombieSpeed, Tile spawnTile)
			: base(GetTexture( ), position, health, attacksPerSecond, parent: spawnTile, moveSpeed: zombieSpeed, canRotate: true) {

		}

		public void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard, Player player) {
			base.Update(gameTime, mouse, keyboard);

			// Rotate the zombie towards the player
			RotateTo(player.Position);

			// Calculate the direction the zombie needs to move as a normalized vector
			Vector2 movement = (player.Position - Position);
			movement.Normalize( );

			// Add the previous values that were removed by rounding
			movement += toMove;
			movement *= moveSpeed;

			// Calcuate the values removed by the rounding
			toMove = new Vector2(movement.X - MathF.Truncate(movement.X), movement.Y - MathF.Truncate(movement.Y));

			// Move the zombie
			MoveBy(movement);
		}

		public new bool CheckUpdateCollision (GameObject other) {
			bool didCollide = base.CheckUpdateCollision(other);

			// If the zombie has collided with the player and can attack, attack the player
			if (didCollide && typeof(Player).IsInstanceOfType(other)) {
				AttackEntityDirectly((Player) other);
			}

			return didCollide;
		}

		private static Texture2D GetTexture ( ) {
			Random random = new Random( );

			return Main.zombieTextures[random.Next(0, Main.zombieTextures.Length)];
		}
	}
}

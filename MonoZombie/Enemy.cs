﻿using System;
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

		private Vector2 toMove;

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

		public Enemy (Vector2 position, int health, int moveSpeed, float attacksPerSecond, GameObject parent = null) : base(GetTexture( ), position, parent: parent, moveSpeed: moveSpeed, canRotate: true) {
			Health = health;
			this.attacksPerSecond = attacksPerSecond;

			timeSinceLastDamage = Main.DAMAGE_INDIC_TIME + 1;
		}

		public void TakeDamage (int damage) {
			Health -= damage;

			timeSinceLastDamage = 0;
		}

		public void Move (Player player) {
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
			//MoveBy(movement);
			AStar(Main.GetMapGraph.GetZombieVertex(this), Main.GetMapGraph.GetPlayerVertex());
		}

		/// <summary>
		/// Purpose: The fastest known pathfinding algorithim. 
		///			 Finds the shortest path between the zombie's vertex and the player's vertex.
		/// </summary>
		/// <param name="start">the zombie's vertex</param>
		/// <param name="goal">the player's vertex</param>
		/// <param name="heuristic">i have no freaking clue</param>
		private void AStar(MapSegment start, MapSegment goal /*, h heuristic*/)
		{
			List<MapSegment> open = new List<MapSegment>(); // set of nodes to be evaluated
			List<MapSegment> closed = new List<MapSegment>(); // set of nodes already evaluated
			open.Add(start);

			//the tutorial suggests making the loop go on forever until it returns.
			while(open.Count > 0)
			{
				MapSegment current = open[0];
				for(int i = 1; i < open.Count; i++)
				{
					if(open[i].FValue < current.FValue || 
					  (open[i].FValue == current.FValue && open[i].HValue < current.HValue))
					{
						current = open[i];
					}
				}

				open.Remove(current);
				closed.Add(current);

				if(current == goal)
				{
					return;
				}
			}
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

			base.Update(gameTime, mouse, keyboard);
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

		private static Texture2D GetTexture ( ) {
			Random random = new Random( );

			return Main.zombieTextures[random.Next(0, Main.zombieTextures.Length)];
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

// Author : Frank Alfano, Matthew Sorrentino
// Purpose : A class for each of the tiles that is part of the map, allowing collision and textures to be checked/given to each

namespace MonoZombie {
	public enum TileType {
		Grass,
		Wall,
		Gravel,
		Lava,
		Speed
	}

	public class Tile : GameObject {
		public bool IsWalkable {
			get;
		}

		public Tile (TileType tileType, Vector2 centerPosition, bool isWalkable = true) : base(GetTexture(tileType), centerPosition) {
			IsWalkable = isWalkable;
		}

		public override void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {

		}

		/*
		 * Overridden from the base GameObject class
		 */
		public new bool CheckCollision (GameObject other) {
			return !IsWalkable && base.CheckCollision(other);
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Get a texture for the tile based on its type
		 * 
		 * TileType tileType					: The type of the tile
		 * 
		 * return Texture2D						: The texture to set for the tile
		 */
		private static Texture2D GetTexture (TileType tileType) {
			// For generating a random index to get a random texture for the tile
			Random random = new Random( );

			// Based on what type the tile is, get a random texture from its corresponding array
			switch (tileType) {
				case TileType.Grass:
					return Game1.grassTextures[random.Next(0, Game1.grassTextures.Length)];
				case TileType.Wall:
					return Game1.wallTextures[random.Next(0, Game1.wallTextures.Length)];
				case TileType.Gravel:
					return null;
				case TileType.Lava:
					return null;
				case TileType.Speed:
					return null;
				default:
					return null;
			}
		}
	}
}

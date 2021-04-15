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

		public Tile (TileType tileType, Vector2 centerPosition, bool isWalkable = true) : base(GetTexture(tileType), centerPosition, canMove: false) {
			IsWalkable = isWalkable;
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Overridden from the base GameObject class
		 */
		public new bool CheckUpdateCollision (GameObject other) {
			return !IsWalkable && base.CheckUpdateCollision(other);
		}

		/*
		 * Author : Frank Alfano, Ken A.
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
			int rng = random.Next(10);
			// Based on what type the tile is, get a random texture from its corresponding array
			switch (tileType) {
				case TileType.Grass:
					if(rng < 8)
                    {
						return Game1.grassTextures[0];
					}
					else if(rng >= 8 && rng < 9)
                    {
						return Game1.grassTextures[1];
					}
                    else
                    {
						return Game1.grassTextures[2];
					}

				case TileType.Wall:
					if (rng < 8)
					{
						return Game1.wallTextures[0];
					}
					else if (rng >= 8 && rng < 9)
					{
						return Game1.wallTextures[1];
					}
					else
					{
						return Game1.wallTextures[2];
					}

				case TileType.Gravel:
					return Game1.gravelTextures[0];

				case TileType.Lava:
					return Game1.lavaTextures[0];

				case TileType.Speed:
					return Game1.speedTextures[0];

				default:
					return Game1.grassTextures[0];
			}
		}
	}
}

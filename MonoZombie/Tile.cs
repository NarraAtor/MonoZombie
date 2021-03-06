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

		public TileType Type {
			get;
		}

		public Tile (TileType tileType, Vector2 centerPosition, bool isWalkable = true) : base(GetTexture(tileType), centerPosition, canMove: false) {
			Type = tileType;
			IsWalkable = isWalkable;
		}

		/*
		 * Author : Frank Alfano, Ken Adachi-Bartholomay
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

			// * Some tiles that return "nullTexture" here are going to be bitmasked later after all of the map tiles have been created. Their
			// texture will be updated correctly there.

			// Based on what type the tile is, get a random texture from its corresponding array
			switch (tileType) {
				case TileType.Grass:
					if (rng < 8) {
						return Main.grassTextures[0];
					} else if (rng >= 8 && rng < 9) {
						return Main.grassTextures[1];
					} else {
						return Main.grassTextures[2];
					}

				case TileType.Wall:
					return Main.nullTexture;

				case TileType.Gravel:
					return Main.nullTexture;

				case TileType.Lava:
					return Main.lavaTextures[0];

				case TileType.Speed:
					return Main.speedTextures[0];

				default:
					return Main.grassTextures[0];
			}
		}

		public void SetTexture (Texture2D texture) {
			this.texture = texture;
		}
	}
}

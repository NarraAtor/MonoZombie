using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public enum TileType {
		Grass,
		Wall,
		Gravel,
		Lava,
		Speed
	}

	public class Tile : GameObject {
		private bool isWalkable;

		public Tile (TileType tileType, Vector2 position, bool isWalkable = true) : base(GetTexture(tileType), position) {
			this.isWalkable = isWalkable;
		}

		public override void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {

		}

		public new void Draw (SpriteBatch spriteBatch) {
			SpriteManager.DrawImage(spriteBatch, texture, position, scale:SpriteManager.ObjectScale);
		}

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

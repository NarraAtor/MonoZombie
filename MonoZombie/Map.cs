using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoZombie {
	public class Map {
		private Dictionary<int, int> bitmaskValues = new Dictionary<int, int>( ) {
			[2] = 1,
			[8] = 2,
			[10] = 3,
			[11] = 4,
			[16] = 5,
			[18] = 6,
			[22] = 7,
			[24] = 8,
			[26] = 9,
			[27] = 10,
			[30] = 11,
			[31] = 12,
			[64] = 13,
			[66] = 14,
			[72] = 15,
			[74] = 16,
			[75] = 17,
			[80] = 18,
			[82] = 19,
			[86] = 20,
			[88] = 21,
			[90] = 22,
			[91] = 23,
			[94] = 24,
			[95] = 25,
			[104] = 26,
			[106] = 27,
			[107] = 28,
			[120] = 29,
			[122] = 30,
			[123] = 31,
			[126] = 32,
			[127] = 33,
			[208] = 34,
			[210] = 35,
			[214] = 36,
			[216] = 37,
			[218] = 38,
			[219] = 39,
			[222] = 40,
			[223] = 41,
			[248] = 42,
			[250] = 43,
			[251] = 44,
			[254] = 45,
			[255] = 46,
			[0] = 47
		};

		public Tile[, ] Tiles {
			get;
			private set;
		}

		public Tile this[int x, int y] {
			get {
				if (x >= 0 && x < Width && y >= 0 && y < Height) {
					return Tiles[x, y];
				}

				return null;
			}
		}

		public GameObject[ ] CollidableTiles {
			get;
			private set;
		}

		public int Width {
			get {
				return Tiles.GetLength(0);
			}
		}

		public int Height {
			get {
				return Tiles.GetLength(1);
			}
		}

		public Map (string mapFilePath) {
			LoadMap(mapFilePath);
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Update the map tiles
		 * 
		 * GameTime gameTime		: Used to get the current time in the game
		 * MouseState mouse			: The current state of the mouse
		 * KeyboardState keyboard	: The current state of the keyboard
		 * 
		 * return					:
		 */
		public void Update (GameTime gameTime, MouseState mouse, KeyboardState keyboard) {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					Tiles[x, y].Update(gameTime, mouse, keyboard);
				}
			}
		}

		public void UpdateCameraScreenPosition (Camera camera) {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					Tiles[x, y].UpdateCameraScreenPosition(camera);
				}
			}
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw the map tiles
		 * * This method needs to be called within a SpriteBatch Begin() and End() draw methods
		 * 
		 * GameTime gameTime		: Used to get the current time in the game
		 * SpriteBatch spriteBatch	: The SpriteBatch object used to draw textures for the game
		 * 
		 * return					:
		 */
		public void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					Tiles[x, y].Draw(gameTime, spriteBatch);
				}
			}
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Load the map values based on the map file specified
		 * 
		 * string filepath					: The path to the map file
		 * 
		 * return							: 
		 */
		private void LoadMap (string filepath) {
			// Read all the lines from the file
			string[ ] lines = File.ReadAllLines(filepath);

			// Get the dimensions of the map
			// The dimensions of the map is on the first line of the map file
			string[ ] mapDimensions = lines[0].Split("|");
			int mapWidth = int.Parse(mapDimensions[0]);
			int mapHeight = int.Parse(mapDimensions[1]);

			// Create an array with the correct width and height of the map which all the tile objects will
			// be stored in
			Tile[ , ] loadedTiles = new Tile[mapWidth, mapHeight];

			// Loop through the map file lines and load in all the tile values
			for (int i = 1; i < lines.Length; i++) {
				// Get the current x and y indexes of the current map tile that needs to be loaded in
				int currX = (i - 1) / mapWidth;
				int currY = (i - 1) - (currX * mapHeight);

				// Based on the current indexes of the map tiles, get their positions on the screen
				// First, get the dimensions of the actual tile sprite in pixels
				Vector2 tileBaseSpriteDimensions = Main.grassTextures[0].Bounds.Size.ToVector2( );
				Vector2 tileSpriteDimensions = tileBaseSpriteDimensions * SpriteUtils.OBJECT_SCALE;

				// Calculate the x and y of the tile incorperating the fact that the sprites need to be scaled up for the game
				// Also, shift the sprites a bit to get their center position rather than the top left position. This makes it a lot
				// easier to draw they using the methods in the SpriteManager class
				int tileX = (int) ((currX * tileSpriteDimensions.X) + (tileSpriteDimensions.X / 2));
				int tileY = (int) ((currY * tileSpriteDimensions.Y) + (tileSpriteDimensions.Y / 2));

				// Make sure to reposition the tiles so that way the map is centered around the center of the screen
				// This also means the player will spawn at the center of the screen
				int mapPixelWidth = mapWidth * (int) tileSpriteDimensions.X;
				int mapPixelHeight = mapHeight * (int) tileSpriteDimensions.Y;
				Vector2 tilePosition = new Vector2((Main.SCREEN_DIMENSIONS.X / 2) - (mapPixelWidth / 2) + tileX, (Main.SCREEN_DIMENSIONS.Y / 2) - (mapPixelHeight / 2) + tileY);

				// Get the type of the tile from the file
				TileType tileType = (TileType) Enum.Parse(typeof(TileType), lines[i], true);

				// Create a tile object and set it to its index position in the 2D tile array of the map
				loadedTiles[currX, currY] = new Tile(tileType, tilePosition, !(tileType == TileType.Lava || tileType == TileType.Wall));
			}

			Tiles = loadedTiles;

			// Get all of the tiles in the map that have colliders
			CollidableTiles = GetColliders( );

			// Update all textures that are reliant on bitmasking
			UpdateBitmaskedTextures( );
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Get all tiles that are not walkable (or "colliders" as I am calling them)
		 * 
		 * return GameObject[]			: A list of all the collidable tiles on the map
		 */
		private GameObject[ ] GetColliders ( ) {
			List<GameObject> tileColliders = new List<GameObject>( );

			// Loop through all the tiles and only get the ones that are collidable
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					if (!Tiles[x, y].IsWalkable) {
						tileColliders.Add(Tiles[x, y]);
					}
				}
			}

			return tileColliders.ToArray( );
		}

		private void UpdateBitmaskedTextures () {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					// Get the current type of the current tile
					TileType currType = Tiles[x, y].Type;

					// If the tile is not equal to a tile that need to be bitmasked, then just continue to the next tile
					if (currType != TileType.Gravel && currType != TileType.Wall) {
						continue;
					}

					// Get the values of the surrounding tiles
					int[ ] surroundingValues = GetSurroundingTileSimilarities(x, y, currType);

					// Calculate the bitmask values for this tile
					int north = 2 * surroundingValues[1];
					int east = 16 * surroundingValues[3];
					int south = 64 * surroundingValues[5];
					int west = 8 * surroundingValues[7];

					int northWest = 0;
					int northEast = 0;
					int southEast = 0;
					int southWest = 0;

					if (north != 0 && west != 0) {
						northWest = 1 * surroundingValues[0];
					}

					if (north != 0 && east != 0) {
						northEast = 4 * surroundingValues[2];
					}

					if (east != 0 && south != 0) {
						southEast = 128 * surroundingValues[4];
					}

					if (west != 0 && south != 0) {
						southWest = 32 * surroundingValues[6];
					}

					// Get the total value of the surrounding bitmask values
					int totalValue = northWest + north + northEast + east + southEast + south + southWest + west;
					int textureIndex = -1;

					// Convert the bitmask value to a value in the texture array
					bitmaskValues.TryGetValue(totalValue, out textureIndex);

					// Set the current tiles texture to the bitmask caluclated texture
					switch (currType) {
						case TileType.Wall:
							Tiles[x, y].SetTexture(Main.wallTextures[textureIndex]);
							break;
						case TileType.Gravel:
							Tiles[x, y].SetTexture(Main.gravelTextures[textureIndex]);
							break;
						default:
							Console.WriteLine("Yikes this isnt right bro");
							break;
					}
				}
			}
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Get all of the values of the surrounding tiles of a certain tile
		 * 
		 * int x				: The x position of the current tile
		 * int y				: The y position of the current tile
		 * TileType trueTile	: The value of the surrounding tiles to determine if it is similar or not
		 * 
		 */
		private int[ ] GetSurroundingTileSimilarities (int x, int y, TileType trueType) {
			int[ ] values = new int[8];

			// * NOTE * The y is shifted in the "wrong" direction because screens are wacky and a lower y value means higher on the screen
			values[0] = ((this[x - 1, y - 1] == null || trueType == this[x - 1, y - 1].Type) ? 1 : 0);
			values[1] = ((this[x, y - 1] == null || trueType == this[x, y - 1].Type) ? 1 : 0);
			values[2] = ((this[x + 1, y - 1] == null || trueType == this[x + 1, y - 1].Type) ? 1 : 0);
			values[3] = ((this[x + 1, y] == null || trueType == this[x + 1, y].Type) ? 1 : 0);
			values[4] = ((this[x + 1, y + 1] == null || trueType == this[x + 1, y + 1].Type) ? 1 : 0);
			values[5] = ((this[x, y + 1] == null || trueType == this[x, y + 1].Type) ? 1 : 0);
			values[6] = ((this[x - 1, y + 1] == null || trueType == this[x - 1, y + 1].Type) ? 1 : 0);
			values[7] = ((this[x - 1, y] == null || trueType == this[x - 1, y].Type) ? 1 : 0);

			return values;
		}
	}
}

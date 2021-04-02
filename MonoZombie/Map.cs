using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoZombie {
	public class Map {
		private Tile[ , ] tiles;

		private Vector2[ ] zombieSpawns;

		public int Width {
			get {
				return tiles.GetLength(0);
			}
		}

		public int Height {
			get {
				return tiles.GetLength(1);
			}
		}

		public int PixelWidth {
			get {
				return (int) (Width * Game1.grassTextures[0].Width * SpriteManager.ObjectScale);
			}
		}

		public int PixelHeight {
			get {
				return (int) (Height * Game1.grassTextures[0].Height * SpriteManager.ObjectScale);
			}
		}

		public Map (string mapFilePath) {
			tiles = LoadMap(mapFilePath);
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
					tiles[x, y].Update(gameTime, mouse, keyboard);
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
		public void Draw (SpriteBatch spriteBatch) {
			for (int x = 0; x < Width; x++) {
				for (int y = 0; y < Height; y++) {
					tiles[x, y].Draw(spriteBatch);
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
		 * return Tile[,]					: The loaded 2D array of tiles
		 */
		private Tile[ , ] LoadMap (string filepath) {
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
				Vector2 tileSpriteDimensions = Game1.grassTextures[0].Bounds.Size.ToVector2( );

				// Calculate the x and y of the tile incorperating the fact that the sprites need to be scaled up for the game
				int tileX = currX * (int) (tileSpriteDimensions.X * SpriteManager.ObjectScale);
				int tileY = currY * (int) (tileSpriteDimensions.Y * SpriteManager.ObjectScale);
				Vector2 tilePosition = new Vector2(tileX, tileY);

				// Get the type of the tile from the file
				TileType tileType = (TileType) Enum.Parse(typeof(TileType), lines[i], true);

				// Create a tile object and set it to its index position in the 2D tile array of the map
				loadedTiles[currX, currY] = new Tile(tileType, tilePosition, !(tileType == TileType.Lava || tileType == TileType.Wall));
			}

			return loadedTiles;
		}
	}
}

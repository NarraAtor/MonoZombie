using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace MonoZombie {
	public enum MenuState {
		MainMenu,
		Game,
		GameOver
	}

	public enum GameState {
		Playing,
		Pause,
		Shop
	}

	/// <summary>
	/// Author: Eric Fotang
	/// Purpose: Manages game states and calls other classes and methods to do their job. 
	/// Restrictions:
	/// </summary>
	public class Game1 : Game {
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private MenuState menuState;
		private GameState gameState;

		private KeyboardState currKeyboardState;
		private KeyboardState prevKeyboardState;
		private MouseState currMouseState;

		//Test variables
		private string currentStateTEST;

		private static Vector2 screenDimensions;

		// Fonts
		public static SpriteFont font;

		// UI Textures
		public static Texture2D titleTexture;
		public static Texture2D buttonTexture;
		public static Texture2D tabTexture;

		// UI Button Variables
		private UIButton menuPlayButton;
		private UIButton menuQuitButton;
		private UIButton pauseResumeButton;
		private UIButton pauseMenuButton;
		private UIButton gameOverMenuButton;

		// Map Tile Texture Arrays
		// * These are arrays because when a tile is created, it picks a random texture from these
		// arrays to add variation to the map
		public static Texture2D[ ] grassTextures;
		public static Texture2D[ ] wallTextures;

		private Map map;

		private static Texture2D turretImage;
		private static Texture2D baseImage;
		private static Texture2D playerImage;
		private static Texture2D enemyImage;
		private static List<Turret> listOfTurrets;
		private static List<Enemy> listOfZombies;
		private static Turret turret;
		private static Player player;
		private static Enemy zombie;
		private static int currency;
		private static int roundNumber;
		private static bool roundIsOngoing;
		private static bool aZombieIsAlive;

		public static Player Player {
			get {
				return player;
			}
		}

		public Game1 ( ) {
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize ( ) {
			// TODO: Add your initialization logic here
			menuState = MenuState.MainMenu;
			gameState = GameState.Playing;
			currency = 0;
			roundNumber = 0;
			listOfZombies = new List<Enemy>( );
			listOfTurrets = new List<Turret>( );
			aZombieIsAlive = false;

			base.Initialize( );
		}

		protected override void LoadContent ( ) {
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load textures for game objects
			baseImage = Content.Load<Texture2D>("TurretBase");
			turretImage = Content.Load<Texture2D>("TurretHead");
			playerImage = Content.Load<Texture2D>("playerproto");
			enemyImage = Content.Load<Texture2D>("zombieproto");

			// Load map tile textures
			grassTextures = new Texture2D[ ] {
				Content.Load<Texture2D>("GrassTile1"),
				Content.Load<Texture2D>("GrassTile2"),
				Content.Load<Texture2D>("GrassTile3")
			};

			wallTextures = new Texture2D[ ] {
				Content.Load<Texture2D>("WallTile1"),
				Content.Load<Texture2D>("WallTile2"),
				Content.Load<Texture2D>("WallTile3")
			};

			// Load fonts
			font = Content.Load<SpriteFont>("5Pixel");

			// Load UI Textures
			titleTexture = Content.Load<Texture2D>("title");
			buttonTexture = Content.Load<Texture2D>("button");
			tabTexture = Content.Load<Texture2D>("tab");

			// Load the map
			map = new Map("../../../MapLevels\\CurrentMapDesign.level");

			// Update the dimensions of the screen
			screenDimensions = new Vector2(map.PixelWidth, map.PixelHeight);

			_graphics.PreferredBackBufferWidth = (int) screenDimensions.X;
			_graphics.PreferredBackBufferHeight = (int) screenDimensions.Y;
			_graphics.ApplyChanges( );

			//Texture reliant intitialization
			turret = new Turret(TurretType.Archer, baseImage, turretImage, new Vector2(100, 100));
			player = new Player(100, 100, playerImage, screenDimensions / 2, 3);
			zombie = new Enemy(enemyImage, new Vector2((_graphics.PreferredBackBufferWidth / 2) + 30, _graphics.PreferredBackBufferHeight / 2), 100, 1, 5);

			// Create UI Buttons
			menuPlayButton = new UIButton("Play", screenDimensions / 2, ( ) => {
				menuState = MenuState.Game;
				gameState = GameState.Playing;
				roundIsOngoing = true;
			}, true);

			// test zombie list
			// listOfZombies.Add(zombie);

			base.LoadContent( );
		}

		protected override void Update (GameTime gameTime) {
			// Get the current keyboard state
			currKeyboardState = Keyboard.GetState( );
			currMouseState = Mouse.GetState( );

			switch (menuState) {
				case MenuState.MainMenu:
					currentStateTEST = "MainMenu";

					// Update the menu UI elements
					menuPlayButton.Update(gameTime, currMouseState);

					break;
				case MenuState.Game:
					currentStateTEST = "Game -";

					switch (gameState) {
						case GameState.Playing:
							currentStateTEST = "Game - Playing";

							/*
							//This code rewards the player when a zombie is killed and makes the round end when in contact with a zombie.
							aZombieIsAlive = false;

							foreach (Enemy zombie in listOfZombies) {
								//If a zombie just died, set indicate that it is dead an increment currency.
								if (zombie.Health <= 0 && zombie.IsAlive) {
									zombie.Die( );
									currency++;
								}


                                if (zombie.IsAlive)
                                {
                                    aZombieIsAlive = true;
                                    zombie.Update(gameTime, player);
                                }

                                //Check if any zombies are in range of the turrets
                                foreach (Turret turret in listOfTurrets)
                                {
                                    turret.Update(zombie);
                                }
                            }

							if (aZombieIsAlive!) {
								roundIsOngoing = false;
							}
							*/

							map.Update(gameTime, currMouseState, currKeyboardState);

							player.Update(gameTime, currMouseState, currKeyboardState);

							if (GetKeyDown(Keys.Escape)) {
								gameState = GameState.Pause;
							}

							if (GetKeyDown(Keys.Tab)) {
								gameState = GameState.Shop;
							}

							break;
						case GameState.Pause:
							currentStateTEST = "Game - Pause";

							if (GetKeyDown(Keys.Escape)) {
								gameState = GameState.Playing;
							}

							break;
						case GameState.Shop:
							currentStateTEST = "Game - Shop";

							if (GetKeyDown(Keys.Tab)) {
								gameState = GameState.Playing;
							}

							break;
					}
					break;

				case MenuState.GameOver:
					currentStateTEST = "GameOver";

					if (GetKeyDown(Keys.Enter)) {
						menuState = MenuState.MainMenu;
					}

					break;
			}

			// Update the past keyboard state to the current one as Update() has ended this frame
			prevKeyboardState = currKeyboardState;

			base.Update(gameTime);
		}

		protected override void Draw (GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// * These settings in SpriteBatch.Begin() prevent sprites from becoming blurry when scaled up. This
			// means we can make pixel art images and import them into the game very small and then scale them up.
			// This makes the images a lot easier to edit if we need to do that again.
			_spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

			switch (menuState) {
				case MenuState.MainMenu:
					// Draw menu UI objects
					SpriteManager.DrawImage(_spriteBatch, titleTexture, screenDimensions * new Vector2(0.5f, 0.25f), scale: SpriteManager.UIScale, isCentered: true);
					menuPlayButton.Draw(_spriteBatch);

					break;
				case MenuState.Game:
					switch (gameState) {
						case GameState.Playing:
							// Draw the map
							map.Draw(_spriteBatch, player);

							// Draw the player
							player.Draw(_spriteBatch);

							/*
							turret.Draw(_spriteBatch, Color.White);

                            foreach(Enemy zombie in listOfZombies)
                            {
                                zombie.Draw(_spriteBatch);
                            }

                            foreach (Turret turret in listOfTurrets)
                            {
                                turret.Draw(_spriteBatch, Color.White);
                            
							*/

							// Draw UI elements
							SpriteManager.DrawImage(_spriteBatch, tabTexture, new Vector2(15, 15), scale: SpriteManager.UIScale);
							SpriteManager.DrawText(_spriteBatch, 0.5f, $"Currency: {currency}", Color.Black, new Vector2(30, 30));
							SpriteManager.DrawText(_spriteBatch, 0.5f, $"Round Number: {roundNumber}", Color.Black, new Vector2(30, 45));
							SpriteManager.DrawText(_spriteBatch, 0.5f, $"Player Health: {player.Health}", Color.Black, new Vector2(30, 60));
							SpriteManager.DrawText(_spriteBatch, 0.5f, $"Zombie Timer: {zombie.Timer}", Color.Black, new Vector2(30, 75));

							break;
						case GameState.Pause:

							break;
						case GameState.Shop:

							break;
					}

					break;
				case MenuState.GameOver:

					break;
			}

			// Being used to test if states are switching properly
			_spriteBatch.DrawString(font, currentStateTEST, new Vector2(15, 900), Color.White);

			_spriteBatch.End( );

			base.Draw(gameTime);
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Get whether a key was pressed this current frame
		 * 
		 * Keys key					: The key to check
		 * 
		 * return bool				: Whether or not the key was pressed this frame
		 */
		private bool GetKeyDown (Keys key) {
			return (currKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key));
		}
	}
}

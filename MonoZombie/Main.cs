using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace MonoZombie {
	public enum MenuState {
		MainMenu,
		Game,
		GameOver
	}

	public enum GameState {
		Playing,
		Pause,
		Shop,
		ShopInPlacment,
		InBetweenRounds
	}

	/// <summary>
	/// Author: Eric Fotang
	/// Purpose: Manages game states and calls other classes and methods to do their job. 
	/// Restrictions:
	/// </summary>
	public class Main : Game {
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		// Game states
		private MenuState menuState;
		private GameState gameState;

		// Input states
		private KeyboardState currKeyboardState;
		private KeyboardState prevKeyboardState;
		private MouseState currMouseState;
		private MouseState prevMouseState;

		// Debug variables
		private float[ ] debugTimes = new float[3];
		private Stopwatch debugTimer = new Stopwatch( );
		private bool isInDebugMode;

		//Test variables
		private bool easyModeTEST;

		// Camera
		private Camera camera;

		// Fonts
		public static SpriteFont font;

		// UI Button Variables
		private UIButton menuPlayButton;
		private UIButton menuPlayEasyModeButton;
		private UIButton menuQuitButton;
		private UIButton pauseResumeButton;
		private UIButton pauseMenuButton;
		private UIButton gameOverMenuButton;

		// Map
		private Map map;

		// Map Graph
		private static MapGraph graph;

		// Map Tile Texture Arrays
		// * These are arrays because when a tile is created, it picks a random texture from these
		// arrays to add variation to the map
		public static Texture2D[ ] grassTextures;
		public static Texture2D[ ] wallTextures;
		public static Texture2D[ ] gravelTextures;
		public static Texture2D[ ] lavaTextures;
		public static Texture2D[ ] speedTextures;

		public static Texture2D[ ] zombieTextures;

		// Game Object Textures
		public static Texture2D nullTexture;
		public static Texture2D playerTexture;
		public static Texture2D bulletTexture;

		public static Texture2D turretArcherBaseTexture;
		public static Texture2D turretArcherHeadTexture;
		public static Texture2D mineHeadTexture;
		public static Texture2D buffTexture;
		public static Texture2D cannonTexture;

		// UI Textures
		public static Texture2D titleTexture;
		public static Texture2D buttonTexture;
		public static Texture2D tabTexture;

		// Game Logic Variables
		public static int currency;
		private int roundNumber;
		private bool isInBetweenRounds;
		private bool isPaused;
		private bool isInShop;

		// Turret
		private List<Turret> turretButtonList;                      // the list that holds all of the turret images
		private List<string> turretNames;                           // holds the names of the turret types, please update
																	// when new turrets are added to the ButtonList
		private Turret turretInPurchase;                            // the turret that the player is currently purchasing from the shop.
		private List<Turret> turretList;                            // turrets that exist in the game;
		private List<int> turretsPurchased;                         // the list of what turrets have been purchased 
																	// should be directly linked with the turretButtonList

		// Constants
		public const int ZOMBIE_BASE_HEALTH = 100; // The default health of the zombie
		public const int ZOMBIE_BASE_MOVESPEED = 2; // The default movespeed of the zombie
		public const int ZOMBIE_BASE_ATTACKSPEED = 1; // The default attackspeed of the zombie
		public const int ZOMBIE_BASE_COUNT = 5; // The starting number of zombies in round 1
		public const float DAMAGE_INDIC_TIME = 0.25f; // The amount of seconds that entities flash when they are damaged
		public const int BULLET_SPEED = 15;
		public const int CANNON_BULLET_DAMAGE = 202;
		public const int ARCHER_BULLET_DAMAGE = 40;
		public const int PLAYER_BULLET_DAMAGE = 10;
		public static Vector2 SCREEN_DIMENSIONS = new Vector2(1280, 720);

		private static Random random;

		public static List<Bullet> Bullets {
			get;
		} = new List<Bullet>( );

		public static List<Zombie> Zombies {
			get;
		} = new List<Zombie>( );

		public static List<Turret> Turrets {
			get;
		} = new List<Turret>( );

		public static List<Particle> Particles {
			get;
		} = new List<Particle>( );

		public static Player Player {
			get;
			private set;
		}

		public static MapGraph GetMapGraph { get { return graph; } }

		public Main ( ) {
			graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";

			IsMouseVisible = true;
		}

		protected override void Initialize ( ) {
			menuState = MenuState.MainMenu;
			gameState = GameState.Playing;

			easyModeTEST = false;

			turretButtonList = new List<Turret>( );
			turretNames = new List<string>( );
			turretsPurchased = new List<int>( );

			random = new Random( );

			base.Initialize( );
		}

		protected override void LoadContent ( ) {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load textures for game objects
			nullTexture = Content.Load<Texture2D>("MapTiles/NullTile");
			turretArcherBaseTexture = Content.Load<Texture2D>("Turrets/TurretCannonBase");
			turretArcherHeadTexture = Content.Load<Texture2D>("Turrets/TurretCannonHead");
			mineHeadTexture = Content.Load<Texture2D>("Turrets/MineHead");
			buffTexture = Content.Load<Texture2D>("Turrets/Buff");
			cannonTexture = Content.Load<Texture2D>("Turrets/Canon");
			playerTexture = Content.Load<Texture2D>("Player");
			bulletTexture = Content.Load<Texture2D>("Bullet");

			zombieTextures = new Texture2D[ ] {
				Content.Load<Texture2D>("Zombie1"),
				Content.Load<Texture2D>("Zombie2"),
				Content.Load<Texture2D>("Zombie3")
			};

			// Load map tile textures
			grassTextures = new Texture2D[ ] {
				Content.Load<Texture2D>("MapTiles/Grass/GrassTile1"),
				Content.Load<Texture2D>("MapTiles/Grass/GrassTile2"),
				Content.Load<Texture2D>("MapTiles/Grass/GrassTile3")
			};

			// * NOTE * When loading the textures, I have not completed all of the map tile sprites for every case. So,
			// for right now, I just made a "null" tile that appears whenever there is a wall tile that does not match one
			// of the ones I have already made. In the future all of these will be replaced with actual textures
			wallTextures = new Texture2D[ ] {
				Content.Load<Texture2D>("MapTiles/Walls/WallTile1"),
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile5"),
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile8"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile13"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile27"),
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile29"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile34"),
				Content.Load<Texture2D>("MapTiles/Walls/WallTile35"),
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile37"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile42"),
				Content.Load<Texture2D>("MapTiles/Walls/WallTile43"),
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Walls/WallTile45"),
				Content.Load<Texture2D>("MapTiles/Walls/WallTile46"),
				Content.Load<Texture2D>("MapTiles/Walls/WallTile1"),
				nullTexture,
			};

			gravelTextures = new Texture2D[ ] {
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile1"),
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile5"),
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile8"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile13"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile27"),
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile29"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile34"),
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile35"),
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile37"),
				nullTexture,
				nullTexture,
				nullTexture,
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile42"),
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile43"),
				nullTexture,
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile45"),
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile46"),
				Content.Load<Texture2D>("MapTiles/Gravel/GravelTile1"),
				nullTexture,
			};

			lavaTextures = new Texture2D[ ]
			{
				Content.Load<Texture2D>("MapTiles/Lava")
			};

			speedTextures = new Texture2D[ ]
			{
				Content.Load<Texture2D>("MapTiles/Speed")
			};

			// Load fonts
			font = Content.Load<SpriteFont>("5Pixel");

			// Load UI Textures
			titleTexture = Content.Load<Texture2D>("title");
			buttonTexture = Content.Load<Texture2D>("button");
			tabTexture = Content.Load<Texture2D>("tab");

			// Load the map
			map = new Map("../../../MapLevels\\TestLevel1.level");

			graphics.PreferredBackBufferWidth = (int) SCREEN_DIMENSIONS.X;
			graphics.PreferredBackBufferHeight = (int) SCREEN_DIMENSIONS.Y;
			graphics.ApplyChanges( );

			//Test map graph creation
			graph = new MapGraph(map.Tiles);

			// Create UI Buttons
			menuPlayButton = new UIButton("Play", SCREEN_DIMENSIONS / 2, ( ) => {
				menuState = MenuState.Game;
				gameState = GameState.Playing;
				easyModeTEST = false;

				// Start the game
				ResetGame( );
				StartNextRound( );
			});

			menuPlayEasyModeButton = new UIButton("Easy Mode", SCREEN_DIMENSIONS / 2 + new Vector2(0f, 100f), ( ) => {
				menuState = MenuState.Game;
				gameState = GameState.Playing;
				easyModeTEST = true;

				// Start the game
				ResetGame( );
				StartNextRound( );
			});

			menuQuitButton = new UIButton("Quit", SCREEN_DIMENSIONS / 2 + new Vector2(0f, 200f), ( ) => {
				Exit( );
			});

			pauseResumeButton = new UIButton("Resume", new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 3 * 2), ( ) => {
				// gameState = GameState.Playing;

				isPaused = false;
			});

			pauseMenuButton = new UIButton("Menu", new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 3), ( ) => {
				menuState = MenuState.MainMenu;
			});


			// Initializing different turret types for the shop

			turretButtonList.Add(
				new Turret(TurretType.Archer, turretArcherBaseTexture, turretArcherHeadTexture, new Vector2(SCREEN_DIMENSIONS.X / 7 * 2, SCREEN_DIMENSIONS.Y / 5 * 2))
				);
			turretNames.Add("Archer");
			turretsPurchased.Add(1);

			turretButtonList.Add(
				new Turret(TurretType.Buff, turretArcherBaseTexture, buffTexture, new Vector2(SCREEN_DIMENSIONS.X / 7 * 4, SCREEN_DIMENSIONS.Y / 5 * 2))
				);
			turretsPurchased.Add(1);
			turretNames.Add("Buff");

			turretButtonList.Add(
				new Turret(TurretType.Buff, turretArcherBaseTexture, cannonTexture, new Vector2(SCREEN_DIMENSIONS.X / 7 * 6, SCREEN_DIMENSIONS.Y / 5 * 2))
				);
			turretsPurchased.Add(0);
			turretNames.Add("Canon");

			base.LoadContent( );
		}

		protected override void Update (GameTime gameTime) {
			currKeyboardState = Keyboard.GetState( );
			currMouseState = Mouse.GetState( );

			switch (menuState) {
				case MenuState.MainMenu:
					// Update the menu UI elements
					menuPlayButton.Update(gameTime, currMouseState);
					menuPlayEasyModeButton.Update(gameTime, currMouseState);
					menuQuitButton.Update(gameTime, currMouseState);

					break;
				case MenuState.Game:
					switch (gameState) {
						case GameState.Playing:
							//Console.WriteLine($"{graph.GetPlayerVertex().TileAtVertex.X} {graph.GetPlayerVertex().TileAtVertex.Y}");
							//foreach (Enemy zombie in ListOfZombies)
							//{
							//	Console.WriteLine($"{graph.GetZombieVertex(zombie).TileAtVertex.X} {graph.GetZombieVertex(zombie).TileAtVertex.Y}");
							//}
							//Console.WriteLine();

							if (!isPaused && !isInShop) {
								debugTimer.Start( );

								// Update all game objects
								Player.Update(gameTime, currMouseState, currKeyboardState);

								for (int i = Turrets.Count - 1; i >= 0; i--) {
									Turrets[i].Update(gameTime, currMouseState, currKeyboardState);
								}

								for (int i = Bullets.Count - 1; i >= 0; i--) {
									Bullets[i].Update(gameTime);
								}

								for (int i = Zombies.Count - 1; i >= 0; i--) {
									Zombies[i].Update(gameTime, currMouseState, currKeyboardState, Player);
								}

								for (int i = Particles.Count - 1; i >= 0; i--) {
									Particles[i].Update(gameTime);
								}

								debugTimes[0] = debugTimer.ElapsedMilliseconds;
								debugTimer.Reset( );

								// Do game logic calculations
								// Check if the player is dead
								if (Player.IsDead && !easyModeTEST) {
									menuState = MenuState.GameOver;
								}

								// If there are no more zombies, then advance to the next round
								if (!isInBetweenRounds && Zombies.Count == 0) {
									//loop through each turret and check if they are out of time and update their duration
									for (int i = Turrets.Count - 1; i >= 0; i--) {
										Turrets[i].TakeDamage(1);
									}

									isInBetweenRounds = true;
								}

								debugTimer.Start( );

								// Check collisions
								for (int i = 0; i < map.CollidableTiles.Length; i++) {
									for (int j = Zombies.Count - 1; j >= 0; j--) {
										Zombies[j].CheckUpdateCollision(map.CollidableTiles[i]);
									}

									for (int j = Turrets.Count - 1; j >= 0; j--) {
										Turrets[j].CheckUpdateCollision(map.CollidableTiles[i]);
									}

									for (int j = Bullets.Count - 1; j >= 0; j--) {
										Bullets[j].CheckUpdateCollision(map.CollidableTiles[i]);
									}

									Player.CheckUpdateCollision(map.CollidableTiles[i]);
								}

								for (int i = Zombies.Count - 1; i >= 0; i--) {
									for (int j = Zombies.Count - 1; j >= 0; j--) {
										Zombies[j].CheckUpdateCollision(Zombies[i]);
									}

									for (int j = Turrets.Count - 1; j >= 0; j--) {
										Turrets[j].CheckUpdateCollision(Zombies[i]);
									}

									Zombies[i].CheckUpdateCollision(Player);

									for (int j = Bullets.Count - 1; j >= 0; j--) {
										if (Bullets[j].CheckUpdateCollision(Zombies[i])) {
											break;
										}
									}
								}

								for (int i = Turrets.Count - 1; i >= 0; i--) {
									for (int j = Turrets.Count - 1; j >= 0; j--) {
										Turrets[j].CheckUpdateCollision(Turrets[i]);
									}

									Player.CheckUpdateCollision(Turrets[i]);
								}

								debugTimes[1] = debugTimer.ElapsedMilliseconds;
								debugTimer.Reset( );

								debugTimer.Start( );

								// Update camera screen positions of all game objects
								Player.UpdateCameraScreenPosition(camera);

								for (int i = Turrets.Count - 1; i >= 0; i--) {
									Turrets[i].UpdateCameraScreenPosition(camera);
								}

								for (int i = Bullets.Count - 1; i >= 0; i--) {
									Bullets[i].UpdateCameraScreenPosition(camera);
								}

								for (int i = Zombies.Count - 1; i >= 0; i--) {
									Zombies[i].UpdateCameraScreenPosition(camera);
								}

								for (int i = Particles.Count - 1; i >= 0; i--) {
									Particles[i].UpdateCameraScreenPosition(camera);
								}

								map.UpdateCameraScreenPosition(camera);

								debugTimes[2] = debugTimer.ElapsedMilliseconds;
								debugTimer.Reset( );
							}

							// Check key inputs
							if (GetKeyDown(Keys.Escape)) {
								isPaused = !isPaused;
							}

							if (GetKeyDown(Keys.OemQuestion)) {
								isInDebugMode = !isInDebugMode;
							}

							if (isInBetweenRounds) {
								if (GetKeyDown(Keys.Tab)) {
									isInShop = !isInShop;
								}

								if (GetKeyDown(Keys.Enter)) {
									StartNextRound( );
								}
							}

							if (isInShop) {
								for (int i = 0; i < turretButtonList.Count; i++) {
									if (currMouseState.X > turretButtonList[i].Rect.Left && currMouseState.X < turretButtonList[i].Rect.Right
											&& currMouseState.Y <= turretButtonList[i].Rect.Bottom && currMouseState.Y >= turretButtonList[i].Rect.Top) {
										Console.WriteLine(turretButtonList[i].Rect.Top + " " + turretButtonList[i].Rect.Bottom);
										if (currMouseState.LeftButton == ButtonState.Pressed) {
											if (currency >= 30) {
												//turretsPurchased[i]++;
												turretsPurchased[i]++;
												currency -= 30;
												break;
											}
										}
									}
								}
							}

							if (isPaused) {
								pauseResumeButton.Update(gameTime, currMouseState);
								pauseMenuButton.Update(gameTime, currMouseState);
							}

							/*
							if (GetKeyDown(Keys.T)) {
								if (turretsPurchased[0] > 0) {
									ListOfTurrets.Add(new Turret(TurretType.Archer, turretArcherBaseTexture, turretArcherHeadTexture, Player.Position, parent: Player));
									--turretsPurchased[0];
								}
							}

							if (GetKeyDown(Keys.Y)) {
								if (turretsPurchased[1] > 0) {
									ListOfTurrets.Add(new Turret(TurretType.Buff, turretArcherBaseTexture, buffTexture, Player.Position, parent: Player));
								}
							}

							if (GetKeyDown(Keys.U)) {
								if (turretsPurchased[2] > 0) {
									ListOfTurrets.Add(new Turret(TurretType.Cannon, turretArcherBaseTexture, turretArcherHeadTexture, Player.Position, parent: Player));
									--turretsPurchased[2];
								}
							}
							*/

							break;
					}

					break;
				case MenuState.GameOver:
					if (GetKeyDown(Keys.Enter)) {
						menuState = MenuState.MainMenu;
					}

					break;
			}

			// Update the past keyboard state to the current one as Update() has ended this frame
			prevKeyboardState = currKeyboardState;
			prevMouseState = currMouseState;
		}

		protected override void Draw (GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			// * These settings in SpriteBatch.Begin() prevent sprites from becoming blurry when scaled up. This
			// means we can make pixel art images and import them into the game very small and then scale them up.
			// This makes the images a lot easier to edit if we need to do that again.
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

			switch (menuState) {
				case MenuState.MainMenu:
					// Draw menu UI objects
					SpriteUtils.DrawImage(spriteBatch, titleTexture, SCREEN_DIMENSIONS * new Vector2(0.5f, 0.25f), Color.White, scale: SpriteUtils.UI_SCALE, isCentered: true);
					menuPlayButton.Draw(spriteBatch);
					menuPlayEasyModeButton.Draw(spriteBatch);
					menuQuitButton.Draw(spriteBatch);

					break;
				case MenuState.Game:
					switch (gameState) {
						case GameState.Playing:
							// Draw all game objects
							map.Draw(gameTime, spriteBatch);

							for (int i = Turrets.Count - 1; i >= 0; i--) {
								Turrets[i].Draw(gameTime, spriteBatch, graphics);
							}

							for (int i = Bullets.Count - 1; i >= 0; i--) {
								Bullets[i].Draw(gameTime, spriteBatch);
							}

							for (int i = Zombies.Count - 1; i >= 0; i--) {
								Zombies[i].Draw(gameTime, spriteBatch, graphics);
							}

							Player.Draw(gameTime, spriteBatch, graphics);

							for (int i = Particles.Count - 1; i >= 0; i--) {
								Particles[i].Draw(gameTime, spriteBatch);
							}

							// Draw UI elements
							SpriteUtils.DrawImage(spriteBatch, tabTexture, new Vector2(15, 15), Color.White, scale: SpriteUtils.UI_SCALE);
							SpriteUtils.DrawText(spriteBatch, new Vector2(30, 30), $"Currency: {currency}", Color.Black, fontScale: 0.5f);
							SpriteUtils.DrawText(spriteBatch, new Vector2(30, 45), $"Round Number: {roundNumber}", Color.Black, fontScale: 0.5f);
							// SpriteManager.DrawText(_spriteBatch, new Vector2(30, 60), $"Player Health: {player.Health}", Color.Black, fontScale: 0.5f);

							// Draw Debug variables
							if (isInDebugMode) {
								SpriteUtils.DrawText(spriteBatch, new Vector2(10, SCREEN_DIMENSIONS.Y - 20), $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds, 3)}", Color.White, fontScale: 0.5f);
								SpriteUtils.DrawText(spriteBatch, new Vector2(10, SCREEN_DIMENSIONS.Y - 40), $"UPT: {string.Join(" | ", debugTimes)}", Color.White, fontScale: 0.5f);
							}

							// Draw turret charges
							// Archer
							SpriteUtils.DrawImage(spriteBatch, turretArcherBaseTexture, new Vector2(400, 50), new Color(255, 255, 255, 255), scale: SpriteUtils.UI_SCALE - 3);
							SpriteUtils.DrawImage(spriteBatch, turretArcherHeadTexture, new Vector2(400, 50), new Color(255, 255, 255, 255), scale: SpriteUtils.UI_SCALE - 3);
							SpriteUtils.DrawText(spriteBatch, new Vector2(405, 100), turretsPurchased[0].ToString( ), Color.White, fontScale: 1f);
							// Buff
							SpriteUtils.DrawImage(spriteBatch, turretArcherBaseTexture, new Vector2(475, 50), new Color(255, 255, 255, 255), scale: SpriteUtils.UI_SCALE - 3);
							SpriteUtils.DrawImage(spriteBatch, buffTexture, new Vector2(475, 50), new Color(255, 255, 255, 255), scale: SpriteUtils.UI_SCALE - 3);
							SpriteUtils.DrawText(spriteBatch, new Vector2(480, 100), turretsPurchased[1].ToString( ), Color.White, fontScale: 1f);
							// Canon
							SpriteUtils.DrawImage(spriteBatch, turretArcherBaseTexture, new Vector2(550, 50), new Color(255, 255, 255, 255), scale: SpriteUtils.UI_SCALE - 3);
							SpriteUtils.DrawImage(spriteBatch, cannonTexture, new Vector2(550, 50), new Color(255, 255, 255, 255), scale: SpriteUtils.UI_SCALE - 3);
							SpriteUtils.DrawText(spriteBatch, new Vector2(555, 100), turretsPurchased[2].ToString( ), Color.White, fontScale: 1f);

							if (isInShop) {
								SpriteUtils.DrawRect(spriteBatch, graphics, new Rectangle(Point.Zero, SCREEN_DIMENSIONS.ToPoint( )), Color.Black, opacity: 0.5f);

								spriteBatch.DrawString(font, "Shop", new Vector2(graphics.PreferredBackBufferWidth / 2 - 40, 30), Color.White);

								for (int i = 0; i < turretButtonList.Count; i++) {
									turretButtonList[i].Draw(gameTime, spriteBatch);
									spriteBatch.DrawString(font, turretNames[i], new Vector2(turretButtonList[i].Position.X - 75, turretButtonList[i].Position.Y + 75), Color.White);
									spriteBatch.DrawString(font, "X" + turretsPurchased[i].ToString( ), new Vector2(turretButtonList[i].Position.X + 100, turretButtonList[i].Position.Y + 75), Color.Gold);
								}
							}

							if (isPaused) {
								SpriteUtils.DrawRect(spriteBatch, graphics, new Rectangle(Point.Zero, SCREEN_DIMENSIONS.ToPoint( )), Color.Black, opacity: 0.5f);

								SpriteUtils.DrawText(spriteBatch, new Vector2(graphics.PreferredBackBufferWidth / 2 - 50, 30), "Paused", Color.White, isCentered: true);

								pauseResumeButton.Draw(spriteBatch);
								pauseMenuButton.Draw(spriteBatch);
							}

							break;
						case GameState.ShopInPlacment:
							turretInPurchase.Draw(gameTime, spriteBatch);
							break;
					}

					break;
				case MenuState.GameOver:
					spriteBatch.DrawString(font, "Game Over!", new Vector2(graphics.PreferredBackBufferWidth / 2 - 50, graphics.PreferredBackBufferHeight / 5 * 2), Color.DarkRed);
					spriteBatch.DrawString(font, "Press 'Enter' to return to main menu!", new Vector2(graphics.PreferredBackBufferWidth / 2 - 400, graphics.PreferredBackBufferHeight / 5 * 4), Color.White);

					break;
			}

			spriteBatch.End( );

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
		public bool GetKeyDown (Keys key) {
			return (currKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key));
		}

		public bool GetLeftMouseButtonDown ( ) {
			return (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed);
		}

		public void StartNextRound ( ) {
			// Increment the round number
			roundNumber++;

			// Generate zombie stats based on the round number
			// * This means that as the rounds go on, the zombies get harder and harder
			// * Each of these stats follower a quadratic equation that I came up with in 2 minutes so it can definitely be tweated
			int zombieHealth = (int) MathF.Round(0.5f * MathF.Pow(roundNumber, 2) + ZOMBIE_BASE_HEALTH);
			int zombieMoveSpeed = (int) MathF.Round(0.002f * MathF.Pow(roundNumber, 2) + ZOMBIE_BASE_MOVESPEED);
			int zombieAttackSpeed = (int) MathF.Round(0.005f * MathF.Pow(roundNumber, 2) + ZOMBIE_BASE_ATTACKSPEED);
			int zombieCount = (int) MathF.Round(0.3f * MathF.Pow(roundNumber, 2) + ZOMBIE_BASE_COUNT);

			// Spawn in all of the zombies
			for (int i = 0; i < zombieCount; i++) {
				// Generate random spawn position
				int randX = random.Next(1, map.Width - 1);
				int randY = random.Next(1, map.Height - 1);
				Tile tile = null;

				switch (random.Next(0, 4)) {
					case 0:
						tile = map[randX, 1];
						break;
					case 1:
						tile = map[map.Width - 2, randY];
						break;
					case 2:
						tile = map[randX, map.Height - 2];
						break;
					case 3:
						tile = map[1, randY];
						break;
				}

				Zombies.Add(new Zombie(tile.Position, zombieHealth, zombieMoveSpeed, zombieAttackSpeed, tile));
			}
		}

		public void ResetGame ( ) {
			// Reset the round number
			roundNumber = 0;
			currency = 0;

			isPaused = false;
			isInBetweenRounds = false;
			isInShop = false;

			// Clear all of the lists of game objects
			Bullets.Clear( );
			Zombies.Clear( );
			Turrets.Clear( );

			// Create (or re-create) the player
			Player = new Player(playerTexture, SCREEN_DIMENSIONS / 2, 100, 5, 3);

			// Create (or re-create) the camera
			camera = new Camera(Player);
		}
	}
}

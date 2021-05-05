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

		// Input states
		private static KeyboardState currKeyboardState;
		private static KeyboardState prevKeyboardState;
		private static MouseState currMouseState;
		private static MouseState prevMouseState;

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
		private UIButton shopCannonTurretButton;
		private UIButton shopBuffTurretButton;
		private UIButton shopArcherTurretButton;

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

		public static Texture2D turretBaseTexture;
		public static Texture2D turretArcherHeadTexture;
		public static Texture2D turretBuffHeadTexture;
		public static Texture2D turretCannonHeadTexture;
		public static Texture2D mineHeadTexture;

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
		private int cannonTurretsStored;
		private int buffTurretsStored;
		private int archerTurretsStored;

		// Constants
		public const int ZOMBIE_BASE_HEALTH = 100; // The default health of the zombie
		public const int ZOMBIE_BASE_MOVESPEED = 2; // The default movespeed of the zombie
		public const int ZOMBIE_BASE_ATTACKSPEED = 1; // The default attackspeed of the zombie
		public const int ZOMBIE_BASE_COUNT = 5; // The starting number of zombies in round 1
		public const int ZOMBIE_REWARD_MIN = 7;
		public const int ZOMBIE_REWARD_MAX = 13;
		public const float ZOMBIE_SPECIAL_CHANCE = 0.05f;
		public const int ZOMBIE_SPECIAL_MULT = 3;
		public const float DAMAGE_INDIC_TIME = 0.25f; // The amount of seconds that entities flash when they are damaged
		public const int BULLET_SPEED = 15;
		public const int CANNON_BULLET_DAMAGE = 65;
		public const int ARCHER_BULLET_DAMAGE = 40;
		public const int PLAYER_BULLET_DAMAGE = 10;
		public const int HEALTHBAR_OFFSET = 30;
		public const int HEALTHBAR_PADDING = 2;
		public const int CANNON_TURRET_COST = 120;
		public const int BUFF_TURRET_COST = 210;
		public const int ARCHER_TURRET_COST = 75;
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

			easyModeTEST = false;

			random = new Random( );

			base.Initialize( );
		}

		protected override void LoadContent ( ) {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load textures for game objects
			nullTexture = Content.Load<Texture2D>("MapTiles/NullTile");
			turretBaseTexture = Content.Load<Texture2D>("Turrets/TurretCannonBase");
			turretCannonHeadTexture = Content.Load<Texture2D>("Turrets/TurretCannonHead");
			mineHeadTexture = Content.Load<Texture2D>("Turrets/MineHead");
			turretBuffHeadTexture = Content.Load<Texture2D>("Turrets/Buff");
			turretArcherHeadTexture = Content.Load<Texture2D>("Turrets/Canon");
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
				easyModeTEST = false;

				// Start the game
				ResetGame( );
				StartNextRound( );
			});

			menuPlayEasyModeButton = new UIButton("Easy Mode", SCREEN_DIMENSIONS / 2 + new Vector2(0f, 100f), ( ) => {
				menuState = MenuState.Game;
				easyModeTEST = true;

				// Start the game
				ResetGame( );
				StartNextRound( );
			});

			menuQuitButton = new UIButton("Quit", SCREEN_DIMENSIONS / 2 + new Vector2(0f, 200f), ( ) => {
				Exit( );
			});

			pauseResumeButton = new UIButton("Resume", new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 2), ( ) => {
				isPaused = false;
			});

			pauseMenuButton = new UIButton("Menu", new Vector2(SCREEN_DIMENSIONS.X / 2, 2 * (SCREEN_DIMENSIONS.Y / 3)), ( ) => {
				menuState = MenuState.MainMenu;
			});

			// Initializing different turret types for the shop
			shopCannonTurretButton = new UIButton($"Buy Cannon Turret", new Vector2(SCREEN_DIMENSIONS.X / 5, 2 * (SCREEN_DIMENSIONS.Y / 3)), ( ) => {
				if (currency >= CANNON_TURRET_COST) {
					currency -= CANNON_TURRET_COST;
					cannonTurretsStored++;
				}
			}, fontScale: 0.75f);

			shopBuffTurretButton = new UIButton($"Buy Buff Turret", new Vector2(SCREEN_DIMENSIONS.X / 2, 2 * (SCREEN_DIMENSIONS.Y / 3)), ( ) => {
				if (currency >= BUFF_TURRET_COST) {
					currency -= BUFF_TURRET_COST;
					buffTurretsStored++;
				}
			}, fontScale: 0.75f);

			shopArcherTurretButton = new UIButton($"Buy Archer Turret", new Vector2(4 * (SCREEN_DIMENSIONS.X / 5), 2 * (SCREEN_DIMENSIONS.Y / 3)), ( ) => {
				if (currency >= ARCHER_TURRET_COST) {
					currency -= ARCHER_TURRET_COST;
					archerTurretsStored++;
				}
			}, fontScale: 0.75f);

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

						if (cannonTurretsStored > 0 && GetKeyDown(Keys.T)) {
							Turrets.Add(new Turret(TurretType.Cannon, Player.Position, Player));
							cannonTurretsStored--;
						}

						if (buffTurretsStored > 0 && GetKeyDown(Keys.Y)) {
							Turrets.Add(new Turret(TurretType.Buff, Player.Position, Player));
							buffTurretsStored--;
						}

						if (archerTurretsStored > 0 && GetKeyDown(Keys.U)) {
							// Mouse.SetPosition((int) (SCREEN_DIMENSIONS.X / 2), (int) (SCREEN_DIMENSIONS.Y / 2));
							Turrets.Add(new Turret(TurretType.Archer, Player.Position, Player));
							archerTurretsStored--;
						}

						if (GetKeyDown(Keys.Enter)) {
							StartNextRound( );
						}
					}

					if (isInShop) {
						shopCannonTurretButton.Update(gameTime, currMouseState);
						shopBuffTurretButton.Update(gameTime, currMouseState);
						shopArcherTurretButton.Update(gameTime, currMouseState);
					}

					if (isPaused) {
						pauseResumeButton.Update(gameTime, currMouseState);
						pauseMenuButton.Update(gameTime, currMouseState);
					}

					break;
				case MenuState.GameOver:
					if (GetKeyDown(Keys.Enter)) {
						menuState = MenuState.MainMenu;
					}

					pauseMenuButton.Update(gameTime, currMouseState);

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
					SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X - 175, 10), $"Round: {roundNumber}", Color.White, fontScale: 0.75f);
					if (Zombies.Count > 0) {
						SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X - 300, 35), $"Zombies Left: {Zombies.Count}", Color.Red, fontScale: 0.75f);
					} else {
						SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X - 300, 35), $"Zombies Left: {Zombies.Count}", Color.Green, fontScale: 0.75f);
						SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X - 300, 60), "Press 'Enter' To\nStart Next Round", Color.White, fontScale: 0.75f);
					}

					// Draw Debug variables
					if (isInDebugMode) {
						SpriteUtils.DrawText(spriteBatch, new Vector2(10, SCREEN_DIMENSIONS.Y - 30), $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds, 3)}", Color.Blue, fontScale: 0.75f);
						SpriteUtils.DrawText(spriteBatch, new Vector2(10, SCREEN_DIMENSIONS.Y - 55), $"UPT: {string.Join(" | ", debugTimes)}", Color.Blue, fontScale: 0.75f);
					}

					// Draw turret charges
					// Cannon
					SpriteUtils.DrawImage(spriteBatch, turretBaseTexture, new Vector2(2 * (SCREEN_DIMENSIONS.X / 5), 50), Color.White, scale: SpriteUtils.UI_SCALE - 3, isCentered: true);
					SpriteUtils.DrawImage(spriteBatch, turretCannonHeadTexture, new Vector2(2 * (SCREEN_DIMENSIONS.X / 5), 50), Color.White, scale: SpriteUtils.UI_SCALE - 3, isCentered: true);
					SpriteUtils.DrawText(spriteBatch, new Vector2(2 * (SCREEN_DIMENSIONS.X / 5), 75), $"x{cannonTurretsStored} | 'T'", Color.Purple, fontScale: 0.75f, isCentered: true);
					// Buff
					SpriteUtils.DrawImage(spriteBatch, turretBaseTexture, new Vector2(SCREEN_DIMENSIONS.X / 2, 50), Color.White, scale: SpriteUtils.UI_SCALE - 3, isCentered: true);
					SpriteUtils.DrawImage(spriteBatch, turretBuffHeadTexture, new Vector2(SCREEN_DIMENSIONS.X / 2, 50), Color.White, scale: SpriteUtils.UI_SCALE - 3, isCentered: true);
					SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X / 2, 75), $"x{buffTurretsStored} | 'Y'", Color.Purple, fontScale: 0.75f, isCentered: true);
					// Archer
					SpriteUtils.DrawImage(spriteBatch, turretBaseTexture, new Vector2(3 * (SCREEN_DIMENSIONS.X / 5), 50), Color.White, scale: SpriteUtils.UI_SCALE - 3, isCentered: true);
					SpriteUtils.DrawImage(spriteBatch, turretArcherHeadTexture, new Vector2(3 * (SCREEN_DIMENSIONS.X / 5), 50), Color.White, scale: SpriteUtils.UI_SCALE - 3, isCentered: true);
					SpriteUtils.DrawText(spriteBatch, new Vector2(3 * (SCREEN_DIMENSIONS.X / 5), 75), $"x{archerTurretsStored} | 'U'", Color.Purple, fontScale: 0.75f, isCentered: true);

					if (isInShop) {
						SpriteUtils.DrawRect(spriteBatch, graphics, new Rectangle(Point.Zero, SCREEN_DIMENSIONS.ToPoint( )), Color.Black, opacity: 0.5f);

						SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 4), "Shop", Color.Purple, isCentered: true);
						shopCannonTurretButton.Draw(spriteBatch);
						shopBuffTurretButton.Draw(spriteBatch);
						shopArcherTurretButton.Draw(spriteBatch);

						// Cannon
						SpriteUtils.DrawImage(spriteBatch, turretBaseTexture, new Vector2(shopCannonTurretButton.Position.X, SCREEN_DIMENSIONS.Y / 2), Color.White, scale: SpriteUtils.UI_SCALE, isCentered: true);
						SpriteUtils.DrawImage(spriteBatch, turretCannonHeadTexture, new Vector2(shopCannonTurretButton.Position.X, SCREEN_DIMENSIONS.Y / 2), Color.White, scale: SpriteUtils.UI_SCALE, isCentered: true);
						SpriteUtils.DrawText(spriteBatch, new Vector2(shopCannonTurretButton.Position.X, 4 * (SCREEN_DIMENSIONS.Y / 5)), $"${CANNON_TURRET_COST}", Color.Yellow, fontScale: 0.75f, isCentered: true);
						// Buff
						SpriteUtils.DrawImage(spriteBatch, turretBaseTexture, new Vector2(shopBuffTurretButton.Position.X, SCREEN_DIMENSIONS.Y / 2), Color.White, scale: SpriteUtils.UI_SCALE, isCentered: true);
						SpriteUtils.DrawImage(spriteBatch, turretBuffHeadTexture, new Vector2(shopBuffTurretButton.Position.X, SCREEN_DIMENSIONS.Y / 2), Color.White, scale: SpriteUtils.UI_SCALE, isCentered: true);
						SpriteUtils.DrawText(spriteBatch, new Vector2(shopBuffTurretButton.Position.X, 4 * (SCREEN_DIMENSIONS.Y / 5)), $"${BUFF_TURRET_COST}", Color.Yellow, fontScale: 0.75f, isCentered: true);
						// Archer
						SpriteUtils.DrawImage(spriteBatch, turretBaseTexture, new Vector2(shopArcherTurretButton.Position.X, SCREEN_DIMENSIONS.Y / 2), Color.White, scale: SpriteUtils.UI_SCALE, isCentered: true);
						SpriteUtils.DrawImage(spriteBatch, turretArcherHeadTexture, new Vector2(shopArcherTurretButton.Position.X, SCREEN_DIMENSIONS.Y / 2), Color.White, scale: SpriteUtils.UI_SCALE, isCentered: true);
						SpriteUtils.DrawText(spriteBatch, new Vector2(shopArcherTurretButton.Position.X, 4 * (SCREEN_DIMENSIONS.Y / 5)), $"${ARCHER_TURRET_COST}", Color.Yellow, fontScale: 0.75f, isCentered: true);
					}

					SpriteUtils.DrawText(spriteBatch, new Vector2(10, 10), $"${currency}", Color.Yellow, fontScale: 0.75f);

					if (isPaused) {
						SpriteUtils.DrawRect(spriteBatch, graphics, new Rectangle(Point.Zero, SCREEN_DIMENSIONS.ToPoint( )), Color.Black, opacity: 0.5f);

						SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 3), "Paused", Color.White, isCentered: true);
						pauseResumeButton.Draw(spriteBatch);
						pauseMenuButton.Draw(spriteBatch);
					}

					break;
				case MenuState.GameOver:
					SpriteUtils.DrawText(spriteBatch, new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 3), "Game Over! :(", Color.Red, isCentered: true);
					SpriteUtils.DrawText(spriteBatch, SCREEN_DIMENSIONS / 2, $"Round Reached: {roundNumber}", Color.White, isCentered: true);

					pauseMenuButton.Draw(spriteBatch);

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
		public static bool GetKeyDown (Keys key) {
			return (currKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key));
		}

		public static bool GetLeftMouseButtonDown ( ) {
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
			currency = 2000;

			isPaused = false;
			isInBetweenRounds = false;
			isInShop = false;

			cannonTurretsStored = 0;
			buffTurretsStored = 0;
			archerTurretsStored = 0;

			// Clear all of the lists of game objects
			Bullets.Clear( );
			Zombies.Clear( );
			Turrets.Clear( );
			Particles.Clear( );

			// Create (or re-create) the player
			Player = new Player(playerTexture, SCREEN_DIMENSIONS / 2, 100, 5, 3);

			// Create (or re-create) the camera
			camera = new Camera(Player);
		}
	}
}

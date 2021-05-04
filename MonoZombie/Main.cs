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
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		// Game states
		private MenuState menuState;
		private GameState gameState;

		// Input states
		private KeyboardState currKeyboardState;
		private KeyboardState prevKeyboardState;
		private MouseState currMouseState;
		private MouseState prevMouseState;

		//Test variables
		private string currentStateTEST;
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
		public static Texture2D[] grassTextures;
		public static Texture2D[] wallTextures;
		public static Texture2D[] gravelTextures;
		public static Texture2D[] lavaTextures;
		public static Texture2D[] speedTextures;

		public static Texture2D[] zombieTextures;

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

		// Game Objects
		private static Player player;

		// Game Logic Variables
		public static int currency;
		private int roundNumber;

		// Turret
		private List<Turret> turretButtonList;                      // the list that holds all of the turret images
		private List<String> turretNames;                           // holds the names of the turret types, please update
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

		private static Random rng;
		public int playerAttacksPerSecond = 3;

		public static Player Player { get { return player; } }

		public static List<Bullet> ListOfBullets {
			get;
		} = new List<Bullet>();

		public static List<Enemy> ListOfZombies {
			get;
		} = new List<Enemy>();

		public static List<Turret> ListOfTurrets {
			get;
		} = new List<Turret>();

		public static Player GetPlayer {get {return player;} }
		public static MapGraph GetMapGraph { get { return graph; } }

		public Main ( ) {
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize ( ) {
			// TODO: Add your initialization logic here
			menuState = MenuState.MainMenu;
			gameState = GameState.Playing;

			easyModeTEST = false;

			turretButtonList = new List<Turret>( );
			turretNames = new List<String>( );
			turretsPurchased = new List<int>();
			rng = new Random( );

			base.Initialize( );
		}

		protected override void LoadContent ( ) {
			_spriteBatch = new SpriteBatch(GraphicsDevice);

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

			_graphics.PreferredBackBufferWidth = (int) SCREEN_DIMENSIONS.X;
			_graphics.PreferredBackBufferHeight = (int) SCREEN_DIMENSIONS.Y;
			_graphics.ApplyChanges( );

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
				ResetGame();
				StartNextRound();
			});

			menuQuitButton = new UIButton("Quit", SCREEN_DIMENSIONS / 2 + new Vector2(0f, 200f), ( ) => {
				Exit( );
			});

			pauseResumeButton = new UIButton("Resume", new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 3 * 2), ( ) => {
				gameState = GameState.Playing;
			});

			pauseMenuButton = new UIButton("Menu", new Vector2(SCREEN_DIMENSIONS.X / 2, SCREEN_DIMENSIONS.Y / 3), ( ) => {
				menuState = MenuState.MainMenu;
			});


			// Initializing different turret types for the shop

			turretButtonList.Add(
				new Turret(TurretType.Archer, turretArcherBaseTexture, turretArcherHeadTexture, new Vector2(SCREEN_DIMENSIONS.X/7*2, SCREEN_DIMENSIONS.Y/5*2 ))
				);
			turretNames.Add("Archer");
			turretsPurchased.Add(1);

			turretButtonList.Add(
				new Turret(TurretType.Buff, turretArcherBaseTexture, buffTexture, new Vector2(SCREEN_DIMENSIONS.X/7*4, SCREEN_DIMENSIONS.Y/5*2))
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
			// Get the current keyboard state
			currKeyboardState = Keyboard.GetState( );
			currMouseState = Mouse.GetState( );

			switch (menuState) {
				case MenuState.MainMenu:
					currentStateTEST = "MainMenu";

					// Update the menu UI elements
					menuPlayButton.Update(gameTime, currMouseState);
					menuPlayEasyModeButton.Update(gameTime, currMouseState);
					menuQuitButton.Update(gameTime, currMouseState);

					break;
				case MenuState.Game:
					currentStateTEST = "Game -";

					switch (gameState) {
						case GameState.Playing:
							currentStateTEST = "Game - Playing";
							Console.WriteLine();

							//Update the graph's shortest path
							//TODO: call shortest path algorithm
							//Console.WriteLine($"{graph.GetPlayerVertex().TileAtVertex.X} {graph.GetPlayerVertex().TileAtVertex.Y}");
							foreach (Enemy zombie in ListOfZombies)
							{
								Console.WriteLine($"{graph.GetZombieVertex(zombie).TileAtVertex.X} {graph.GetZombieVertex(zombie).TileAtVertex.Y}");
							}
							Console.WriteLine();
							// Update all game objects
							player.Update(gameTime, currMouseState, currKeyboardState);

							for (int i = ListOfTurrets.Count - 1; i >= 0; i--) {
								if (ListOfTurrets[i].Type != TurretType.Buff)
								{									
									ListOfTurrets[i].Update(gameTime, currMouseState, currKeyboardState);
								}
								else
								{
									if (Vector2.Distance(ListOfTurrets[i].Position, player.Position) < ListOfTurrets[i].Range)
                                    {
										player.AttacksPerSecond = 6;
                                    }
									else
										player.AttacksPerSecond = 3;
								}
							}

							for (int i = ListOfBullets.Count - 1; i >= 0; i--) {
								ListOfBullets[i].Update(gameTime, currMouseState, currKeyboardState);
							}

							for (int i = ListOfZombies.Count - 1; i >= 0; i--) {
								ListOfZombies[i].Move(player);

								ListOfZombies[i].Update(gameTime, currMouseState, currKeyboardState);
							}

							// Do game logic calculations

							// Check if the player is dead
							if (player.IsDead && !easyModeTEST) {
								menuState = MenuState.GameOver;
							}

							// If there are no more zombies, then advance to the next round
							if (ListOfZombies.Count == 0) {
								//loop through each turret and check if they are out of time and update their duration
								for (int i = ListOfTurrets.Count - 1; i >= 0; i--)
								{
									ListOfTurrets[i].RoundTimer--;
									if(ListOfTurrets[i].RoundTimer==0)
                                    {
										ListOfTurrets.RemoveAt(i);
                                    }
                                }
								player.AttacksPerSecond = 3;
								gameState = GameState.InBetweenRounds;
							}

							// Check collisions
							// * The reason I think we should do it like this is because each of the game objects
							// have their own custom collisions functions, and in order for each of them to work
							// we need to call them like this
							foreach (GameObject wallTile in map.CollidableMapTiles) {
								// Update the player colliding with the wall
								player.CheckUpdateCollision(wallTile);

								// Update zombies colliding with walls, other zombies, and the player
								foreach (Enemy zombie in ListOfZombies) {
									zombie.CheckUpdateCollision(wallTile);
									zombie.CheckUpdateCollision(player);
									
									//turret collision
									for (int i = ListOfTurrets.Count - 1; i >= 0; i--)
									{
										zombie.CheckUpdateCollision(ListOfTurrets[i]);
										player.CheckUpdateCollision(ListOfTurrets[i]);
										wallTile.CheckUpdateCollision(ListOfTurrets[i]);

									}
									
										for (int i = ListOfZombies.Count - 1; i >= 0; i--)
									{
										// Makes sure the zombie doesn't check itself
										if (zombie != ListOfZombies[i])
										{
											zombie.CheckUpdateCollision(ListOfZombies[i]);
										}
									}
								}

								// Update bullets colliding with walls and zombies
								// * Since bullets and zombies can be destroyed within this loop, we cant do a foreach or there will be an error
								for (int i = ListOfBullets.Count - 1; i >= 0; i--) {
									// If the bullet gets too far from the player, destroy it so it doesn't cause lag
									if (Vector2.Distance(ListOfBullets[i].Position, player.Position) > 1000) {
										ListOfBullets[i].Destroy();
										continue;
									}

									// The bullet collision was successfull with either the wall tile or a zombie, then we dont want to check any more collisions
									// with the current bullet because the object will be destroyed
									if (ListOfBullets[i].CheckCollision(wallTile)) {
										continue;
									}

									for (int j = ListOfZombies.Count - 1; j >= 0; j--) {
										if (ListOfBullets[i].CheckCollision(ListOfZombies[j])) {
											break;
										}
									}
								}
							}

							// Update camera screen positions of all game objects
							player.UpdateCameraScreenPosition(camera);

							map.UpdateCameraScreenPosition(camera);

							for (int i = ListOfTurrets.Count - 1; i >= 0; i--) {
								ListOfTurrets[i].UpdateCameraScreenPosition(camera);
							}

							for (int i = ListOfBullets.Count - 1; i >= 0; i--) {
								ListOfBullets[i].UpdateCameraScreenPosition(camera);
							}

							for (int i = ListOfZombies.Count - 1; i >= 0; i--) {
								ListOfZombies[i].UpdateCameraScreenPosition(camera);
							}

							if (GetKeyDown(Keys.T)) {
								if(turretsPurchased[0] > 0)
								{
									ListOfTurrets.Add(new Turret(TurretType.Archer, turretArcherBaseTexture, turretArcherHeadTexture, player.Position, parent: player));
									--turretsPurchased[0];
								}
							}
							if (GetKeyDown(Keys.Y))
                            {
								if (turretsPurchased[1] > 0)
                                {
									ListOfTurrets.Add(new Turret(TurretType.Buff, turretArcherBaseTexture, buffTexture, player.Position, parent: player));
                                }
                            }
							if (GetKeyDown(Keys.U))
                            {
								if (turretsPurchased[2] > 0)
                                {
									ListOfTurrets.Add(new Turret(TurretType.Cannon, turretArcherBaseTexture, turretArcherHeadTexture, player.Position, parent: player));
									--turretsPurchased[2];
                                }
                            }

							if (GetKeyDown(Keys.Escape)) {
								gameState = GameState.Pause;
							}

							

							break;

						case GameState.InBetweenRounds:

							player.Update(gameTime, currMouseState, currKeyboardState);

							for (int i = ListOfTurrets.Count - 1; i >= 0; i--)
							{
								ListOfTurrets[i].UpdateCameraScreenPosition(camera);
							}

							// Update camera screen positions of all game objects
							player.UpdateCameraScreenPosition(camera);

							map.UpdateCameraScreenPosition(camera);

							if (GetKeyDown(Keys.T))
							{
								if (turretsPurchased[0] > 0)
								{
									ListOfTurrets.Add(new Turret(TurretType.Archer, turretCannonBaseTexture, turretCannonHeadTexture, player.Position, parent: player));
									--turretsPurchased[0];
								}
							}
							if (GetKeyDown(Keys.Y))
							{
								if (turretsPurchased[1] > 0)
								{
									ListOfTurrets.Add(new Turret(TurretType.Buff, turretCannonBaseTexture, turretCannonHeadTexture, player.Position, parent: player));
								}
							}
							if (GetKeyDown(Keys.U))
							{
								if (turretsPurchased[2] > 0)
								{
									ListOfTurrets.Add(new Turret(TurretType.Cannon, turretCannonBaseTexture, turretCannonHeadTexture, player.Position, parent: player));
									--turretsPurchased[2];
								}
							}

							if (GetKeyDown(Keys.Tab))
							{
								gameState = GameState.Shop;
							}

							if(GetKeyDown(Keys.Space))
							{
								StartNextRound();
								gameState = GameState.Playing;
							}
							break;
						case GameState.Pause:
							currentStateTEST = "Game - Pause";

							if (GetKeyDown(Keys.Escape)) {
								gameState = GameState.Playing;
							}

							pauseResumeButton.Update(gameTime, currMouseState);
							pauseMenuButton.Update(gameTime, currMouseState);

							break;
						case GameState.Shop:
							currentStateTEST = "Game - Shop";

							if (GetKeyDown(Keys.Tab)) {
								gameState = GameState.InBetweenRounds;
							}

							for (int i = 0; i < turretButtonList.Count; i++) {
								if (currMouseState.X > turretButtonList[i].Rect.Left && currMouseState.X < turretButtonList[i].Rect.Right
										&& currMouseState.Y <= turretButtonList[i].Rect.Bottom && currMouseState.Y >= turretButtonList[i].Rect.Top)
								{
									Console.WriteLine(turretButtonList[i].Rect.Top + " " + turretButtonList[i].Rect.Bottom);
									if (currMouseState.LeftButton == ButtonState.Pressed)
									{
										if (currency >= 30)
										{
											//turretsPurchased[i]++;
											turretsPurchased[i]++;
											currency -= 30;
											break;
										}
									}
								}
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
			prevMouseState = currMouseState;

			base.Update(gameTime);
		}

		protected override void Draw (GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			// * These settings in SpriteBatch.Begin() prevent sprites from becoming blurry when scaled up. This
			// means we can make pixel art images and import them into the game very small and then scale them up.
			// This makes the images a lot easier to edit if we need to do that again.
			_spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

			switch (menuState) {
				case MenuState.MainMenu:
					// Draw menu UI objects
					SpriteManager.DrawImage(_spriteBatch, titleTexture, SCREEN_DIMENSIONS * new Vector2(0.5f, 0.25f), Color.White, scale: SpriteManager.UI_SCALE, isCentered: true);
					menuPlayButton.Draw(_spriteBatch);
					menuPlayEasyModeButton.Draw(_spriteBatch);
					menuQuitButton.Draw(_spriteBatch);

					break;
				case MenuState.Game:
					switch (gameState) {
						case GameState.Playing:
							// Draw all game objects
							map.Draw(gameTime, _spriteBatch);

							for (int i = ListOfTurrets.Count - 1; i >= 0; i--) {
								ListOfTurrets[i].Draw(gameTime, _spriteBatch);
							}

							for (int i = ListOfBullets.Count - 1; i >= 0; i--) {
								ListOfBullets[i].Draw(gameTime, _spriteBatch);
							}

							for (int i = ListOfZombies.Count - 1; i >= 0; i--) {
								ListOfZombies[i].Draw(gameTime, _spriteBatch);
							}

							player.Draw(gameTime, _spriteBatch);

							// Draw UI elements
							SpriteManager.DrawImage(_spriteBatch, tabTexture, new Vector2(15, 15), Color.White, scale: SpriteManager.UI_SCALE);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 30), $"Currency: {currency}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 45), $"Round Number: {roundNumber}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 60), $"Player Health: {player.Health}", Color.Black, fontScale: 0.5f);

							// Draw FPS counter
							SpriteManager.DrawText(_spriteBatch, new Vector2(10, SCREEN_DIMENSIONS.Y - 20), $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}", Color.Black, fontScale: 0.5f);

							// Draw turret charges
		// Archer
							SpriteManager.DrawImage(_spriteBatch, turretArcherBaseTexture, new Vector2(400, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawImage(_spriteBatch, turretArcherHeadTexture, new Vector2(400, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawText(_spriteBatch, new Vector2(405, 100), turretsPurchased[0].ToString(), Color.White, fontScale: 1f);
		// Buff
							SpriteManager.DrawImage(_spriteBatch, turretArcherBaseTexture, new Vector2(475, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawImage(_spriteBatch, buffTexture, new Vector2(475, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawText(_spriteBatch, new Vector2(480, 100), turretsPurchased[1].ToString(), Color.White, fontScale: 1f);
		// Canon
							SpriteManager.DrawImage(_spriteBatch, turretArcherBaseTexture, new Vector2(550, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawImage(_spriteBatch, cannonTexture, new Vector2(550, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawText(_spriteBatch, new Vector2(555, 100), turretsPurchased[2].ToString(), Color.White, fontScale: 1f);

							break;
						case GameState.InBetweenRounds:
							// Draw all game objects
							map.Draw(gameTime, _spriteBatch);

							for (int i = ListOfTurrets.Count - 1; i >= 0; i--)
							{
								ListOfTurrets[i].Draw(gameTime, _spriteBatch);
							}

							player.Draw(gameTime, _spriteBatch);

							// Draw UI elements
							SpriteManager.DrawImage(_spriteBatch, tabTexture, new Vector2(15, 15), Color.White, scale: SpriteManager.UI_SCALE);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 30), $"Currency: {currency}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 45), $"Round Number: {roundNumber}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 60), $"Player Health: {player.Health}", Color.Black, fontScale: 0.5f);

							// Draw FPS counter
							SpriteManager.DrawText(_spriteBatch, new Vector2(10, SCREEN_DIMENSIONS.Y - 20), $"FPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}", Color.Black, fontScale: 0.5f);

							// Draw turret charges
		// Archer
							SpriteManager.DrawImage(_spriteBatch, turretCannonBaseTexture, new Vector2(400, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawImage(_spriteBatch, turretCannonHeadTexture, new Vector2(400, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawText(_spriteBatch, new Vector2(405, 100), turretsPurchased[0].ToString(), Color.White, fontScale: 1f);
		// Buff
							SpriteManager.DrawImage(_spriteBatch, turretCannonBaseTexture, new Vector2(475, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawImage(_spriteBatch, turretCannonHeadTexture, new Vector2(475, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawText(_spriteBatch, new Vector2(480, 100), turretsPurchased[1].ToString(), Color.White, fontScale: 1f);
		// Canon
							SpriteManager.DrawImage(_spriteBatch, turretCannonBaseTexture, new Vector2(550, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawImage(_spriteBatch, turretCannonHeadTexture, new Vector2(550, 50), new Color(255, 255, 255, 255), scale: SpriteManager.UI_SCALE - 3);
							SpriteManager.DrawText(_spriteBatch, new Vector2(555, 100), turretsPurchased[2].ToString(), Color.White, fontScale: 1f);
							break;
						case GameState.Pause:
							DrawPauseMenu();
							break;
						case GameState.Shop:
							DrawShop(gameTime);
							break;
						case GameState.ShopInPlacment:
							turretInPurchase.Draw(gameTime, _spriteBatch);
							break;
					}

					break;
				case MenuState.GameOver:
					_spriteBatch.DrawString(font, "Game Over!", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, _graphics.PreferredBackBufferHeight / 5 * 2), Color.DarkRed);
					_spriteBatch.DrawString(font, "Press 'Enter' to return to main menu!", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 400, _graphics.PreferredBackBufferHeight / 5 * 4), Color.White);

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
		public bool GetKeyDown (Keys key) {
			return (currKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key));
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
			if(zombieCount>=15)
            {
				zombieCount = 15;
            }

			// Spawn in all of the zombies
			for (int i = 0; i < zombieCount; i++) {
				// Generate random spawn position
				int randX = rng.Next(1, map.Width - 1);
				int randY = rng.Next(1, map.Height - 1);
				Tile tile = null;

				switch (rng.Next(0, 4)) {
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

				ListOfZombies.Add(new Enemy(tile.Position, zombieHealth, zombieMoveSpeed, zombieAttackSpeed, parent: tile));


			}
		}

		public void ResetGame ( ) {
			// Reset the round number
			roundNumber = 0;
			currency = 0;

			// Clear all of the lists of game objects
			ListOfBullets.Clear( );
			ListOfZombies.Clear( );
			ListOfTurrets.Clear( );

			// Create (or re-create) the player
			player = new Player(playerTexture, SCREEN_DIMENSIONS / 2, 100, 5, playerAttacksPerSecond);

			// Create (or re-create) the camera
			camera = new Camera(player);
		}

		private void DrawPauseMenu ( ) {
			// _spriteBatch.Draw(new Rectangle(0,0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.LightBlue);
			_spriteBatch.DrawString(font, "Paused", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, 30), Color.White);

			pauseResumeButton.Draw(_spriteBatch);
			pauseMenuButton.Draw(_spriteBatch);

		}

		private void DrawShop (GameTime gameTime) {
			_spriteBatch.DrawString(font, "Shop", new Vector2(_graphics.PreferredBackBufferWidth / 2 - 40, 30), Color.White);

			for (int i = 0; i < turretButtonList.Count; i++) {
				turretButtonList[i].Draw(gameTime, _spriteBatch);
				_spriteBatch.DrawString(font, turretNames[i], new Vector2(turretButtonList[i].X-75, turretButtonList[i].Y + 75), Color.White);
				_spriteBatch.DrawString(font, "X"+turretsPurchased[i].ToString(), new Vector2(turretButtonList[i].X + 100, turretButtonList[i].Y + 75), Color.Gold);
			}
		}
	}
}

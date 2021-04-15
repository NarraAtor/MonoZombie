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
	public class Main : Game {
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private MenuState menuState;
		private GameState gameState;

		private KeyboardState currKeyboardState;
		private KeyboardState prevKeyboardState;
		private MouseState currMouseState;

		//Test variables
		private string currentStateTEST;

		public static Vector2 ScreenDimensions = new Vector2(1280, 720);
		private Camera camera;

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
		public static Texture2D[ ] gravelTextures;
		public static Texture2D[] lavaTextures;
		public static Texture2D[] speedTextures;

		private Map map;

		private static Texture2D turretImage;
		private static Texture2D baseImage;
		public static Texture2D playerImage;
		private static Texture2D enemyImage;
		private static Texture2D bulletImage;
		private static List<Turret> listOfTurrets;
		private static List<Enemy> listOfZombies;
		private static List<Bullet> listOfBullets;
		private static Turret turret;
		private static Player player;
		private static Enemy zombie;
		private static int currency;
		private static int roundNumber;
		private static bool roundIsOngoing;
		private static bool aZombieIsAlive;
		private static bool aBulletIsInactive;

		public static Player Player {
			get {
				return player;
			}
		}

		public static List<Bullet> ListOfBullets
		{
			get
			{
				return listOfBullets;
			}
			set
			{
				listOfBullets = value;
			}
		}

		public static List<Enemy> ListOfZombies
		{
			get
			{
				return listOfZombies;
			}
			set
			{
				listOfZombies = value;
			}
		}

		public Main ( ) {
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            menuState = MenuState.MainMenu;
            gameState = GameState.Playing;
            currency = 0;
            roundNumber = 0;
            listOfZombies = new List<Enemy>();
            listOfTurrets = new List<Turret>();
			listOfBullets = new List<Bullet>();
            aZombieIsAlive = false;
			aBulletIsInactive = false;

			base.Initialize( );
		}

		protected override void LoadContent ( ) {
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// Load textures for game objects
			baseImage = Content.Load<Texture2D>("TurretBase");
			turretImage = Content.Load<Texture2D>("TurretHead");
			playerImage = Content.Load<Texture2D>("playerproto");
			enemyImage = Content.Load<Texture2D>("zombieproto");
			bulletImage = Content.Load<Texture2D>("bullet");

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

			gravelTextures = new Texture2D[]
			{
				Content.Load<Texture2D>("Gravel")
			};

			lavaTextures = new Texture2D[]
			{
				Content.Load<Texture2D>("Lava")
			};

			speedTextures = new Texture2D[]
			{
				Content.Load<Texture2D>("Speed")
			};

			// Load fonts
			font = Content.Load<SpriteFont>("5Pixel");

			// Load UI Textures
			titleTexture = Content.Load<Texture2D>("title");
			buttonTexture = Content.Load<Texture2D>("button");
			tabTexture = Content.Load<Texture2D>("tab");

			// Load the map
			map = new Map("../../../MapLevels\\CurrentMapDesign.level");

			_graphics.PreferredBackBufferWidth = (int) ScreenDimensions.X;
			_graphics.PreferredBackBufferHeight = (int) ScreenDimensions.Y;
			_graphics.ApplyChanges( );

			// Texture-reliant intitialization
			turret = new Turret(TurretType.Archer, baseImage, turretImage, new Vector2(100, 100));
			player = new Player(playerImage, ScreenDimensions / 2, 10, 5, 3);
			zombie = new Enemy(enemyImage, new Vector2((_graphics.PreferredBackBufferWidth / 2) + 30, _graphics.PreferredBackBufferHeight / 2), 100, 1, 5);

			// Create the camera
			camera = new Camera(player);

			// Create UI Buttons
			menuPlayButton = new UIButton("Play", ScreenDimensions / 2, ( ) => {
				menuState = MenuState.Game;
				gameState = GameState.Playing;
				roundIsOngoing = true;
			});

			// test zombie list
			listOfZombies.Add(zombie);

			//test turret list
			listOfTurrets.Add(turret);

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

							//Check if the player is dead
							//if(player.Health <= 0)
							//{
							//	menuState = MenuState.GameOver;
							//}

							aBulletIsInactive = false;

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
                                    turret.UpdateTurret(zombie, bulletImage, gameTime);
                                }
                            }

							if (!aZombieIsAlive) {
								roundIsOngoing = false;
								roundNumber++;
								//allowaccess to shop
								//run shop methods when opened
							}

							if (currMouseState.LeftButton == ButtonState.Pressed) {
								player.Shoot(bulletImage, currMouseState, gameTime);
							}

							foreach (Bullet bullet in listOfBullets) {
								bullet.Move( );
							}

							// Update the player
							player.Update(gameTime, currMouseState, currKeyboardState);

							// Update the map
							map.Update(gameTime, currMouseState, currKeyboardState);

							// Check gameobject collisions
							map.CheckUpdateCollision(player);
							// check zombie-map collisions
							// check zombie-player collisions
							// check bullet-zombie collisions
							foreach(Enemy zombie in listOfZombies)
							{
								foreach(Bullet bullet in ListOfBullets)
								{
									//If the bullet is colliding with a zombie and hasn't already hit one.
									if(bullet.CheckUpdateCollision(zombie) && bullet.IsActive)
									{
										zombie.TakeDamage(10);
										bullet.IsActive = false;
										aBulletIsInactive = true;
									}
								}
							}

							if(aBulletIsInactive)
							{
								//Delete inactive bullets by creating a new list without the the inactive bullets.
								List<Bullet> newBulletList = new List<Bullet>();
								for (int i = 0; i < listOfBullets.Count; i++)
								{
									if (listOfBullets[i].IsActive)
									{
										newBulletList.Add(listOfBullets[i]);
									}
								}
								listOfBullets = newBulletList;
							}
							

							// Update the camera screen positions of the game objects
							player.UpdateCameraScreenPosition(camera);
							map.UpdateCameraScreenPosition(camera);

							foreach(Enemy zombie in listOfZombies)
							{
								zombie.UpdateCameraScreenPosition(camera);
							}

							foreach(Turret turret in listOfTurrets)
							{
								turret.UpdateCameraScreenPosition(camera);
							}

							//Bullets spawn in the center of the screen for some reason when I use UpdateCameraScreenPosition
							//foreach (Bullet bullet in listOfBullets)
							//{
							//	bullet.UpdateCameraScreenPosition(camera);
							//}

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
					SpriteManager.DrawImage(_spriteBatch, titleTexture, ScreenDimensions * new Vector2(0.5f, 0.25f), scale: SpriteManager.UIScale, isCentered: true);
					menuPlayButton.Draw(_spriteBatch);

					break;
				case MenuState.Game:
					switch (gameState) {
						case GameState.Playing:
							// Draw the map
							map.Draw(_spriteBatch);

							// Draw the player
							player.Draw(_spriteBatch);

							
							turret.Draw(_spriteBatch, Color.White);

                            foreach(Enemy zombie in listOfZombies)
                            {
                                zombie.Draw(_spriteBatch);
                            }

                            foreach (Turret turret in listOfTurrets)
                            {
                                turret.Draw(_spriteBatch, Color.White);
                            }
							

							// Draw the bullets
							foreach (Bullet bullet in listOfBullets)
							{
								bullet.Draw(_spriteBatch);
							}

							player.Draw(_spriteBatch);
							

							// Draw UI elements
							SpriteManager.DrawImage(_spriteBatch, tabTexture, new Vector2(15, 15), scale: SpriteManager.UIScale);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 30), $"Currency: {currency}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 45), $"Round Number: {roundNumber}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 60), $"Player Health: {player.Health}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 75), $"Zombie Timer: {zombie.Timer}", Color.Black, fontScale: 0.5f);
							SpriteManager.DrawText(_spriteBatch, new Vector2(30, 90), $"Zombie Health: {zombie.Health}", Color.Black, fontScale: 0.5f);

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

		/*
		 * Author : Frank Alfano
		 * 
		 * Get the distance (in pixels) between 2 points
		 * 
		 * Point point1				: The first point
		 * Point point2				: The second point
		 * 
		 * return double			: The distance (in pixels) between the two points
		 */
		public static float Distance (Vector2 point1, Vector2 point2) {
			return MathF.Sqrt(MathF.Pow(point1.X - point2.X, 2) + MathF.Pow(point1.Y - point2.Y, 2));
		}
	}
}

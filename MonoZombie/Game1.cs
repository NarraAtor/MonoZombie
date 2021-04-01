using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZombie
{
    public enum MenuState
    {
        MainMenu,
        Game,
        GameOver
    }

    public enum GameState
    {
        Playing,
        Pause,
        Shop
    }

    /// <summary>
    /// Author: Eric Fotang
    /// Purpose: Manages game states and calls other classes and methods to do their job. 
    /// Restrictions:
    /// </summary>
    public class Game1 : Game
    {
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

        //Properties for WallTile
        public static Texture2D GrassProperty1 { get; set; }
        public static Texture2D GrassProperty2 { get; set; }
        public static Texture2D GrassProperty3 { get; set; }
        public static Texture2D WallProperty1 { get; set; }
        public static Texture2D WallProperty2 { get; set; }
        public static Texture2D WallProperty3 { get; set; }

        private static Texture2D turretImage;
        private static Texture2D baseImage;
        private static Texture2D playerImage;
        private static Texture2D enemyImage;
        private static List<WallTile> listOfTiles;
        private static List<Enemy> listOfZombies;
        private static List<Turret> listOfTurrets;
        private static Turret turret;
        private static Player player;
        private static Enemy zombie;
        private static int currency;
        private static int roundNumber;
        private static bool roundIsOngoing;
        private static bool aZombieIsAlive;

        public static Player Player { get { return player; } }

        //Adjustment Variables
        private int tileWidth;

        public Game1()
        {
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
            listOfTiles = new List<WallTile>();
            listOfZombies = new List<Enemy>();
            listOfTurrets = new List<Turret>();
            aZombieIsAlive = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            baseImage = Content.Load<Texture2D>("TurretBase");
            turretImage = Content.Load<Texture2D>("TurretHead");
            playerImage = Content.Load<Texture2D>("playerproto");
            enemyImage = Content.Load<Texture2D>("zombieproto");
            GrassProperty1 = Content.Load<Texture2D>("GrassTile1");
            GrassProperty2 = Content.Load<Texture2D>("GrassTile2");
            GrassProperty3 = Content.Load<Texture2D>("GrassTile3");
            WallProperty1 = Content.Load<Texture2D>("WallTile1");
            WallProperty2 = Content.Load<Texture2D>("WallTile2");
            WallProperty3 = Content.Load<Texture2D>("WallTile3");

            // Load fonts
            font = Content.Load<SpriteFont>("5Pixel");

            // Load UI Textures
            titleTexture = Content.Load<Texture2D>("title");
            buttonTexture = Content.Load<Texture2D>("button");
            tabTexture = Content.Load<Texture2D>("tab");

            //Texture reliant intitialization
            turret = new Turret(TurretType.Archer, baseImage, turretImage, _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            player = new Player(100, 100, playerImage, _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 3);
            zombie = new Enemy(enemyImage, (_graphics.PreferredBackBufferWidth / 2) + 30, _graphics.PreferredBackBufferHeight / 2, 100, 1, 5);

            //test zombie list
            listOfZombies.Add(zombie);
            //test turret list
            listOfTurrets.Add(turret);

            //Load the map;
            StreamReader reader = new StreamReader("../../../MapLevels\\CurrentMapDesign.level");

            string currentLine;
            //Used for quickly changing the map's scale;
            tileWidth = 45;

            //Get the dimensions
            currentLine = reader.ReadLine( );
            string[ ] mapDimensionStrings = currentLine.Split("|");
            int[ ] mapDimensions = new int[ ] { int.Parse(mapDimensionStrings[0]), int.Parse(mapDimensionStrings[1]) };

            // Update the dimensions of the screen
            screenDimensions = new Vector2(mapDimensions[0] * tileWidth, mapDimensions[1] * tileWidth);
            
            _graphics.PreferredBackBufferWidth = (int) screenDimensions.X;
            _graphics.PreferredBackBufferHeight = (int) screenDimensions.Y;
            _graphics.ApplyChanges( );

            // Update the scale of the UI
            UIElement.UIScale = 5;

            // Create UI Buttons
            menuPlayButton = new UIButton("Play", screenDimensions / 2, () => {
                menuState = MenuState.Game;
                gameState = GameState.Playing;
                roundIsOngoing = true;
            }, true);

            int xPosition = 0;
            int yPosition = 0;
            while ((currentLine = reader.ReadLine( )) != null) {
                switch (currentLine) {
                    case "Grass":
                        listOfTiles.Add(new WallTile(Tile.Grass,
                            (_graphics.PreferredBackBufferWidth / mapDimensions[0]) * xPosition,
                            (_graphics.PreferredBackBufferHeight / mapDimensions[1]) * yPosition,
                            tileWidth,
                            tileWidth
                            ));
                        break;

                    case "Wall":
                        listOfTiles.Add(new WallTile(Tile.Wall,
                            (_graphics.PreferredBackBufferWidth / mapDimensions[0]) * xPosition,
                            (_graphics.PreferredBackBufferHeight / mapDimensions[1]) * yPosition,
                            tileWidth,
                            tileWidth
                            ));
                        break;
                }

                //The map editor saves files by column,left to right.
                if (yPosition == (mapDimensions[1] - 1))
                {
                    yPosition = 0;
                    xPosition++;
                }
                else
                {
                    yPosition++;
                }


            }

            reader.Close( );



            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Get the current keyboard state
            currKeyboardState = Keyboard.GetState();
            currMouseState = Mouse.GetState( );

            switch (menuState)
            {
                case MenuState.MainMenu:
                    currentStateTEST = "MainMenu";

                    // Update the menu UI elements
                    menuPlayButton.Update(gameTime, Mouse.GetState( ));

                    break;
                case MenuState.Game:
                    currentStateTEST = "Game -";
                    switch (gameState)
                    {
                        case GameState.Playing:
                            currentStateTEST = "Game - Playing";
                            //This code rewards the player when a zombie is killed and makes the round end when in contact with a zombie.
                            aZombieIsAlive = false;

                            foreach (Enemy zombie in listOfZombies)
                            {
                                //If a zombie just died, set indicate that it is dead an increment currency.
                                if (zombie.Health <= 0 && zombie.IsAlive)
                                {
                                    zombie.Die();
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

                            if (aZombieIsAlive!)
                            {
                                roundIsOngoing = false;
                            }

                            foreach(WallTile tile in listOfTiles)
                            {
                                tile.Update();
                            }

                            player.Update(gameTime, currMouseState, currKeyboardState);

                            if (GetKeyDown(Keys.Escape))
                            {
                                gameState = GameState.Pause;
                            }

                            if (GetKeyDown(Keys.Tab))
                            {
                                gameState = GameState.Shop;
                            }
                            break;
                        case GameState.Pause:
                            currentStateTEST = "Game - Pause";
                            if (GetKeyDown(Keys.Escape))
                            {
                                gameState = GameState.Playing;
                            }
                            break;
                        case GameState.Shop:
                            currentStateTEST = "Game - Shop";
                            if (GetKeyDown(Keys.Tab))
                            {
                                gameState = GameState.Playing;
                            }
                            break;
                    }
                    break;

                case MenuState.GameOver:
                    currentStateTEST = "GameOver";
                    //Single press bool so that you don't switch states twice.
                    if (GetKeyDown(Keys.Enter))
                    {
                        menuState = MenuState.MainMenu;
                    }
                    break;
            }

            prevKeyboardState = currKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // * These settings in SpriteBatch.Begin() prevent sprites from becoming blurry when scaled up. This
            // means we can make pixel art images and import them into the game very small and then scale them up.
            // This makes the images a lot easier to edit if we need to do that again.
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            switch (menuState)
            {
                case MenuState.MainMenu:
                    // Draw menu UI objects
                    UIElement.DrawImage(_spriteBatch, titleTexture, screenDimensions * new Vector2(0.5f, 0.25f), true);
                    menuPlayButton.Draw(_spriteBatch);

                    break;
                case MenuState.Game:
                    switch (gameState)
                    {
                        case GameState.Playing:
                            foreach (WallTile tile in listOfTiles)
                            {
                                tile.Draw(_spriteBatch, Color.White);
                            }

                            foreach(Enemy zombie in listOfZombies)
                            {
                                zombie.Draw(_spriteBatch);
                            }

                            foreach (Turret turret in listOfTurrets)
                            {
                                turret.Draw(_spriteBatch, Color.White);
                            }

                            player.Draw(_spriteBatch);

                            // Draw UI elements
                            UIElement.DrawImage(_spriteBatch, tabTexture, new Vector2(15, 15), false);
                            UIElement.DrawText(_spriteBatch, 0.5f, $"Currency: {currency}", Color.Black, new Vector2(30, 30), false);
                            UIElement.DrawText(_spriteBatch, 0.5f, $"Round Number: {roundNumber}", Color.Black, new Vector2(30, 45), false);
                            UIElement.DrawText(_spriteBatch, 0.5f, $"Player Health: {player.Health}", Color.Black, new Vector2(30, 60), false);

                            //Test, draw zombie current health
                            //UIElement.DrawText(_spriteBatch, 0.5f, $"Zombie Health: {zombie.Health}", Color.Black, new Vector2(30, 90), false);

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

            //Being used to test if states are switching properly.
            _spriteBatch.DrawString(font, currentStateTEST, new Vector2(100, 100), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    
        private bool GetKeyDown (Keys key) {
            return (currKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key));
		}
    }
}

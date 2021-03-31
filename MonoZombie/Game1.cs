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
        private KeyboardState ks;
        private KeyboardState previousks;

        //Test variables
        private string currentStateTEST;
        // Fonts
        private SpriteFont dogicaPixel;
        private SpriteFont dogicaPixelBold;

        private static Vector2 screenDimensions;

        // UI Variables
        private UIImage menuTitle;
        private UIButton menuPlayButton;
        private UIButton menuQuitButton;

        private UIImage gameUITab;
        // private UIProgressBar gameHealthBar;

        // SHOP UI OBJECTS (TO BE DETERMINED)

        private UIText pausePausedText;
        private UIButton pauseResumeButton;
        private UIButton pauseMenuButton;

        private UIImage gameOverImage;
        private UIText gameOverScore;
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

            //Texture reliant intitialization
            turret = new Turret(TurretType.Archer, baseImage, turretImage, 100, 100);
            player = new Player(100, 100, playerImage, _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 3);
            zombie = new Enemy(enemyImage, (_graphics.PreferredBackBufferWidth / 2) + 30, _graphics.PreferredBackBufferHeight / 2, 100, 1, 5);

            // Load fonts
            dogicaPixel = Content.Load<SpriteFont>("DogicaPixel");
            dogicaPixelBold = Content.Load<SpriteFont>("DogicaPixelBold");

            // Load UI Textures
            Texture2D menuTitleTexture = Content.Load<Texture2D>("title");
            Texture2D buttonTexture = Content.Load<Texture2D>("button");
            Texture2D tabTexture = Content.Load<Texture2D>("tab");

            // Update the scale of the UI
            UIElement.UIScale = 5;

            // Create UI
            menuTitle = new UIImage(menuTitleTexture, new Point((int)screenDimensions.X / 2, (int)screenDimensions.Y / 4));
            menuPlayButton = UIElement.CreateButton(buttonTexture, new Point((int)screenDimensions.X / 2, (int)screenDimensions.Y / 2), () => {
                menuState = MenuState.Game;
                gameState = GameState.Playing;
                roundIsOngoing = true;
            }, dogicaPixel, "Play");
            // menuQuitButton

            // private UIImage gameUITab;
            // private UIProgressBar gameHealthBar;

            // SHOP UI OBJECTS (TO BE DETERMINED)

            // pausePausedText
            // pauseResumeButton
            // pauseMenuButton

            // private UIImage gameOverImage;
            // gameOverScore
            // gameOverMenuButton


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

            //test zombie list
            // listOfZombies.Add(zombie);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here
            ks = Keyboard.GetState();
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
                            player.Move(ks);

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
                            }

                            if (aZombieIsAlive!)
                            {
                                roundIsOngoing = false;
                            }

                            foreach(WallTile tile in listOfTiles)
                            {
                                tile.Update();
                            }

                            player.Update(gameTime, Mouse.GetState(), ks);
                            //Single press bool so that you don't switch states twice.
                            if (ks.IsKeyDown(Keys.Escape) && !previousks.IsKeyDown(Keys.Escape))
                            {
                                gameState = GameState.Pause;
                            }

                            //Single press bool so that you don't switch states twice.
                            if (ks.IsKeyDown(Keys.Tab) && !previousks.IsKeyDown(Keys.Tab))
                            {
                                gameState = GameState.Shop;
                            }
                            break;
                        case GameState.Pause:
                            currentStateTEST = "Game - Pause";
                            //Single press bool so that you don't switch states twice.
                            if (ks.IsKeyDown(Keys.Escape) && !previousks.IsKeyDown(Keys.Escape))
                            {
                                gameState = GameState.Playing;
                            }
                            break;
                        case GameState.Shop:
                            currentStateTEST = "Game - Shop";
                            //Single press bool so that you don't switch states twice.
                            if (ks.IsKeyDown(Keys.Tab) && !previousks.IsKeyDown(Keys.Tab))
                            {
                                gameState = GameState.Playing;
                            }
                            break;
                    }
                    break;

                case MenuState.GameOver:
                    currentStateTEST = "GameOver";
                    //Single press bool so that you don't switch states twice.
                    if (ks.IsKeyDown(Keys.Enter) && !previousks.IsKeyDown(Keys.Enter))
                    {
                        menuState = MenuState.MainMenu;
                    }
                    break;
            }

            previousks = ks;

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
                    menuTitle.Draw(_spriteBatch);
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

                            turret.Draw(_spriteBatch, Color.White);
                            player.Draw(_spriteBatch);


                            _spriteBatch.DrawString(dogicaPixel, $"Currency: {currency}", new Vector2(10, 10), Color.White);
                            _spriteBatch.DrawString(dogicaPixel, $"Round Number: {roundNumber}", new Vector2(10, 30), Color.White);
                            _spriteBatch.DrawString(dogicaPixel, $"Player Health: {player.Health}", new Vector2(10, 50), Color.White);
                            _spriteBatch.DrawString(dogicaPixel, $"Zombie Timer: {zombie.Timer}", new Vector2(10, 90), Color.White);

                            //_spriteBatch.DrawString(spriteFontTEST, $"Game Timer: {gameTime.ElapsedGameTime.TotalMilliseconds}", new Vector2(10, 70), Color.White);
                            _spriteBatch.DrawString(dogicaPixel, $"Currency: {currency}", new Vector2(10, 10), Color.White);
                            _spriteBatch.DrawString(dogicaPixel, $"Round Number: {roundNumber}", new Vector2(10, 30), Color.White);
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
            _spriteBatch.DrawString(dogicaPixel, currentStateTEST, new Vector2(100, 100), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

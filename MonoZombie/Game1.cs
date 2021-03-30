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
        private SpriteFont spriteFontTEST;
        private string currentStateTEST;

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

        protected override void Initialize()
        {
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
            spriteFontTEST = Content.Load<SpriteFont>("File");
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


            //Load the map;
            StreamReader reader = new StreamReader("../../../MapLevels\\CurrentMapDesign.level");

            string currentLine;
            //Used for quickly changing the map's scale;
            tileWidth = 45;

            //Get the dimensions
            currentLine = reader.ReadLine();
            string[] mapDimensionStrings = currentLine.Split("|");
            int[] mapDimensions = new int[] { int.Parse(mapDimensionStrings[0]), int.Parse(mapDimensionStrings[1]) };
            _graphics.PreferredBackBufferWidth = mapDimensions[0] * tileWidth;
            _graphics.PreferredBackBufferHeight = mapDimensions[1] * tileWidth;
            _graphics.ApplyChanges();

            int xPosition = 0;
            int yPosition = 0;
            while ((currentLine = reader.ReadLine()) != null)
            {
                switch (currentLine)
                {
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

            reader.Close();

            //Texture reliant intitialization
            turret = new Turret(TurretType.Archer, baseImage, turretImage, 100, 100);
            player = new Player(100, 100, playerImage, _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 3);
            zombie = new Enemy(enemyImage, (_graphics.PreferredBackBufferWidth / 2) + 30, _graphics.PreferredBackBufferHeight / 2, 100, 1, 5);

            //test zombie list
            listOfZombies.Add(zombie);
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
                    //Single press bool so that you don't switch states twice.
                    if (ks.IsKeyDown(Keys.Enter) && !previousks.IsKeyDown(Keys.Enter))
                    {
                        menuState = MenuState.Game;
                        gameState = GameState.Playing;
                        roundIsOngoing = true;
                    }
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

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (menuState)
            {
                case MenuState.MainMenu:
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


                            _spriteBatch.DrawString(spriteFontTEST, $"Currency: {currency}", new Vector2(10, 10), Color.White);
                            _spriteBatch.DrawString(spriteFontTEST, $"Round Number: {roundNumber}", new Vector2(10, 30), Color.White);
                            _spriteBatch.DrawString(spriteFontTEST, $"Player Health: {player.Health}", new Vector2(10, 50), Color.White);
                            _spriteBatch.DrawString(spriteFontTEST, $"Zombie Timer: {zombie.Timer}", new Vector2(10, 90), Color.White);
                            //_spriteBatch.DrawString(spriteFontTEST, $"Game Timer: {gameTime.ElapsedGameTime.TotalMilliseconds}", new Vector2(10, 70), Color.White);
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
            _spriteBatch.DrawString(spriteFontTEST, currentStateTEST, new Vector2(100, 100), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

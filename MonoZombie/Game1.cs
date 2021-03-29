using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;

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

        //Properties for WallTile
        public static Texture2D GrassProperty1 { get; set; }
        public static Texture2D GrassProperty2 { get; set; }
        public static Texture2D GrassProperty3 { get; set; }
        public static Texture2D WallProperty1 { get; set; }
        public static Texture2D WallProperty2 { get; set; }
        public static Texture2D WallProperty3 { get; set; }

        private Texture2D turretImage;
        private Texture2D baseImage;
        private Texture2D playerImage;
        private Texture2D enemyImage;
        private List<WallTile> listOfTiles;
        private Turret turret;
        private Player player;
        private int currency;
        private int roundNumber;
        private bool roundIsOngoing;

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


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            dogicaPixel = Content.Load<SpriteFont>("DogicaPixel");
            dogicaPixelBold = Content.Load<SpriteFont>("DogicaPixelBold");

            baseImage = Content.Load<Texture2D>("TurretBase");
            turretImage = Content.Load<Texture2D>("TurretHead");
            playerImage = Content.Load<Texture2D>("playerproto");
            GrassProperty1 = Content.Load<Texture2D>("GrassTile1");
            GrassProperty2 = Content.Load<Texture2D>("GrassTile2");
            GrassProperty3 = Content.Load<Texture2D>("GrassTile3");
            WallProperty1 = Content.Load<Texture2D>("WallTile1");
            WallProperty2 = Content.Load<Texture2D>("WallTile2");
            WallProperty3 = Content.Load<Texture2D>("WallTile3");
            turret = new Turret(TurretType.Archer, baseImage, turretImage, 100, 100);
            player = new Player(100, 100, playerImage, 150, 150, 3);


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
                if (yPosition % mapDimensions[1] == 0 && yPosition != 0)
                {
                    yPosition = 0;
                    xPosition++;
                }

                yPosition++;
            }

            reader.Close();

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

                            //planned code for determining whether or not to end a round and rewarding the player for killing zombies.
                            bool aZombieIsAlive = false; //(flag)


                            //foreach (Enemy zombie in zombieList)
                            //{
                            //    //If a zombie just died, set indicate that it is dead an increment currency.
                            //    if(zombie.Health <= 0 && zombie.IsAlive == true)
                            //    {
                            //        zombie.IsAlive = false;
                            //        currency++;
                            //    }
                            //
                            //    
                            //    if (zombie.IsAlive)
                            //    {
                            //        aZombieIsAlive = true;
                            //    }
                            //}
                            //
                            //if (aZombieIsAlive!)
                            //{
                            //    roundIsOngoing = false;
                            //}



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
                            turret.Draw(_spriteBatch, Color.White);
                            player.Draw(_spriteBatch);

                            foreach (WallTile tile in listOfTiles)
                            {
                                tile.Draw(_spriteBatch, Color.White);
                            }

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

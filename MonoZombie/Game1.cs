using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private Texture2D turretImage;
        private Texture2D baseImage;
        private Texture2D playerImage;
        private Texture2D enemyImage;
        private Turret turret;
        private Player player;
        private int currency;
        private int roundNumber;
        private bool zombiesAreSpawning;
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
            turret = new Turret(TurretType.Archer, baseImage, turretImage, 100, 100);
            player = new Player(100, 100, playerImage, 150, 150, 3);
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
                        zombiesAreSpawning = true;
                    }
                    break;

                case MenuState.Game:
                    currentStateTEST = "Game -";
                    switch (gameState)
                    {
                        case GameState.Playing:
                            currentStateTEST = "Game - Playing";
                            player.Move(ks);

                            //planned code for determining whether or not to end a round.
                            //bool zombieIsAlive = false; (flag)
                            //
                            //foreach(Enemy zombie in zombieList)
                            //        {
                            //         
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
                            _spriteBatch.DrawString(spriteFontTEST, $"Currency: {currency}", new Vector2(10, 10), Color.White);
                            _spriteBatch.DrawString(spriteFontTEST, $"Round Number: {roundNumber}", new Vector2(10, 30), Color.White);
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

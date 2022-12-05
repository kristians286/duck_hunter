using DuckHunterGame.src.controllers;
using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace DuckHunterGame.src
{
    public class MainWindow : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private DHGame game;
        private DHGameController gameController = new();
        private DuckController duckController = new();
        private DogController dogController = new();

        private Texture2D duckHitBox;
        private Texture2D dogHitBox;

        private float _timer = 5;

        // MOUSE
        private MouseState mouseState;
        private MouseState prevMouseState;

        // Sprite code
        private SpriteFont font;
        private Vector2 duckPosition;
        private Vector2 dogPosition;
        private (float, float) mousePos;

        public MainWindow()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            game = gameController.NewGame();
            
            _graphics.PreferredBackBufferWidth = game.screenWidth;
            _graphics.PreferredBackBufferHeight = game.screenHeight;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("File");
            // TODO: use this.Content to load your game content here
            duckHitBox = new Texture2D(GraphicsDevice, 1, 1);
            duckHitBox.SetData(new[] { Color.White });

            dogHitBox = new Texture2D(GraphicsDevice, 1, 1);
            dogHitBox.SetData(new[] { Color.White });
        }

        protected override void UnloadContent() // WILL BE REMOVED
        {
            base.UnloadContent();
            _spriteBatch.Dispose();
            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            duckHitBox.Dispose();
            dogHitBox.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            if (!dogController.IsVisable(gameController.GetDog(game)))
            {

                dogPosition = new Vector2(game.dog.posX, game.dog.posY);
                dogController.Move(gameController.GetDog(game), 64*3, delta);
            } else { 
            

            
            if (!gameController.GetCanShoot(game)) {
                mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    mousePos = (mouseState.X, mouseState.Y);
                    gameController.Shoot(game, mouseState.X, mouseState.Y);
                }
            }

            if ( !(game.ducks[game.currentDuck].isFlyAway ^ game.ducks[game.currentDuck].isHit) )
            {
                duckController.Fly(game.ducks[game.currentDuck], delta);
            } else
            {
                duckController.Leave(game.ducks[game.currentDuck], delta);
            }

            prevMouseState = mouseState;
            
            duckPosition = new Vector2(game.ducks[game.currentDuck].posX, game.ducks[game.currentDuck].posY);

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();


            _spriteBatch.DrawString(font, "X:" + game.ducks[game.currentDuck].posX +
                                        "\nY:" + game.ducks[game.currentDuck].posY +
                                        "\nmPos:" + mousePos +
                                        "\ndogX:" + game.dog.posX +
                                        "\ndogY:" + game.dog.posY, new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(font, "Bullets: " + gameController.GetBullets(game) , new Vector2(0, 64*7), Color.Black);
            
            
            _spriteBatch.Draw(duckHitBox, duckPosition, null,
                                Color.Red, 0f, new Vector2(0,0), new Vector2(64f, 64f),
                                SpriteEffects.None, 0f);
            _spriteBatch.Draw(dogHitBox, dogPosition, null,
                                Color.Orange, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
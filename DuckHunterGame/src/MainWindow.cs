﻿using DuckHunterGame.src.controllers;
using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
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

        private Texture2D background;

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

            // TODO: use this.Content to load your game content here

            background = Content.Load<Texture2D>("background");

            font = Content.Load<SpriteFont>("File");
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

            if (dogController.IsVisable(gameController.GetDog(game))) 
            {
                if (gameController.GetIsIntro(game)){ 
                    dogController.Walk(gameController.GetDog(game), 64*3, delta);
                } else
                {
                    gameController.AnimateDog(game, delta); 
                }

            } else {

                if (!gameController.GetCanShoot(game)) {
                    mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        mousePos = (mouseState.X, mouseState.Y); // DELETE LATER
                        gameController.Shoot(game, mouseState.X, mouseState.Y);
                    }
                }

                if ( !(gameController.GetCurrentDuck(game).isFlyAway ^ gameController.GetCurrentDuck(game).isHit) )
                {
                    duckController.Fly(gameController.GetCurrentDuck(game), delta);
                } else
                {
                    gameController.DuckLeave(game, delta);
                }

                prevMouseState = mouseState;
                }

            dogPosition = new Vector2(game.dog.posX, game.dog.posY);
            duckPosition = new Vector2(gameController.GetCurrentDuck(game).posX, gameController.GetCurrentDuck(game).posY);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(duckHitBox, duckPosition, null,
                                Color.Red, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                SpriteEffects.None, 0f);

            if (dogController.GetIsInBackground(gameController.GetDog(game)))
            {
                _spriteBatch.Draw(dogHitBox, dogPosition, null,
                                Color.Orange, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                SpriteEffects.None, 0f);
                _spriteBatch.Draw(background, new Rectangle(0, 32, game.screenHeight, game.screenHeight), Color.White);
            } else
            {
                _spriteBatch.Draw(background, new Rectangle(0, 32, game.screenHeight, game.screenHeight), Color.White);
                _spriteBatch.Draw(dogHitBox, dogPosition, null,
                                    Color.Orange, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                    SpriteEffects.None, 0f);
            }
            

            _spriteBatch.DrawString(font, "X:" + game.ducks[game.currentDuck].posX +
                            "\nY:" + game.ducks[game.currentDuck].posY +
                            "\nGamePoints:" + gameController.GetPoints(game) +
                            "\nmPos:" + mousePos + "\n" +gameController.GetDog(game).isInBackround+
                            "\ndogX:" + game.dog.posX +
                            "\ndogY:" + game.dog.posY, new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(font, "Bullets: " + gameController.GetBullets(game) +
                                        "\nround: " + gameController.GetRound(game)
                , new Vector2(0, 64 * 7), Color.Black);

            _spriteBatch.DrawString(font, "Dog: " + gameController.GetDog(game).animDuration
                + gameController.GetDog(game).isVisable + gameController.GetDog(game).enumDogAnimState
                , new Vector2(0, 64 * 6), Color.Black);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
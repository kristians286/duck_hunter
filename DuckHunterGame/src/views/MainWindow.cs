using DuckHunterGame.src.controllers;
using DuckHunterGame.src.enums;
using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DuckHunterGame.src.views
{
    public class MainWindow : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private models.Game _game;

        private GameController _gameController = new GameController();
        private DuckController _duckController = new DuckController();
        private DogController _dogController = new DogController();

        private Texture2D duckHitBox;
        private Texture2D dogHitBox;

        // MOUSE
        private MouseState mouseState;
        private MouseState prevMouseState;

        // Sprite code
        private SpriteFont font;
        private Vector2 duckPosition;
        private Vector2 dogPosition;
        private (float, float) mousePos;

        private Texture2D background;
        private Rectangle coolerDuckPosition;
        private Texture2D duckSprite;

        public MainWindow()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _game = _gameController.NewGame();

            _graphics.PreferredBackBufferWidth = _game.screenWidth;
            _graphics.PreferredBackBufferHeight = _game.screenHeight;
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

            duckSprite = Content.Load<Texture2D>("ducks");
            background = Content.Load<Texture2D>("background");

            font = Content.Load<SpriteFont>("TextFont");

            duckHitBox = new Texture2D(GraphicsDevice, 1, 1);
            duckHitBox.SetData(new[] { Color.White });

            dogHitBox = new Texture2D(GraphicsDevice, 1, 1);
            dogHitBox.SetData(new[] { Color.White });
        }

        protected override void UnloadContent() // WILL BE REMOVED only used to draw Hitboxes
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

            if (_dogController.IsVisible(_gameController.GetDog(_game))) // DOG
            {
                if (_gameController.GetIsIntro(_game))
                {
                    if (_dogController.GetAnimState(_game.dog) == EnumDogAnimState.WALK)
                    {
                        _dogController.Walk(_game.dog, 64 * 2, delta);
                    }
                    else if (_dogController.GetAnimState(_game.dog) == EnumDogAnimState.SNIFF)
                    {
                        _dogController.Sniff(_game.dog, delta);
                    }
                    else if (_dogController.GetAnimState(_game.dog) == EnumDogAnimState.JUMP)
                    {
                        _dogController.JumpInBush(_game.dog, 64 * 2, delta);
                    }
                }
                else
                {
                    _gameController.DogReaction(_game, delta);
                }

            }
            else
            { // DUCK

                if (!_gameController.GetCanShoot(_game))
                {
                    mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        mousePos = (mouseState.X, mouseState.Y); // DELETE LATER
                        _gameController.Shoot(_game, mouseState.X, mouseState.Y);
                    }
                }

                if (!(_gameController.GetCurrentDuck(_game).isFlyAway ^ _gameController.GetCurrentDuck(_game).isHit))
                {
                    _duckController.Fly(_game.ducks[_game.currentDuck], delta);
                }
                else
                {
                    _gameController.DuckLeave(_game, delta);
                }
                
                prevMouseState = mouseState;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // POSITIONS
            dogPosition = new Vector2(_game.dog.posX, _game.dog.posY);
            duckPosition = new Vector2(_game.ducks[_game.currentDuck].posX, _game.ducks[_game.currentDuck].posY);
            coolerDuckPosition = new Rectangle((int)_game.ducks[_game.currentDuck].posX, (int)_game.ducks[_game.currentDuck].posY, _game.ducks[_game.currentDuck].height, _game.ducks[_game.currentDuck].width);

            // TODO: Add your drawing code here  //IGNORE THIS CHAOS FOR NOW THIS IS TESTING. RIGHT ?
            _spriteBatch.Begin();

            _spriteBatch.Draw(duckHitBox, duckPosition, null,
                                Color.Red, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                SpriteEffects.None, 0f);
            /*
            if (_gameController.GetCurrentDuck(_game).flyDirHorizontal)
            {
                _spriteBatch.Draw(duckSprite, coolerDuckPosition, new Rectangle(36 * _gameController.GetCurrentDuck(_game).frame, 0 + 36 * (int)_gameController.GetCurrentDuck(_game).enumDuckType,
                                                                                36, 36), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.0f); //FLIGHT TEST
            }
            else
            {
                _spriteBatch.Draw(duckSprite, coolerDuckPosition, new Rectangle(36 * _gameController.GetCurrentDuck(_game).frame, 0 + 36 * (int)_gameController.GetCurrentDuck(_game).enumDuckType,
                                                                                _game.ducks[_game.currentDuck].height, _game.ducks[_game.currentDuck].width), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f); //FLIGHT TEST
            }
            */

            if (_dogController.GetIsInBackground(_gameController.GetDog(_game)))
            {
                _spriteBatch.Draw(dogHitBox, dogPosition, null,
                                Color.Orange, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                SpriteEffects.None, 0f);
                _spriteBatch.Draw(background, new Rectangle(0, 32, _game.screenHeight, _game.screenHeight), Color.White);
            }
            else
            {
                _spriteBatch.Draw(background, new Rectangle(0, 32, _game.screenHeight, _game.screenHeight), Color.White);
                _spriteBatch.Draw(dogHitBox, dogPosition, null,
                                    Color.Orange, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                    SpriteEffects.None, 0f);
            }


            _spriteBatch.DrawString(font, "X:" + _game.ducks[_game.currentDuck].posX +
                            "\nY:" + _game.ducks[_game.currentDuck].posY +
                            "\nGamePoints:" + _gameController.GetPoints(_game) +
                            "\nmPos:" + mousePos + "\n" + _gameController.GetDog(_game).isInBackround +
                            "\nB_FRAME:" + _gameController.GetCurrentDuck(_game) +
                            "\ndogY:" + _game.dog.posY, new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(font, "Bullets: " + _gameController.GetBullets(_game) +
                                        "\nround: " + _gameController.GetRound(_game)
                , new Vector2(0, 64 * 5), Color.Black);

            _spriteBatch.DrawString(font, "Dog: " + _gameController.GetDog(_game).animDuration
                + _gameController.GetDog(_game).isVisable + _gameController.GetDog(_game).enumDogAnimState
                , new Vector2(0, 64 * 6), Color.Black);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
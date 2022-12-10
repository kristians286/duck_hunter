using DuckHunterGame.src.controllers;
using DuckHunterGame.src.enums;
using DuckHunterGame.src.models;
using DuckHunterGame.src.serializer;
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

        private GameSerializer _serializer = new GameSerializer();

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
        
        private Texture2D duckSprite;
        private Texture2D dogSprite;

        private Rectangle _DuckPosition;
        private Rectangle _DogPosition;

        private Texture2D _blackBirdSprite;
        private Texture2D _blueBirdSprite;
        private Texture2D _redBirdSprite;

        private Dictionary<EnumDuckType, Texture2D> _currentDuckType = new Dictionary<EnumDuckType, Texture2D>();
        //private AnimatedTexture 

        private Dictionary<EnumDuckState, AnimationController> _spriteDuckStates = new Dictionary<EnumDuckState, AnimationController>();
        private Dictionary<EnumDogState, AnimationController> _spriteDogStates = new Dictionary<EnumDogState, AnimationController>();
        public MainWindow()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _game = _gameController.NewGame();

            _graphics.PreferredBackBufferWidth = _game.screenWidth;
            _graphics.PreferredBackBufferHeight = _game.screenHeight;
            _graphics.ApplyChanges();

            //_spiteDuckPosition = new Dictionary<EnumDuckAnimState, Rectangle>();
            _spriteDuckStates.Add(EnumDuckState.FLY_LEFT, new AnimationController   (36, 36, 0, 36*3, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.FLY_RIGHT, new AnimationController  (36, 36, 1, 36*3, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.HIT, new AnimationController        (36, 36, 2, 36, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.FALL, new AnimationController       (36, 36, 3, 36*2, 0.2f));
            _spriteDuckStates.Add(EnumDuckState.FLY_UP, new AnimationController     (36, 36, 0, 36*3, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.IDLE, new AnimationController       (1, 1, 1, 1, 1));

            _spriteDogStates.Add(EnumDogState.WALK, new AnimationController     (48, 56, 0, 56 * 4, 0.15f));
            _spriteDogStates.Add(EnumDogState.SNIFF, new AnimationController    (48, 56, 1, 56 * 3, 0.6f));
            _spriteDogStates.Add(EnumDogState.JUMP, new AnimationController     (48, 56, 2, 56 * 2, 0.7f));
            _spriteDogStates.Add(EnumDogState.SHOW_DUCK, new AnimationController(48, 56, 3, 56, 0.2f));
            _spriteDogStates.Add(EnumDogState.LAUGH, new AnimationController    (48, 56, 4, 56 * 2 , 0.15f));
            _spriteDogStates.Add(EnumDogState.IDLE, new AnimationController     (1, 1, 1, 1, 1));

            
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
            _blackBirdSprite = Content.Load<Texture2D>("blackBird");
            _blueBirdSprite = Content.Load<Texture2D>("blueBird");
            _redBirdSprite = Content.Load<Texture2D>("redBird");
            _currentDuckType.Add(EnumDuckType.BLACK_DUCK, _blackBirdSprite);
            _currentDuckType.Add(EnumDuckType.BLUE_DUCK, _blueBirdSprite);
            _currentDuckType.Add(EnumDuckType.RED_DUCK, _redBirdSprite);

            dogSprite = Content.Load<Texture2D>("dog");

            duckSprite = Content.Load<Texture2D>("ducks");
            background = Content.Load<Texture2D>("background");
            font = Content.Load<SpriteFont>("TextFont");

            duckHitBox = new Texture2D(GraphicsDevice, 1, 1); // Will be deleted
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
                    if (_dogController.GetAnimState(_game.dog) == EnumDogState.WALK)
                    {
                        _dogController.Walk(_game.dog, 64 * 2, delta);
                    }
                    else if (_dogController.GetAnimState(_game.dog) == EnumDogState.SNIFF)
                    {
                        _dogController.Sniff(_game.dog, delta);
                    }
                    else if (_dogController.GetAnimState(_game.dog) == EnumDogState.JUMP)
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
                        //_serializer.SaveGame(_game);  // Save Game 
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

            _spriteDuckStates[_game.ducks[_game.currentDuck].enumDuckAnimState].UpdateFrame(delta);
            _spriteDogStates[_game.dog.enumDogAnimState].UpdateFrame(delta);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // POSITIONS
            dogPosition = new Vector2(_game.dog.posX, _game.dog.posY);
            duckPosition = new Vector2(_game.ducks[_game.currentDuck].posX, _game.ducks[_game.currentDuck].posY);
            _DuckPosition = new Rectangle((int)_game.ducks[_game.currentDuck].posX, (int)_game.ducks[_game.currentDuck].posY, _game.ducks[_game.currentDuck].height, _game.ducks[_game.currentDuck].width);
            _DogPosition = new Rectangle((int)_game.dog.posX, (int)_game.dog.posY, 48 *2, 56 *2);
            // TODO: Add your drawing code here  //IGNORE THIS CHAOS FOR NOW THIS IS TESTING. RIGHT ?
            _spriteBatch.Begin();
            /*
            _spriteBatch.Draw(duckHitBox, duckPosition, null,
                                Color.Red, 0f, new Vector2(0, 0), new Vector2(64f, 64f),
                                SpriteEffects.None, 0f);
            */
            if (_gameController.GetCurrentDuck(_game).flyDirHorizontal)
            {
                _spriteBatch.Draw(_currentDuckType[_game.ducks[_game.currentDuck].enumDuckType], _DuckPosition, _spriteDuckStates[_game.ducks[_game.currentDuck].enumDuckAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.0f); //FLIGHT TEST
            }
            else
            {
                _spriteBatch.Draw(_currentDuckType[_game.ducks[_game.currentDuck].enumDuckType], _DuckPosition, _spriteDuckStates[_game.ducks[_game.currentDuck].enumDuckAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f); //FLIGHT TEST
            }
            

            if (_dogController.GetIsInBackground(_gameController.GetDog(_game)))
            {
                //_spriteBatch.Draw(dogHitBox, dogPosition, null,Color.Red, 0f, new Vector2(0, 0), new Vector2(64f, 64f),SpriteEffects.None, 0f);
                _spriteBatch.Draw(dogSprite, _DogPosition, _spriteDogStates[_game.dog.enumDogAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.0f);
                
                _spriteBatch.Draw(background, new Rectangle(0, 32, _game.screenHeight, _game.screenHeight), Color.White);
            }
            else
            {
                _spriteBatch.Draw(background, new Rectangle(0, 32, _game.screenHeight, _game.screenHeight), Color.White);
                //_spriteBatch.Draw(dogHitBox, dogPosition, null,Color.Orange, 0f, new Vector2(0, 0), new Vector2(64f, 64f),SpriteEffects.None, 0f);
                _spriteBatch.Draw(dogSprite, _DogPosition, _spriteDogStates[_game.dog.enumDogAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.0f);

            }


            _spriteBatch.DrawString(font, "X:" + _game.ducks[_game.currentDuck].posX +
                            "\nY:" + _game.ducks[_game.currentDuck].posY +
                            "\nGamePoints:" + _gameController.GetPoints(_game) +
                            "\nmPos:" + mousePos + "\n" + _gameController.GetDog(_game).isInBackround +
                            "\nDT:" + _gameController.GetCurrentDuck(_game).enumDuckType +
                            "\ndogY:" + _game.dog.posY, new Vector2(0, 0), Color.Black);
            _spriteBatch.DrawString(font, "Bullets: " + _gameController.GetBullets(_game) +
                                        "\nround: " + _gameController.GetRound(_game) +
                                        "\n" + _spriteDuckStates[_game.ducks[_game.currentDuck].enumDuckAnimState].print()
                , new Vector2(0, 64 * 5), Color.Black);

            _spriteBatch.DrawString(font, "Dog: " + _gameController.GetDog(_game).animDuration
                + _gameController.GetDog(_game).isVisable + _gameController.GetDog(_game).enumDogAnimState
                , new Vector2(0, 64 * 6), Color.Black);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
using DuckHunter.Models;
using DuckHunter.Models.Enums;
using DuckHunter.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Game = DuckHunter.Models.Game;

namespace DuckHunterGame.src.views
{
    public class MainWindow : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Game _game;

        // CONTROLLERS
        //private GameController _gameController = new GameController();
        //private DuckController _duckController = new DuckController();
        //private DogController _dogController = new DogController();

        private GameSerializer _serializer = new GameSerializer();
        
        // VIEWS
        private HUD _hudView;

        // MOUSE
        private MouseState mouseState;
        private MouseState prevMouseState;

        // SPRITES
        private Texture2D _background;
        private Texture2D _dogSprite;
        private Texture2D _blackBirdSprite;
        private Texture2D _blueBirdSprite;
        private Texture2D _redBirdSprite;
        private Rectangle _DuckPosition;
        private Rectangle _DogPosition;

        private Texture2D _hudElements;

        private SpriteFont _textFont;

        private Dictionary<EnumDuckType, Texture2D> _currentDuckType = new Dictionary<EnumDuckType, Texture2D>();
        private Dictionary<EnumDuckState, AnimationController> _spriteDuckStates = new Dictionary<EnumDuckState, AnimationController>();
        private Dictionary<EnumDogState, AnimationController> _spriteDogStates = new Dictionary<EnumDogState, AnimationController>();

        private EnumDogState _prevEnumState;

        // BUTTONS
        private List<ComponentButton> _componentButtons;
        
        private Texture2D _buttonTexture;


        /// <bugs>
        /// 
        /// Clicking outside of game window still uses a shot
        /// 
        /// Clicking a button will shoot one bullet 
        /// 
        /// </bugs>

        public MainWindow()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _game = GameController.NewGame();

            _graphics.PreferredBackBufferWidth = _game.screenWidth;
            _graphics.PreferredBackBufferHeight = _game.screenHeight;
            _graphics.ApplyChanges();

            _spriteDuckStates.Add(EnumDuckState.FLY_LEFT, new AnimationController   (36, 36, 0, 36*3, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.FLY_RIGHT, new AnimationController  (36, 36, 1, 36*3, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.HIT, new AnimationController        (36, 36, 2, 36, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.FALL, new AnimationController       (36, 36, 3, 36*2, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.FLY_UP, new AnimationController     (36, 36, 0, 36*3, 0.15f));
            _spriteDuckStates.Add(EnumDuckState.IDLE, new AnimationController       (1, 1, 1, 1, 1));

            _spriteDogStates.Add(EnumDogState.WALK, new AnimationController     (48, 56, 0, 56 * 4, 0.15f));
            _spriteDogStates.Add(EnumDogState.SNIFF, new AnimationController    (48, 56, 1, 56 * 3, 0.5f));
            _spriteDogStates.Add(EnumDogState.JUMP, new AnimationController     (48, 56, 2, 56 * 2, 0.5f));
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

            _dogSprite = Content.Load<Texture2D>("dog");

            _background = Content.Load<Texture2D>("background");

            _hudElements = Content.Load<Texture2D>("hudElements");

            _textFont = Content.Load<SpriteFont>("TextFont");

            _hudView = new HUD(_game, _hudElements, _spriteBatch);

            _buttonTexture = Content.Load<Texture2D>("Button");

            var saveButton = new ComponentButton(_buttonTexture, _textFont) 
            { 
                Position = new Vector2(30,20),
                Text = "Save"
            };
            saveButton.ClickEvent += (object sender, System.EventArgs e) =>
            {
                _serializer.SaveGame(_game);
            };

            var loadButton = new ComponentButton(_buttonTexture, _textFont)
            {
                Position = new Vector2(30 + 160, 20),
                Text = "Load"
            };
            loadButton.ClickEvent += (object sender, System.EventArgs e) =>
            {
                _game = _serializer.LoadGame();
                _hudView = new HUD(_game, _hudElements, _spriteBatch);
            };

            var newGameButton = new ComponentButton(_buttonTexture, _textFont)
            {
                Position = new Vector2(30 + 320, 20),
                Text = "New Game"
            };
            newGameButton.ClickEvent += (object sender, System.EventArgs e) =>
            {
                GameController.RestartGame(_game);
                
            };

            _componentButtons = new List<ComponentButton>
            {
                saveButton,
                loadButton,
                newGameButton,
            };
        }

        protected override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (_prevEnumState != _game.dog.enumDogAnimState)
            {
                foreach (var spriteDogState in _spriteDogStates)
                    if (_prevEnumState == spriteDogState.Key)
                    {
                        Debug.WriteLine("Restore: "+ spriteDogState.Key);
                        spriteDogState.Value.RestoreToFirstFrame();
                    }
            }
            _prevEnumState = _game.dog.enumDogAnimState;
            
            


            foreach (var component in _componentButtons)
                component.Update(gameTime);


            if (DogController.IsVisible(GameController.GetDog(_game))) // DOG
            {
                if (GameController.GetIsIntro(_game))
                {
                    if (DogController.GetAnimState(_game.dog) == EnumDogState.WALK)
                    {
                        DogController.Walk(_game.dog, _game.screenWidth/2 - 64 -32, delta);

                    }
                    else if (DogController.GetAnimState(_game.dog) == EnumDogState.SNIFF)
                    {
                        DogController.Sniff(_game.dog, delta);
                    }
                    else if (DogController.GetAnimState(_game.dog) == EnumDogState.JUMP)
                    {
                        DogController.JumpInBush(_game.dog, _game.screenWidth/2 - 64, delta);
                    }
                }
                else
                {
                    GameController.DogReaction(_game, delta);
                }

            }
            else // DUCK
            {
                
                if (!GameController.GetCanShoot(_game))
                {
                    mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        GameController.Shoot(_game, mouseState.X, mouseState.Y);
                    }
                }

                if (!(GameController.GetCurrentDuck(_game).isFlyAway ^ GameController.GetCurrentDuck(_game).isHit))
                {
                    DuckController.Fly(_game.Ducks[_game.currentDuck], delta);
                }
                else
                {
                    GameController.DuckLeave(_game, delta);
                }
                
                prevMouseState = mouseState;
            }

            _spriteDuckStates[_game.Ducks[_game.currentDuck].enumDuckAnimState].UpdateFrame(delta);
            _spriteDogStates[_game.dog.enumDogAnimState].UpdateFrame(delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_game.Ducks[_game.currentDuck].isFlyAway)
            {
                GraphicsDevice.Clear(Color.Pink);
            } else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }

            // POSITIONS
            _DuckPosition = new Rectangle((int)_game.Ducks[_game.currentDuck].posX, (int)_game.Ducks[_game.currentDuck].posY, 64, 64);
            _DogPosition = new Rectangle((int)_game.dog.posX, (int)_game.dog.posY, 48 *2, 56 *2);
            // TODO: Add your drawing code here 
            _spriteBatch.Begin();

            if (_game.Ducks[_game.currentDuck].enumDuckAnimState == EnumDuckState.FALL)
            {
                _spriteBatch.DrawString(_textFont, _game.Ducks[_game.currentDuck].points.ToString(), new Vector2(mouseState.X - _game.Ducks[_game.currentDuck].points.ToString().Length * 4, mouseState.Y), Color.White);
            }

            if (GameController.GetCurrentDuck(_game).flyDirHorizontal)
            {
                _spriteBatch.Draw(_currentDuckType[_game.Ducks[_game.currentDuck].enumDuckType], _DuckPosition, _spriteDuckStates[_game.Ducks[_game.currentDuck].enumDuckAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.0f); //FLIGHT TEST
            }
            else
            {
                _spriteBatch.Draw(_currentDuckType[_game.Ducks[_game.currentDuck].enumDuckType], _DuckPosition, _spriteDuckStates[_game.Ducks[_game.currentDuck].enumDuckAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0.0f); //FLIGHT TEST
            }
            


            if (DogController.GetIsInBackground(GameController.GetDog(_game)))
            {
                _spriteBatch.Draw(_dogSprite, _DogPosition, _spriteDogStates[_game.dog.enumDogAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.0f);
                _spriteBatch.Draw(_background, new Rectangle(0, 32, _game.screenHeight, _game.screenHeight), Color.White);
            }
            else
            {
                _spriteBatch.Draw(_background, new Rectangle(0, 32, _game.screenHeight, _game.screenHeight), Color.White);
                _spriteBatch.Draw(_dogSprite, _DogPosition, _spriteDogStates[_game.dog.enumDogAnimState].GetFrame(), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.0f);

            }

            var test = new DuckHunter.Models.Dog();


            _hudView.Draw(gameTime);

            foreach (var component in _componentButtons)
                component.Draw(gameTime,_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
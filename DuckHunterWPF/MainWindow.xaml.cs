using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Configuration;
using DuckHunter.Models;
using DuckHunter.Controllers;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Input;

using DuckHunter.Models.Enums;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace DuckHunterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTime = new DispatcherTimer();

        private Game _game;
        private GameController _gameController = new GameController();
        private DogController _dogController = new DogController();
        private DuckController _duckController = new DuckController();

        private GameSerializer _gameSerializer = new GameSerializer();
        
        private ImageBrush _background = new ImageBrush();
        private ImageBrush _duckSprite = new ImageBrush();
        private ImageBrush _dogSprite = new ImageBrush();
        private ImageBrush _hudElements = new ImageBrush();

        private Rectangle _dogRect = new Rectangle();
        private Rectangle _duckRect = new Rectangle();

        private DateTime _current;
        private DateTime _previous;

        private MouseButtonState mouseState;
        private MouseButtonState prevMouseState;


        public MainWindow()
        {
            InitializeComponent();
            Cursor = Cursors.None;

            _game = _gameController.NewGame();

            gameTime.Interval = TimeSpan.FromMilliseconds(16.6666666667);
            Debug.WriteLine(gameTime.Interval.ToString());
            gameTime.Tick += GameTick;
            gameTime.Start();


            _background.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.png"));

            _hudElements.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/hudElements.png"));

            _dogSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/dog.png"));
            _dogSprite.Viewbox = new Rect(0,0,0.25,.2);
            _duckSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/blackBird.png"));
            _duckSprite.Viewbox = new Rect(0, 0, 0.33, 0.25);
            
            this.DataContext = _game;
            findMyBackground();
            findMyDog();
            findMyDuck();
            

        }

        private void GameTick(object? sender, EventArgs e)
        {
            var mp = Mouse.GetPosition(this);
            Canvas.SetLeft(Crosshair, mp.X - Crosshair.ActualWidth / 2);
            Canvas.SetTop(Crosshair, mp.Y - Crosshair.ActualHeight / 2);


            _current = DateTime.Now;
            float delta = (float) (_current.Millisecond - _previous.Millisecond) / 1000;
            if (delta < 0)
            {
                delta = 0.01f;
            }
            //Debug.WriteLine(delta);
            _previous = _current;
            
            if (_dogController.IsVisible(_gameController.GetDog(_game))) // DOG
            {
                if (_gameController.GetIsIntro(_game))
                {
                    if (_dogController.GetAnimState(_game.dog) == EnumDogState.WALK)
                    {
                        _dogController.Walk(_game.dog, _game.screenWidth / 2 - 64 - 32, delta);
                            
                    }
                    else if (_dogController.GetAnimState(_game.dog) == EnumDogState.SNIFF)
                    {
                        _dogController.Sniff(_game.dog, delta);
                    }
                    else if (_dogController.GetAnimState(_game.dog) == EnumDogState.JUMP)
                    {
                        _dogController.JumpInBush(_game.dog, _game.screenWidth / 2 - 64, delta);
                    }
                }
                else
                {
                    _gameController.DogReaction(_game, delta);
                }

            }
            else // DUCK
            {

                if (!_gameController.GetCanShoot(_game))
                {

                    prevMouseState = mouseState;
                    mouseState = Mouse.LeftButton;
                    
                    if (mouseState == MouseButtonState.Pressed && prevMouseState == MouseButtonState.Released)
                    {
                        var mousePosition = Mouse.GetPosition(Application.Current.MainWindow);
                        _gameController.Shoot(_game, (int)mousePosition.X, (int)mousePosition.Y);
                    }
                }

                if (!(_gameController.GetCurrentDuck(_game).isFlyAway ^ _gameController.GetCurrentDuck(_game).isHit))
                {
                    _duckController.Fly(_game.Ducks[_game.currentDuck], delta);
                }
                else
                {
                    _gameController.DuckLeave(_game, delta);
                }
            }

            //_spriteDuckStates[_game.ducks[_game.currentDuck].enumDuckAnimState].UpdateFrame(delta);
            //_spriteDogStates[_game.dog.enumDogAnimState].UpdateFrame(delta);

            updateEntities();
        }

        private void updateEntities()
        {

            Canvas.SetLeft(_dogRect, _game.dog.posX);
            Canvas.SetTop(_dogRect, _game.dog.posY);
            if (_game.dog.isInBackround) 
            {
                Panel.SetZIndex(_dogRect, 0);
            } 
            else if (_game.isIntro)
            {
                Panel.SetZIndex(_dogRect, 2);
            }

            Canvas.SetLeft(_duckRect, _game.Ducks[_game.currentDuck].posX);
            Canvas.SetTop(_duckRect,  _game.Ducks[_game.currentDuck].posY);
        }
        private void findMyBackground()
        {
            Rectangle _backgroundRect = new Rectangle
            {
                Tag = "background",
                Width = 512,
                Height = 512,
                Fill = _background,
            };
            Panel.SetZIndex(_backgroundRect, 1);
            MainCanvas.Children.Add( _backgroundRect );
        }
        private void findMyDog()
        {
            _dogRect = new Rectangle
            {
                Tag = "dog",
                Width = 120,
                Height = 120,
                Fill = _dogSprite,
                Stretch = Stretch.Fill,
            };
            Panel.SetZIndex(_dogRect, 2);
            MainCanvas.Children.Add(_dogRect);

        }
        private void findMyDuck()
        {
            _duckRect = new Rectangle
            {
                Tag = "duck",
                Width = 64,
                Height = 64,
                Fill = _duckSprite,
                Stretch = Stretch.Fill,
                
            };
            Panel.SetZIndex(_duckRect, 0);
            MainCanvas.Children.Add(_duckRect);
        }

        private void uiButtonSaves_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Save button click + "+ _game.points);
            _gameSerializer.SaveGame(_game);
            
        }

        private void uiButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            _game = _gameSerializer.LoadGame();
        }

        private void uiButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            _game = _gameController.NewGame();
        }

    }
}

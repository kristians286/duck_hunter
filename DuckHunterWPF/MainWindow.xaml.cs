using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DuckHunter.Models;
using DuckHunter.Controllers;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows.Input;

using DuckHunter.Models.Enums;

namespace DuckHunterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private MediaPlayer _mediaPlayer = new MediaPlayer();

        DispatcherTimer gameTime = new DispatcherTimer();

        private Game _game;

        private int _timer = 0;
        

        private GameSerializer _gameSerializer = new GameSerializer();
        
        private ImageBrush _background = new ImageBrush();
        private ImageBrush _duckSprite = new ImageBrush();
        private ImageBrush _dogSprite = new ImageBrush();
        private ImageBrush _hudElements = new ImageBrush();

        private Rectangle _dogRect = new Rectangle();
        private Rectangle _duckRect = new Rectangle();

        private DateTime _currentTime = DateTime.Now;
        private DateTime _previous = DateTime.Now;

        private MouseButtonState mouseState;
        private MouseButtonState prevMouseState;

        private DispatcherTimer t;
        private DateTime _startTime = DateTime.Now;
        public MainWindow()
        {
            InitializeComponent();
            Cursor = Cursors.None;
            var musicPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\music\melody.mp3"));
            _mediaPlayer.Open(new Uri(musicPath));
            _mediaPlayer.MediaEnded += new EventHandler(Media_ended);
            _game = GameController.NewGame();

            t = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 0), DispatcherPriority.Background,
                GameTick, Dispatcher.CurrentDispatcher); t.IsEnabled = true;

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

        private void Media_ended(object? sender, EventArgs e)
        {
            _mediaPlayer.Position = TimeSpan.Zero; 
            _mediaPlayer.Play();
        }

        private void GameTick(object? sender, EventArgs e)
        {
            _previous = _currentTime;
            _currentTime = DateTime.Now;
            
            float delta = (float) (_currentTime.Millisecond  - _previous.Millisecond ) / 1000;
            if (delta < 0)
            {
                delta = 0.02f;
            }

            if (GameStateButtons.IsMouseOver)
            {
                _game.canShoot = true;
            } else
            {
                _game.canShoot = false;
            }

            _game.timer += (_currentTime - _previous);

            var mp = Mouse.GetPosition(this);
            Canvas.SetLeft(Crosshair, mp.X - Crosshair.ActualWidth / 2);
            Canvas.SetTop(Crosshair, mp.Y - Crosshair.ActualHeight / 2);


            if (DogController.IsVisible(GameController.GetDog(_game))) // DOG
            {
                if (GameController.GetIsIntro(_game))
                {
                    if (DogController.GetAnimState(_game.dog) == EnumDogState.WALK)
                    {
                        DogController.Walk(_game.dog, _game.screenWidth / 2 - 64 - 32, delta);
                            
                    }
                    else if (DogController.GetAnimState(_game.dog) == EnumDogState.SNIFF)
                    {
                        DogController.Sniff(_game.dog, delta);
                    }
                    else if (DogController.GetAnimState(_game.dog) == EnumDogState.JUMP)
                    {   
                        DogController.JumpInBush(_game.dog, _game.screenWidth / 2 - 64, delta);
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

                    prevMouseState = mouseState;
                    mouseState = Mouse.LeftButton;
                    
                    if (mouseState == MouseButtonState.Pressed && prevMouseState == MouseButtonState.Released)
                    {
                        var mousePosition = Mouse.GetPosition(Application.Current.MainWindow);
                        GameController.Shoot(_game, (int)mousePosition.X, (int)mousePosition.Y);
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
            this.DataContext = _game;
        }

        private void uiButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            _game = GameController.NewGame();
            this.DataContext = _game;
        }

        private void mediaPlay(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Play();
        }

        private int i = 0;
        private void mediaPause(object sender, RoutedEventArgs e)
        {
            

            _mediaPlayer.Pause();

            
        }
    }
}

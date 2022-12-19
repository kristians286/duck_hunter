
using System.ComponentModel;
using System.Xml.Serialization;

namespace DuckHunter.Models
{

    [XmlRoot("Game")]
    public class Game : INotifyPropertyChanged
    {
        private int _points;
        private int _round;
        private int _bullets;
        private TimeSpan _timer;
        public int screenHeight { get; set; }
        public int screenWidth { get; set; }

        public TimeSpan timer 
        {
            get { return _timer; }
            set 
            { 
                _timer = value;
                OnPropertyChanged("timer");
            }
        }

        public int round 
        {

            get { return _round ; }
            set 
            { 
                _round = value;
                OnPropertyChanged("round"); 
            }

        }
        public int points 
        {
            get {  return _points;  }
            set 
            { 
                _points = value;
                OnPropertyChanged("points");
            }
        }
        public int bullets 
        {
            get { return _bullets; }
            set
            {
                _bullets = value;
                OnPropertyChanged("bullets");
            }
        }

        public List<Duck>? Ducks { set; get; }
        public int ducksHitGoal { set; get; }
        public int ducksHitCount { set; get; }
        public int currentDuck { set; get; }

        public Dog dog { get; set; }

        // TURN THIS IN to EnumGameState
        public bool isGameOver { get; set; } // not used for now
        public bool isIntro { get; set; }
        public bool canShoot { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


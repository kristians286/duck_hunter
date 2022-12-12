using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.models
{
    internal class Game
    {
        public int screenHeight { get; set; }
        public int screenWidth { get; set; }

        public int round { set; get; }
        public int points { set; get; }
        public int bullets { set; get; }

        public List<Duck> ducks { set; get; }
        public int ducksHitGoal { set; get; }
        public int ducksHitCount { set; get; }
        public int currentDuck { set; get; }

        public Dog dog { get; set; }

        // TURN THIS IN to EnumGameState
        public bool isGameOver { get; set; } // not used for now
        public bool isIntro { get; set; }
        public bool canShoot { get; set; }
    }
}

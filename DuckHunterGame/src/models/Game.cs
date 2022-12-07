using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.models
{
    internal class Game
    {
        public int screenHeight;
        public int screenWidth;

        public int round { set; get; }
        public int points { set; get; }
        public int bullets { set; get; }

        public List<Duck> ducks { set; get; }
        public int ducksHitGoal { set; get; }
        public int ducksHitCount { set; get; }
        public int currentDuck { set; get; }

        public Dog dog;

        // TURN THIS IN to EnumGameState
        public bool isGameOver; // not used for now
        public bool isIntro;
        public bool canShoot;
    }
}

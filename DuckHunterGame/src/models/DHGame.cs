using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.models
{
    internal class DHGame
    {
        public int screenHeight;
        public int screenWidth;

        public int round;
        public int points;
        public int bullets;

        public List<Duck> ducks;
        public int birdHitGoal;
        public int currentDuck;

        public Dog dog;

        public bool isGameOver;
        public bool canShoot;
    }
}

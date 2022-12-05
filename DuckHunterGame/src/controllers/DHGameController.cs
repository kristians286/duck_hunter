using DuckHunterGame.src.models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.controllers
{
    internal class DHGameController
    {
        private DuckController duckController = new();
        private DogController dogController = new();
        public DHGame NewGame()
        {
            DHGame game = new();
            // TODO SET DEFAULT SETTINGS
            game.screenHeight = 8 * 64;
            game.screenWidth = 8 * 64;

            game.round = 0;
            game.points = 0;
            game.bullets = 3;
            game.ducks = new List<Duck>();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.birdHitGoal = 6;
            game.currentDuck = 0;

            return game;
        }


        public void Shoot(DHGame game, int MousePosX, int MousePosY)
        {
            //TODO
            SubBullets(game);
            
            Duck duck = game.ducks[game.currentDuck];  // gameContriller.getCurrentDuck(game) -> Duck()
            if ( (MousePosX > duck.posX && MousePosX < duck.posX + 64 ) &&
                MousePosY > duck.posY && MousePosY < duck.posY + 64) {
                //ChangeCanShoot(game);
                duckController.ChangeIsHit(duck);
                RestoreBullets(game);
            
            } else if (GetBullets(game) == 0)
            {
                duckController.ChangeIsFlyAway(duck);
                RestoreBullets(game);
            }
            
        }


        public int GetRound(DHGame game)
        {
            return game.round;
        }
        public void AddRound(DHGame game)
        {
            game.round++;
        }

        public int GetPoints(DHGame game) 
        { 
            return game.points;
        }
        public void AddPoints(DHGame game, int points)
        {
            game.points += points;
        }

        public int GetBullets(DHGame game)
        {
            return game.bullets;
        }
        public void SubBullets(DHGame game)
        {
            game.bullets--;
        }
        public void RestoreBullets(DHGame game)
        {
            game.bullets = 3;
        }

        public bool GetCanShoot(DHGame game)
        {
            return game.canShoot;
        }
        public void ChangeCanShoot(DHGame game)
        {
            game.canShoot = !game.canShoot;
        }
        
        public int GetCurrentDuck(DHGame game)
        {
            return game.currentDuck;
        }
        public void NextDuck(DHGame game)
        {
            game.currentDuck++;
        }

        public void ChangeGameOver(DHGame game)
        {
            game.isGameOver = !game.isGameOver;
        }

        public int GetHitGoal(DHGame game)
        {
            return game.birdHitGoal;
        }

        public void RestartGame(DHGame game) 
        {
            game = NewGame();
        }

        public Dog GetDog(DHGame game)
        {
            return game.dog;
        }
    }
}

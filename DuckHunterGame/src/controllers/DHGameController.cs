using DuckHunterGame.src.enums;
using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
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

            game.isIntro = true;

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

        public void DuckLeave(DHGame game, float delta) // MAYBE MOVE TO GAME CONTROLLER // MOVED FROM DUCK CONTROLLER
        {
            DisableIntro(game);
            Duck duck = GetCurrentDuck(game);
            Dog dog = GetDog(game);
            if (duck.isHit)
            {
                if (duckController.GetAnimState(duck) != EnumDuckAnimState.FALL) 
                {
                    duckController.ChangeAnimState(duck, EnumDuckAnimState.FALL);
                }

                if (duck.posY < 64 * 6)
                {
                    duck.posY += duck.speed * delta;
                }
                else
                {
                    dogController.ChangeIsVisable(dog);
                    dogController.SetDogPosition(dog, duck);
                    dogController.ChangeDogAnimState(dog, enums.EnumDogAnimState.SHOW_DUCK);
                }
            }
            else if (duck.isFlyAway)
            {
                if (duckController.GetAnimState(duck) != EnumDuckAnimState.FLY_UP)
                {
                    duckController.ChangeAnimState(duck, EnumDuckAnimState.FLY_UP);
                }

                if (duck.posY > -64)
                {
                    duck.posY -= duck.speed * delta;
                }
                else
                {
                    dogController.ChangeIsVisable(dog);
                    dogController.CenterDog(dog, game);
                    dogController.ChangeDogAnimState(dog, enums.EnumDogAnimState.LAUGH);
                }
            }
        }
        public void AnimateDog(DHGame game, float delta)
        {
            Dog dog = GetDog(game);
            if (dog.enumDogAnimState == EnumDogAnimState.SHOW_DUCK)
            {
                if (dog.animDuration < 1)
                {
                    dog.animDuration += delta;
                    dogController.Reveal(dog, delta);

                }
                else if (1 < dog.animDuration && dog.animDuration < 2)
                {
                    dog.animDuration += delta;
                    dogController.Hide(dog, delta);
                }
                else
                {
                    dog.enumDogAnimState = EnumDogAnimState.IDLE;
                    dogController.ChangeIsVisable(dog);
                    dogController.ResetAnimDuration(dog);
                    NextDuck(game);
                }

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
        
        public Duck GetCurrentDuck(DHGame game)
        {
            return game.ducks[game.currentDuck];
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
        public bool GetIsIntro(DHGame game)
        {
            return game.isIntro;
        }
        public void DisableIntro(DHGame game)
        {
            game.isIntro = false;
        }
    }
}

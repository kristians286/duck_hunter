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
            setDefaultSettings(game);
            return game;
        }

        private void setDefaultSettings(DHGame game)
        {
            game.screenHeight = 8 * 64;
            game.screenWidth = 8 * 64;

            game.round = 1;
            game.points = 0;
            game.bullets = 3;
            game.ducks = new List<Duck>();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.ducksHitGoal = 1;
            game.ducksHitCount = 0;
            game.currentDuck = 0;

            game.isIntro = true;
        }

        public void Shoot(DHGame game, int MousePosX, int MousePosY)
        {
            //TODO
            SubBullets(game);
            
            Duck duck = game.ducks[game.currentDuck];  // gameContriller.getCurrentDuck(game) -> Duck()
            if ( (MousePosX > duck.posX && MousePosX < duck.posX + 64 ) &&
                MousePosY > duck.posY && MousePosY < duck.posY + 64) {
                
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
            if (game.isIntro)
            {
                DisableIntro(game);
            }

            //slows down code;
            

            Duck duck = GetCurrentDuck(game);
            Dog dog = GetDog(game);
            
            if (duckController.GetIsHit(duck))
            {
                

                if (duckController.GetAnimDuration(duck) < 0.3)
                {
                    if (duckController.GetAnimState(duck) != EnumDuckAnimState.HIT)
                    {
                        ChangeCanShoot(game);
                        duckController.ChangeAnimState(duck, EnumDuckAnimState.HIT);
                        //duckController.RestoreAnimDuration(duck); // For now not realy needed
                    }
                    duck.animDuration += delta;
                } else {
                    duckController.AnimateDuck(GetCurrentDuck(game), delta);

                    if (duckController.GetAnimState(duck) != EnumDuckAnimState.FALL) 
                    {
                        duckController.ChangeAnimState(duck, EnumDuckAnimState.FALL);

                        AddHitCount(game);
                        AddPoints(game, duckController.GetPoints(duck));
                    }

                    if (duck.posY < 64 * 6)
                    {
                        duck.posY += 200 * delta;
                    }
                    else
                    {
                        ChangeCanShoot(game);
                        dogController.ChangeIsVisable(dog);
                        dogController.SetDogPosition(dog, duck);
                        dogController.ChangeDogAnimState(dog, EnumDogAnimState.SHOW_DUCK);
                    }
                }
            }
            else if (duckController.GetIsFlyAway(duck))
            {
                if (duckController.GetAnimState(duck) != EnumDuckAnimState.FLY_UP)
                {
                    ChangeCanShoot(game);
                    duckController.ChangeAnimState(duck, EnumDuckAnimState.FLY_UP);
                }

                if (duck.posY > -64)
                {
                    duck.posY -= 200 * delta;
                }
                else
                {
                    ChangeCanShoot(game);
                    dogController.ChangeIsVisable(dog);
                    dogController.CenterDog(dog, game);
                    dogController.ChangeDogAnimState(dog, EnumDogAnimState.LAUGH);
                }
            }

            

        }
        public void AnimateDog(DHGame game, float delta)
        {
            Dog dog = GetDog(game);
            if (dogController.GetAnimState(dog) == EnumDogAnimState.SHOW_DUCK || dogController.GetAnimState(dog) == EnumDogAnimState.LAUGH )
            {
                if (dogController.GetAnimDuration(dog) < 0.7)
                {
                    dog.animDuration += delta;
                    dogController.Reveal(dog, delta);

                }
                else if (0.7 < dogController.GetAnimDuration(dog) && dogController.GetAnimDuration(dog) < 1.4)
                {
                    dog.animDuration += delta;
                    dogController.Hide(dog, delta);
                }
                else
                {
                    dog.enumDogAnimState = EnumDogAnimState.IDLE;
                    dogController.ChangeIsVisable(dog);
                    dogController.ResetAnimDuration(dog);
                    
                    if (GetCurrentDuckNr(game) == GetTotalDucksNr(game))
                    {
                        if (GetHitGoal(game) <= GetDucksHitCount(game))
                        {
                            NextRound(game);
                        } else
                        {
                            RestartGame(game);
                        }
                    } else
                    {
                        NextDuck(game);
                    }
                }
            }
        }

        public int GetRound(DHGame game)
        {
            return game.round;
        }
        public void NextRound(DHGame game)
        {
            
            game.round++;
            game.ducks.Clear();
            duckController.GenerateDucks(game);

            //dogController.ChangeDogAnimState(GetDog(game), EnumDogAnimState.WALK);
            //dogController.ChangeIsVisable(GetDog(game));
            game.dog = dogController.NewDog();

            game.currentDuck = 0;
            game.ducksHitCount = 0;
            game.isIntro = true;

            
        }
        public void RestartGame(DHGame game)
        {
            game.round = 1;
            game.points = 0;
            game.ducks.Clear();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.ducksHitGoal = 1;
            game.ducksHitCount = 0;
            game.currentDuck = 0;

            game.isIntro = true;
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
        public int GetCurrentDuckNr(DHGame game)
        {
            return game.currentDuck;
        }
        public int GetTotalDucksNr(DHGame game)
        {
            return game.ducks.Count() - 1;
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
            return game.ducksHitGoal;
        }
        public void AddHitCount(DHGame game)
        {
            game.ducksHitCount++;
        }
        public int GetDucksHitCount(DHGame game)
        {
            return game.ducksHitCount;
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

using DuckHunterGame.src.enums;
using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.controllers
{
    internal class GameController
    {
        private DuckController duckController = new();
        private DogController dogController = new();
        public models.Game NewGame()
        {
            models.Game game = new();
            setDefaultSettings(game);
            return game;
        }

        private void setDefaultSettings(models.Game game)
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

        public void Shoot(models.Game game, int MousePosX, int MousePosY)
        {
            //TODO
            SubBullets(game);
            
            Duck duck = game.ducks[game.currentDuck];  // gameContriller.getCurrentDuck(game) -> Duck()
            if ( (MousePosX > duck.posX && MousePosX < duck.posX + 64 ) &&
                MousePosY > duck.posY && MousePosY < duck.posY + 64) {
                
                duckController.ChangeIsHit(duck);
                
                
            
            } else if (GetBullets(game) == 0)
            {
                duckController.ChangeIsFlyAway(duck);
                
            }
            
        }

        public void DuckLeave(models.Game game, float delta) // MAYBE MOVE TO GAME CONTROLLER // MOVED FROM DUCK CONTROLLER
        {
            if (game.isIntro)
            {
                DisableIntro(game);
            }

            Duck duck = GetCurrentDuck(game);
            Dog dog = GetDog(game);
            
            if (duckController.GetIsHit(duck))
            {
                

                if (duckController.GetAnimDuration(duck) < 0.3)
                {
                    if (duckController.GetAnimState(duck) != EnumDuckState.HIT)
                    {
                        ChangeCanShoot(game);
                        duckController.ChangeAnimState(duck, EnumDuckState.HIT);
                    }
                    duck.animDuration += delta;
                } else {
                    if (duckController.GetAnimState(duck) != EnumDuckState.FALL) 
                    {
                        duckController.ChangeAnimState(duck, EnumDuckState.FALL);
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
                        dogController.ChangeDogAnimState(dog, EnumDogState.SHOW_DUCK);
                    }
                }
            }
            else if (duckController.GetIsFlyAway(duck))
            {
                if (duckController.GetAnimState(duck) != EnumDuckState.FLY_UP)
                {
                    ChangeCanShoot(game);
                    duckController.ChangeAnimState(duck, EnumDuckState.FLY_UP);
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
                    dogController.ChangeDogAnimState(dog, EnumDogState.LAUGH);
                }
            }

            

        }
        public void DogReaction(models.Game game, float delta)
        {
            Dog dog = GetDog(game);
            if (dogController.GetAnimState(dog) == EnumDogState.SHOW_DUCK || dogController.GetAnimState(dog) == EnumDogState.LAUGH )
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
                    dog.enumDogAnimState = EnumDogState.IDLE;
                    dogController.ChangeIsVisable(dog);
                    dogController.ResetAnimDuration(dog);
                    
                    if (GetCurrentDuckNr(game) == GetTotalDucksNr(game))
                    {
                        if (GetHitGoal(game) <= GetDucksHitCount(game))
                        {
                            RestoreBullets(game);
                            NextRound(game);
                        } else
                        {
                            RestartGame(game);
                        }
                    } else
                    {
                        RestoreBullets(game);
                        NextDuck(game);
                    }
                }
            }
        }

        public int GetRound(models.Game game)
        {
            return game.round;
        }
        public void NextRound(models.Game game)
        {
            
            game.round++;
            game.ducks.Clear();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.currentDuck = 0;
            game.ducksHitCount = 0;
            game.isIntro = true;

        }
        public void RestartGame(models.Game game)
        {
            game.round = 1;
            game.points = 0;
            game.ducks.Clear();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.ducksHitGoal = 1;
            game.ducksHitCount = 0;
            game.currentDuck = 0;
            game.bullets = 3;
            game.isIntro = true;
        }

        public int GetPoints(models.Game game) 
        { 
            return game.points;
        }
        public void AddPoints(models.Game game, int points)
        {
            game.points += points;
        }
        public int GetBullets(models.Game game)
        {
            return game.bullets;
        }
        public void SubBullets(models.Game game)
        {
            game.bullets--;
        }
        public void RestoreBullets(models.Game game)
        {
            game.bullets = 3;
        }
        public bool GetCanShoot(models.Game game)
        {
            return game.canShoot;
        }
        public void ChangeCanShoot(models.Game game)
        {
            game.canShoot = !game.canShoot;
        }
        public Duck GetCurrentDuck(models.Game game)
        {
            return game.ducks[game.currentDuck];
        }
        public int GetCurrentDuckNr(models.Game game)
        {
            return game.currentDuck;
        }
        public int GetTotalDucksNr(models.Game game)
        {
            return game.ducks.Count() - 1;
        }
        public void NextDuck(models.Game game)
        {
            game.currentDuck++;
        }
        public void ChangeGameOver(models.Game game)
        {
            game.isGameOver = !game.isGameOver;
        }
        public int GetHitGoal(models.Game game)
        {
            return game.ducksHitGoal;
        }
        public void AddHitCount(models.Game game)
        {
            game.ducksHitCount++;
        }
        public int GetDucksHitCount(models.Game game)
        {
            return game.ducksHitCount;
        }
        public Dog GetDog(models.Game game)
        {
            return game.dog;
        }
        public bool GetIsIntro(models.Game game)
        {
            return game.isIntro;
        }
        public void DisableIntro(models.Game game)
        {
            game.isIntro = false;
        }
    }
}

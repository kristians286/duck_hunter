using DuckHunter.Models;
using DuckHunter.Models.Enums;

namespace DuckHunter.Controllers
{
    public class GameController
    {
        private DuckController duckController = new();
        private DogController dogController = new();
        public Game NewGame()
        {
            Game game = new();
            setDefaultSettings(game);
            return game;
        }

        private void setDefaultSettings(Game game)
        {
            game.screenHeight = 8 * 64;
            game.screenWidth = 8 * 64;

            game.round = 1;
            game.points = 0;
            game.bullets = 3;
            game.Ducks = new List<Duck>();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.ducksHitGoal = 1;
            game.ducksHitCount = 0;
            game.currentDuck = 0;

            game.isIntro = true;
        }

        public void Shoot(Game game, int MousePosX, int MousePosY)
        {
            //TODO
            SubBullets(game);
            
            Duck duck = game.Ducks[game.currentDuck];  // gameContriller.getCurrentDuck(game) -> Duck()
            if ( (MousePosX > duck.posX && MousePosX < duck.posX + 64 ) &&
                MousePosY > duck.posY && MousePosY < duck.posY + 64) {
                
                duckController.ChangeIsHit(duck);
                
                
            
            } else if (GetBullets(game) == 0)
            {
                duckController.ChangeIsFlyAway(duck);
                
            }
            
        }

        public void DuckLeave(Game game, float delta) 
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
        public void DogReaction(Game game, float delta)
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

        public int GetRound(Game game)
        {
            return game.round;
        }
        public void NextRound(Game game)
        {
            
            game.round++;
            game.Ducks.Clear();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.currentDuck = 0;
            game.ducksHitCount = 0;
            game.isIntro = true;

        }
        public void RestartGame(Game game)
        {
            game.round = 1;
            game.points = 0;
            game.Ducks.Clear();
            duckController.GenerateDucks(game);
            game.dog = dogController.NewDog();
            game.ducksHitGoal = 1;
            game.ducksHitCount = 0;
            game.currentDuck = 0;
            game.bullets = 3;
            game.isIntro = true;
        }

        public int GetPoints(Game game) 
        { 
            return game.points;
        }
        public void AddPoints(Game game, int points)
        {
            game.points += points;
        }
        public int GetBullets(Game game)
        {
            return game.bullets;
        }
        public void SubBullets(Game game)
        {
            game.bullets--;
        }
        public void RestoreBullets(Game game)
        {
            game.bullets = 3;
        }
        public bool GetCanShoot(Game game)
        {
            return game.canShoot;
        }
        public void ChangeCanShoot(Game game)
        {
            game.canShoot = !game.canShoot;
        }
        public Duck GetCurrentDuck(Game game)
        {
            return game.Ducks[game.currentDuck];
        }
        public int GetCurrentDuckNr(Game game)
        {
            return game.currentDuck;
        }
        public int GetTotalDucksNr(Game game)
        {
            return game.Ducks.Count() - 1;
        }
        public void NextDuck(Game game)
        {
            game.currentDuck++;
        }
        public void ChangeGameOver(Game game)
        {
            game.isGameOver = !game.isGameOver;
        }
        public int GetHitGoal(Game game)
        {
            return game.ducksHitGoal;
        }
        public void AddHitCount(Game game)
        {
            game.ducksHitCount++;
        }
        public int GetDucksHitCount(Game game)
        {
            return game.ducksHitCount;
        }
        public Dog GetDog(Game game)
        {
            return game.dog;
        }
        public bool GetIsIntro(Game game)
        {
            return game.isIntro;
        }
        public void DisableIntro(Game game)
        {
            game.isIntro = false;
        }
    }
}

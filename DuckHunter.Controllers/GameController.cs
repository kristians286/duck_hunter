using DuckHunter.Models;
using DuckHunter.Models.Enums;

namespace DuckHunter.Controllers
{
    public static class GameController
    {
        public static Game NewGame()
        {
            Game game = new();
            SetDefaultSettings(game);
            return game;
        }

        private static void SetDefaultSettings(Game game)
        {
            game.screenHeight = 8 * 64;
            game.screenWidth = 8 * 64;

            game.round = 1;
            game.points = 0;
            game.bullets = 3;
            game.Ducks = new List<Duck>();
            DuckController.GenerateDucks(game);
            game.dog = DogController.NewDog();
            game.ducksHitGoal = 1;
            game.ducksHitCount = 0;
            game.currentDuck = 0;
            game.gameState = EnumGameStates.GAME_RUNNING;
            game.isIntro = true;
        }

        public static void Shoot(Game game, int MousePosX, int MousePosY)
        {
            //TODO
            SubBullets(game);
            
            Duck duck = game.Ducks[game.currentDuck];  // gameContriller.getCurrentDuck(game) -> Duck()
            if ( (MousePosX > duck.posX && MousePosX < duck.posX + 64 ) &&
                MousePosY > duck.posY && MousePosY < duck.posY + 64) {
                
                DuckController.ChangeIsHit(duck);
                
                
            
            } else if (GetBullets(game) == 0)
            {
                DuckController.ChangeIsFlyAway(duck);
                
            }
            
        }

        public static void DuckLeave(Game game, float delta) 
        {
            if (game.isIntro)
            {
                DisableIntro(game);
            }

            Duck duck = GetCurrentDuck(game);
            Dog dog = GetDog(game);
            
            if (DuckController.GetIsHit(duck))
            {
                

                if (DuckController.GetAnimDuration(duck) < 0.3)
                {
                    if (DuckController.GetAnimState(duck) != EnumDuckState.HIT)
                    {
                        ChangeCanShoot(game);
                        DuckController.ChangeAnimState(duck, EnumDuckState.HIT);
                    }
                    duck.animDuration += delta;
                } else {
                    if (DuckController.GetAnimState(duck) != EnumDuckState.FALL) 
                    {
                        DuckController.ChangeAnimState(duck, EnumDuckState.FALL);
                        AddHitCount(game);
                        AddPoints(game, DuckController.GetPoints(duck));
                    }
                    
                    if (duck.posY < 64 * 6)
                    {
                        duck.posY += 200 * delta;

                    }
                    else
                    {
                        ChangeCanShoot(game);
                        DogController.ChangeIsVisable(dog);
                        DogController.SetDogPosition(dog, duck); 
                        DogController.ChangeDogAnimState(dog, EnumDogState.SHOW_DUCK);
                    }
                }
            }
            else if (DuckController.GetIsFlyAway(duck))
            {
                if (DuckController.GetAnimState(duck) != EnumDuckState.FLY_UP)
                {
                    ChangeCanShoot(game);
                    DuckController.ChangeAnimState(duck, EnumDuckState.FLY_UP);
                }

                if (duck.posY > -64)
                {
                    duck.posY -= 200 * delta;
                }
                else
                {
                    ChangeCanShoot(game);
                    DogController.ChangeIsVisable(dog);
                    DogController.CenterDog(dog, game);
                    DogController.ChangeDogAnimState(dog, EnumDogState.LAUGH);
                }
            }

            

        }
        public static void DogReaction(Game game, float delta)
        {
            Dog dog = GetDog(game);
            if (DogController.GetAnimState(dog) == EnumDogState.SHOW_DUCK || DogController.GetAnimState(dog) == EnumDogState.LAUGH )
            {
                if (DogController.GetAnimDuration(dog) < 0.7)
                {
                    dog.animDuration += delta;
                    DogController.Reveal(dog, delta);

                }
                else if (0.7 < DogController.GetAnimDuration(dog) && DogController.GetAnimDuration(dog) < 1.4)
                {
                    dog.animDuration += delta;
                    DogController.Hide(dog, delta);
                }
                else
                {
                    dog.enumDogAnimState = EnumDogState.IDLE;
                    DogController.ChangeIsVisable(dog);
                    DogController.ResetAnimDuration(dog);
                    
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

        public static int GetRound(Game game)
        {
            return game.round;
        }
        public static void NextRound(Game game)
        {
            
            game.round++;
            game.Ducks.Clear();
            DuckController.GenerateDucks(game);
            game.dog = DogController.NewDog();
            game.currentDuck = 0;
            game.ducksHitCount = 0;
            game.isIntro = true;

        }
        public static void RestartGame(Game game)
        {
            game.round = 1;
            game.points = 0;
            game.Ducks.Clear();
            DuckController.GenerateDucks(game);
            game.dog = DogController.NewDog();
            game.ducksHitGoal = 1;
            game.ducksHitCount = 0;
            game.currentDuck = 0;
            game.bullets = 3;
            game.isIntro = true;
            game.gameState = EnumGameStates.GAME_RUNNING;
        }

        public static int GetPoints(Game game) 
        { 
            return game.points;
        }
        public static void AddPoints(Game game, int points)
        {
            game.points += points;
        }
        public static int GetBullets(Game game)
        {
            return game.bullets;
        }
        public static void SubBullets(Game game)
        {
            game.bullets--;
        }
        public static void RestoreBullets(Game game)
        {
            game.bullets = 3;
        }
        public static bool GetCanShoot(Game game)
        {
            return game.canShoot;
        }
        public static void ChangeCanShoot(Game game)
        {
            game.canShoot = !game.canShoot;
        }
        public static Duck GetCurrentDuck(Game game)
        {
            return game.Ducks[game.currentDuck];
        }
        public static int GetCurrentDuckNr(Game game)
        {
            return game.currentDuck;
        }
        public static int GetTotalDucksNr(Game game)
        {
            return game.Ducks.Count() - 1;
        }
        public static void NextDuck(Game game)
        {
            game.currentDuck++;
        }
        public static int GetHitGoal(Game game)
        {
            return game.ducksHitGoal;
        }
        public static void AddHitCount(Game game)
        {
            game.ducksHitCount++;
        }
        public static int GetDucksHitCount(Game game)
        {
            return game.ducksHitCount;
        }
        public static Dog GetDog(Game game)
        {
            return game.dog;
        }
        public static bool GetIsIntro(Game game)
        {
            return game.isIntro;
        }
        public static void DisableIntro(Game game)
        {
            game.isIntro = false;
        }
    }
}

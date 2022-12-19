
using DuckHunter.Models.Enums;
using DuckHunter.Models;

namespace DuckHunter.Controllers
{
    public class DogController
    {
        public static Dog NewDog()
        {
            Dog dog = new();
            setDefaultValues(dog);
            return dog;
        }

        private static void setDefaultValues(Dog dog)
        {
            dog.posY = 64 * 5;
            dog.posX = 64 + 32;
            dog.isVisable = true;
            dog.isInBackround = false;
            dog.enumDogAnimState = EnumDogState.WALK;
        }

        public static void Walk(Dog dog, int targetPosX, float delta)
        {
            //add: if enum state not WALK {}

            if (dog.posX < targetPosX)
            {
                dog.posX += 50 * delta;
            } else
            {
                dog.enumDogAnimState = EnumDogState.SNIFF;
                ResetAnimDuration(dog);
            }
        }

        public static void Sniff(Dog dog, float delta)
        {
            if (dog.animDuration < 1.46f)
            {
                dog.animDuration += delta;
            } else
            {
                ChangeDogAnimState(dog, EnumDogState.JUMP);
                ResetAnimDuration(dog);
            }
        }

        public static void JumpInBush(Dog dog, int targetPosX , float delta) // MIGHT NEED TO MOVE TO GAME CONTROLLER
        {
            dog.animDuration += delta;
            if (dog.posX < targetPosX)
            {
                dog.posX += 64 * delta ;
                dog.posY -= 64 * 3 * delta ;
            } else
            {   
                if (dog.isInBackround != true)
                {
                    ChangeIsInBackgound(dog);
                }
                if (dog.posY < 64 * 6 - 32)
                {
                    dog.posY += 300 * delta ;
                } else
                {
                    if (dog.animDuration < 3)
                    {
                        
                    } else
                    {
                        ChangeIsVisable(dog);
                        ChangeDogAnimState(dog, EnumDogState.IDLE);
                        ResetAnimDuration(dog);
                    }
                    
                }
            
            }
        }
        public static float GetAnimDuration(Dog dog)
        {
            return dog.animDuration;
        }
        public static EnumDogState GetAnimState(Dog dog)
        {
            return dog.enumDogAnimState;
        }
        public static void Reveal(Dog dog, float delta)
        {
            dog.posY -= 150 * delta;
             
        }
        public static void Hide(Dog dog, float delta)
        {
            dog.posY += 150 * delta;
        }
        public static void SetDogPosition(Dog dog, Duck duck)
        {
            if (duck.posX > 64 * 5 - 32)
            {
                dog.posX = 64 * 5 - 32;
            } 
            else if ( duck.posX < 64 *2)
            {
                dog.posX = 64 * 2;
            } else
            {
                dog.posX = duck.posX;
            }
            
        }
        public static void CenterDog(Dog dog, Game game)
        {
            dog.posX = game.screenWidth / 2 - 32;
        }
        public static void ChangeDogAnimState(Dog dog, EnumDogState targetState)
        {
            dog.enumDogAnimState = targetState;
        }
        public static void ChangeIsVisable(Dog dog)
        {
            dog.isVisable = !dog.isVisable;
        }
        public static bool IsVisible(Dog dog)
        {
            return dog.isVisable;
        }
        public static void ChangeIsInBackgound(Dog dog)
        {
            dog.isInBackround = !dog.isInBackround;
        }
        public static bool GetIsInBackground(Dog dog)
        {
            return dog.isInBackround;
        }
        public static void ResetAnimDuration(Dog dog)
        {
            dog.animDuration = 0;
        }
    }
}

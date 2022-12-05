using DuckHunterGame.src.enums;
using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.controllers
{
    internal class DogController
    {
        public Dog NewDog()
        {
            Dog dog = new();
            dog.posY = 64 * 6;
            dog.posX = 64;
            dog.isVisable = true;
            dog.isInBackround = false;
            return dog;
        }

        public void Move(Dog dog, int targetPosX, float delta)
        {
            if (dog.posX < targetPosX)
            {
                dog.posX += 50 * delta;
            } else
            {   
                if (dog.animDuration < 1)
                {
                    dog.animDuration += delta;
                } else 
                {
                    JumpInBush(dog, targetPosX, delta);
                }
                
            }
            //TODO
        }

        public void JumpInBush(Dog dog, int targetPosX , float delta) // MIGHT NEED TO MOVE TO GAME CONTROLLER
        {
            if (dog.posX < targetPosX + 32)
            {
                dog.posX += 25* delta;
                dog.posY -= 100* delta;
            } else
            {   
                ChangeIsInBackgound(dog);
                if (dog.posY < 64*6)
                {
                    dog.posY += 100 * delta;
                } else
                {
                    ChangeIsVisable(dog);
                    //gameController.NextDuck(game); // IDK 
                }
            
            }
        }

        public void Reval(Dog dog, float delta)
        {

            if (dog.posY < 64*4)
            {
                dog.posY -= 200 * delta;
            } else
            {
                if (dog.animDuration < 0.5)
                {
                    dog.animDuration += delta;
                } else
                {
                    Hide(dog, delta);
                }
            }
        }
        public void Hide(Dog dog, float delta)
        {
            if (dog.posY > 64* 6)
            {
                dog.posY += 200* delta;
            } else
            {
                //ChangeIsVisable(dog);
            }
        }

        public void SetDogPosition(Dog dog, Duck duck)
        {
            dog.posX = duck.posX;
        }

        public void CenterDog(Dog dog, DHGame game)
        {
            dog.posX = game.screenWidth / 2;
        }

        public void ChangeDogAnimState(Dog dog, EnumDogAnimState targetState)
        {
            dog.enumDogAnimState = targetState;
        }

        public void ChangeIsVisable(Dog dog) // MIGHT REMOVE BECAUSE THE DOG WOULD ALWAYS BE IN BACKGROUND
        {
            dog.isVisable = !dog.isVisable;
        }

        public bool IsVisable(Dog dog)
        {
            return dog.isVisable;
        }

        public void ChangeIsInBackgound(Dog dog)
        {
            dog.isInBackround = !dog.isInBackround;
        }
        public bool GetIsInBackground(Dog dog)
        {
            return dog.isInBackround;
        }
    }
}

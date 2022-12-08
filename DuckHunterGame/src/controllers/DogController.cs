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
            setDefaultValues(dog);
            return dog;
        }

        private void setDefaultValues(Dog dog)
        {
            dog.posY = 64 * 6 - 32;
            dog.posX = 64;
            dog.isVisable = true;
            dog.isInBackround = false;
            dog.enumDogAnimState = EnumDogState.WALK;
        }

        public void Walk(Dog dog, int targetPosX, float delta)
        {
            //add: if enum state not WALK {}

            if (dog.posX < targetPosX)
            {
                dog.posX += 50 * delta;
            } else
            {   
                dog.enumDogAnimState = EnumDogState.SNIFF;
                //JumpInBush(dog, targetPosX, delta);
            }
        }

        public void Sniff(Dog dog, float delta)
        {
            if (dog.animDuration < 0.5)
            {
                dog.animDuration += delta;
            } else if (0.5 < dog.animDuration && dog.animDuration < 1) 
            {
                dog.animDuration += delta * 100;
                // looks up before jumping in bush
            } else
            {
                ResetAnimDuration(dog);
                ChangeDogAnimState(dog, EnumDogState.JUMP);
            }
        }

        public void JumpInBush(Dog dog, int targetPosX , float delta) // MIGHT NEED TO MOVE TO GAME CONTROLLER
        {
            if (dog.posX < targetPosX + 32)
            {
                dog.posX += 50* delta;
                dog.posY -= 100* delta;
            } else
            {   
                if (dog.isInBackround != true)
                {
                    ChangeIsInBackgound(dog);
                }
                if (dog.posY < 64*6 - 32)
                {
                    dog.posY += 100 * delta;
                } else
                {
                    ChangeIsVisable(dog);
                    ResetAnimDuration(dog);
                }
            
            }
        }
        public float GetAnimDuration(Dog dog)
        {
            return dog.animDuration;
        }
        public EnumDogState GetAnimState(Dog dog)
        {
            return dog.enumDogAnimState;
        }
        public void Reveal(Dog dog, float delta)
        {
            dog.posY -= 100 * delta;
             
        }
        public void Hide(Dog dog, float delta)
        {
            dog.posY += 100* delta;
        }
        public void SetDogPosition(Dog dog, Duck duck)
        {
            dog.posX = duck.posX;
        }
        public void CenterDog(Dog dog, models.Game game)
        {
            dog.posX = game.screenWidth / 2 - 32;
        }
        public void ChangeDogAnimState(Dog dog, EnumDogState targetState)
        {
            dog.enumDogAnimState = targetState;
        }
        public void ChangeIsVisable(Dog dog)
        {
            dog.isVisable = !dog.isVisable;
        }
        public bool IsVisible(Dog dog)
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
        public void ResetAnimDuration(Dog dog)
        {
            dog.animDuration = 0;
        }
    }
}

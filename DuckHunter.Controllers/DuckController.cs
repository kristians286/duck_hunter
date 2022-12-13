using DuckHunter.Models;
using DuckHunter.Models.Enums;

namespace DuckHunter.Controllers
{
    public class DuckController
    {
        public Duck NewDuck()
        {
             
            Duck duck = new();
            setDefaultSettings(duck);
            return duck;
        }

        private void setDefaultSettings(Duck duck)
        {
            Random rand = new();
            Array duckTypeList = Enum.GetValues(typeof(EnumDuckType));
            int enumDuckTypeValues = rand.Next(duckTypeList.Length);
            duck.posX = rand.Next(0, 64 * 7);
            duck.posY = 64 * 7;
            duck.speed = 200 + (50 * enumDuckTypeValues);
            duck.flyDirHorizontal = rand.Next(2) == 1;
            if (duck.flyDirHorizontal)
            {
                duck.enumDuckAnimState = EnumDuckState.FLY_RIGHT;
            }
            else
            {
                duck.enumDuckAnimState = EnumDuckState.FLY_LEFT;
            }
            duck.enumDuckType = (EnumDuckType)duckTypeList.GetValue(enumDuckTypeValues);
            duck.points = 500 + (enumDuckTypeValues * 500);

        }

        public void GenerateDucks(Game game)
        {
            for (int i = 0; i < 10; i ++)
            {
                game.Ducks.Add(NewDuck());
            }
        }
        
        public void Fly(Duck duck, float delta) // TODO ADD DEVIATION = A RANDOM VAR THAT MAKES IT LESS PREDICTABLE
        {

            if (duck.flyDirVertical)
            {
                duck.posY += duck.speed * delta;
                if (duck.posY > 64*5) 
                {
                    ChangeFlyDirVertical(duck);
                }
            } else
            {
                duck.posY -= duck.speed * delta;
                if (duck.posY < 0)
                {
                    ChangeFlyDirVertical(duck);
                }
            }
            
            if (duck.flyDirHorizontal)
            {
                duck.posX += duck.speed * delta;
                if (duck.posX > 64 * 7)
                {
                    ChangeFlyDirHorizontal(duck);
                    ChangeAnimState(duck, EnumDuckState.FLY_LEFT);
                }
            } else
            {
                duck.posX -= duck.speed * delta;
                if (duck.posX < 0)
                {
                    ChangeFlyDirHorizontal(duck);
                    ChangeAnimState(duck, EnumDuckState.FLY_RIGHT);
                }
            }
        }

        public int GetPoints(Duck duck)
        {
            return duck.points;
        }
        public bool GetIsHit(Duck duck)
        {
            return duck.isHit;
        }
        public void ChangeIsHit(Duck duck)
        {
            duck.isHit = !duck.isHit;
        }
        public bool GetIsVisable(Duck duck)
        {
            return duck.isVisable;
        }
        public void ChangeIsVisable(Duck duck)
        {
            duck.isVisable = !duck.isVisable;
        }
        public bool GetIsFlyAway(Duck duck)
        {
            return duck.isFlyAway;
        }
        public void ChangeIsFlyAway(Duck duck)
        {
            duck.isFlyAway= !duck.isFlyAway;
        }
        public void ChangeFlyDirVertical(Duck duck)
        {
            duck.flyDirVertical = !duck.flyDirVertical;
        }
        public void ChangeFlyDirHorizontal(Duck duck)
        {
            duck.flyDirHorizontal = !duck.flyDirHorizontal;
        }
        public void ChangeAnimState(Duck duck, EnumDuckState targetState)
        {
            duck.enumDuckAnimState = targetState;
        }
        public EnumDuckState GetAnimState(Duck duck)
        {
            return duck.enumDuckAnimState;
        }
        public float GetAnimDuration(Duck duck)
        {
            return duck.animDuration;
        }
        public void RestoreAnimDuration(Duck duck)
        {
            duck.animDuration = 0;
        }


    }
}

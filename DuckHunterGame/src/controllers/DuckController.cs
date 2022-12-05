﻿using DuckHunterGame.src.enums;
using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.controllers
{
    internal class DuckController
    {
        public Duck NewDuck()
        {
            Random rand = new Random(); 
            Duck duck = new();
            duck.posX = rand.Next(0, 64 * 7);
            duck.posY = 64*7;
            duck.speed = 200;
            duck.flyDirHorizontal = rand.Next(2) == 1;
            if (duck.flyDirHorizontal)
            {
                duck.enumDuckAnimState = EnumDuckAnimState.FLY_RIGHT;
            } else
            {
                duck.enumDuckAnimState = EnumDuckAnimState.FLY_LEFT;
            }
            return duck;
        }

        public void GenerateDucks(DHGame game)
        {
            for (int i = 0; i < 10; i ++)
            {
                game.ducks.Add(NewDuck());
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
                    duck.enumDuckAnimState = EnumDuckAnimState.FLY_LEFT;
                }
            } else
            {
                duck.posX -= duck.speed * delta;
                if (duck.posX < 0)
                {
                    ChangeFlyDirHorizontal(duck);
                    duck.enumDuckAnimState = EnumDuckAnimState.FLY_RIGHT;
                }
            }
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
        
        public void ChangeAnimState(Duck duck, EnumDuckAnimState targetState)
        {
            duck.enumDuckAnimState = targetState;
        }
        public EnumDuckAnimState GetAnimState(Duck duck)
        {
            return duck.enumDuckAnimState;
        }


    }
}

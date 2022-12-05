using DuckHunterGame.src.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.models
{
    internal class Dog
    {
        public float posX;
        public float posY;
        public int posTargetX;
        public int posTargetY;

        public bool isInBackround;
        public bool isVisable;

        public float animDuration;
        public EnumDogAnimState enumDogAnimState;
    }
}

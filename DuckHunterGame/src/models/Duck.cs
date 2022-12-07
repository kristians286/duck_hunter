using DuckHunterGame.src.enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.models
{
    internal class Duck
    {
        public float posX;
        public float posY;

        public int height;
        public int width;

        public int points;
        public int speed;

        public bool isHit;
        public bool isFlyAway;
        public bool isVisable;

        public bool flyDirVertical;
        public bool flyDirHorizontal;

        public float animDuration;

        public EnumDuckType enumDuckType;
        public EnumDuckAnimState enumDuckAnimState;

    }
}

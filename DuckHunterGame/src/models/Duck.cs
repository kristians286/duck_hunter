﻿using DuckHunterGame.src.enums;
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
        public float posX { get; set; }
        public float posY { get; set; }

        public int points { set; get; }
        public int speed { set; get; }

        public bool isHit { set; get; }
        public bool isFlyAway { set; get; }
        public bool isVisable { set; get; }

        public bool flyDirVertical { get; set; }
        public bool flyDirHorizontal { get; set; }

        public float animDuration { get; set; }

        public EnumDuckType enumDuckType { get; set; }
        public EnumDuckState enumDuckAnimState { get; set; }

    }
}

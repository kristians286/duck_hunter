using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.controllers
{

    public class AnimationController
    {
        private int _spriteHeight;
        private int _spriteWidth;

        private int _textureHeight;
        private int _textureWidth;

        private int _row;
        private int _col;
        private int _totalCols;

        private float _timeElapsed;
        private float _timePerFreme;


        public AnimationController(int spriteHeight, int spriteWidth, int textureRow, int textureWidth, float timePerFrame)
        {
            _spriteHeight = spriteHeight;
            _spriteWidth = spriteWidth;
            
            _row = textureRow;
            _textureWidth = textureWidth;

            _totalCols = (_textureWidth / _spriteWidth) -1;

            _timePerFreme = timePerFrame;

        }

        public void UpdateFrame(float delta)
        {
            
            _timeElapsed += delta;
            if (_timeElapsed > _timePerFreme)
            {
                _col++;
                _timeElapsed -= _timePerFreme;
                if (_col > _totalCols)
                {
                    _col = 0;
                }
            } 

        }

        public Rectangle GetFrame()
        {
            return new Rectangle(_spriteWidth * _col, _spriteHeight * _row, _spriteWidth, _spriteHeight); ;
        }

        public String print() // For viewing only / will be deleted
        {
            return $"{_timeElapsed},\n {_row}, {_col}";
        }

    }
}

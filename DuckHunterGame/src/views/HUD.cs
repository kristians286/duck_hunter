﻿using DuckHunterGame.src.models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.views
{
    internal class HUD
    {
        private models.Game game;
        private Texture2D hudElements;
        private SpriteBatch spriteBatch;

        public HUD(models.Game game, Texture2D hudElements, SpriteBatch spriteBatch)
        {
            this.game = game;
            this.hudElements = hudElements;
            this.spriteBatch = spriteBatch;
        }

        public void Draw(GameTime gameTime)
        {
            if (game.round < 10)
            {
                spriteBatch.Draw(hudElements, new Rectangle(64 + 16, 64 * 6 + 32, 16, 16), new Rectangle(8 * game.round, 0, 8, 8), Color.White);
            } else
            {
                spriteBatch.Draw(hudElements, new Rectangle(64 + 16, 64 * 6 + 32, 16, 16), new Rectangle(8 * (game.round / 10), 0, 8, 8), Color.White);
                spriteBatch.Draw(hudElements, new Rectangle(64 + 32, 64 * 6 + 32, 16, 16), new Rectangle(8 * (game.round % 10), 0, 8, 8), Color.White);
            }
            
            for (int i = 0; i < game.bullets; i++)
            {
                spriteBatch.Draw(hudElements, new Rectangle(32 + 16 + (16*i) -2, 64 * 7, 16, 16), new Rectangle( 80, 0, 8, 8), Color.White);
            }


        }
    }
}

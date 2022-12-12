using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DuckHunterGame.src.views
{
    internal class ComponentButton
    {
        public Vector2 Position { get; set; }
        public String Text { get; set; }

        private int width;
        private int height;

        private MouseState _mouseState;
        private MouseState _prevMouseState;
        private bool _isHovering;

        private Texture2D _texture;
        private SpriteFont _font;

        public EventHandler ClickEvent;

        public ComponentButton(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
        }

        public void OnClick()
        {
            ClickEvent?.Invoke(this,EventArgs.Empty);   
        }

        public void Update(GameTime gameTime)
        {
            _prevMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            var mouseRect = new Rectangle(_mouseState.X, _mouseState.Y, 1, 1);
            _isHovering = false;

            if (mouseRect.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
                {
                    OnClick();
                }
            }

            
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (_isHovering) 
            { 
                color = Color.Gray;
            }
            spriteBatch.Draw(_texture, Rectangle, color);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), Color.Black);
            }

        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X,(int)Position.Y,_texture.Width,_texture.Height);
            }
        }

    }
}

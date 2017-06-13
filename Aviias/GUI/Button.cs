using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Button
    {
        public Vector2 _position { get; set; }
        public int _width { get; set; }
        public int _height { get; set; }
        string _texture;
        MouseState mouseState = Mouse.GetState();
        public bool IsTrue { get; set; }

        public Button(Vector2 position, int width, int height)
        {
            _position = position;
            _width = width;
            _height = height;
            _texture = ".\\Menu\\Button\\jouer_gris";
        }

        public bool IsPressed()
        {
            return mouseState.LeftButton == ButtonState.Pressed;           
        }

        internal void Update(GameTime gameTime, ContentManager Content)
        {
            mouseState = Mouse.GetState();
            if (IsPressed() && mouseState.Position.X >= _position.X && mouseState.Position.Y >= _position.Y && mouseState.Position.X <= _position.X + _width && mouseState.Position.Y <= _position.Y + _height )
            {
                _texture = ".\\Menu\\Button\\jouer_rouge";
                IsTrue = true;
            }
            else
            { 
                _texture = ".\\Menu\\Button\\jouer_gris";
                IsTrue = false;
            }
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {

            spriteBatch.Draw(content.Load<Texture2D>(_texture), new Vector2(305, 675), null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
        }
    }
}

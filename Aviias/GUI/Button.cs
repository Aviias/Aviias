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
    [Serializable]
    public class Button
    {
        [field: NonSerialized]
        public Vector2 _position;// { get; set; }
        public int _width { get; set; }
        public int _height { get; set; }
        public string _texture;
        [field: NonSerialized]
        MouseState mouseState = Mouse.GetState();
        public bool IsTrue { get; set; }
        public string _name { get; set; }
        public int _id { get;set; }

        public Button(Vector2 position, int width, int height)
        {
            _position = position;
            _width = width;
            _height = height;
            _texture = "";
        }

        public Button(Vector2 position, int width, int height, string texture, string name)
        {
            _position = position;
            _width = width;
            _height = height;
            _texture = texture;
            _name = name;
            _id++;
        }

        public bool IsPressed(MouseState mouseState)
        {
            return mouseState.LeftButton == ButtonState.Pressed;           
        }


        internal void Draw(SpriteBatch spriteBatch, ContentManager content , Vector2 pos)
        {

            spriteBatch.Draw(content.Load<Texture2D>(_texture), pos, null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            spriteBatch.Draw(content.Load<Texture2D>(_texture), _position, null, Color.White, 0f, Vector2.Zero, 1.1f,
               SpriteEffects.None, 0f);
        }
    }
}

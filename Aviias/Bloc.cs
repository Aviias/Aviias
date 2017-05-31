using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Bloc
    {
        protected Vector2 _position;
        public Texture2D _texture;
        float _scale;
        string _type;
        bool _isBreakable;
        bool _isAir;
        public float moveSpeed = 0.1f;

        public Bloc(Vector2 position, float scale, string type, ContentManager content)
        {
            _position = position;
            _texture = content.Load<Texture2D>(type);
            _scale = scale;
            _type = type;
            if (type != "air")
            {
                _isBreakable = true;
                _isAir = false;
            }
            else
            {
                _isBreakable = false;
                _isAir = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, _scale / 64, SpriteEffects.None, 0f);
        }

        

        public void setBloc(Bloc bloc, Vector2 position, ContentManager content)
        {
            
        }

        public bool IsBreakable
        {
            get { return _isBreakable; }
        }

        public Vector2 GetPosBlock
        {
            get { return _position; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public void ChangeBloc(string newTexture, ContentManager content)
        {
            _texture = content.Load<Texture2D>(newTexture);
        }

        public float posX
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float posY
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        public int Width
        {
            get { return _texture.Width; }
        }

        public int Height
        {
            get { return _texture.Height; }
        }
    }
}

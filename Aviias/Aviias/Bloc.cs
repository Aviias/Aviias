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
    [Serializable]
    public class Bloc
    {
        [field: NonSerialized]
        public Vector2 _position;
        [field: NonSerialized]
        public Texture2D _texture;
        float _scale;
        int _x;
        int _y;
        string _type;
        bool _isBreakable;
        bool _isAir;
        public float moveSpeed = 0.1f;
        int _luminosity;
        public bool _isInContactWithTheSky;

        public Bloc(Vector2 position, float scale, string type, ContentManager content, int x, int y)
        {
            //_position = position;
            _position = new Vector2(x, y);
            _x = x;
            _y = y;
            _texture = content.Load<Texture2D>(type);
            _scale = scale;
            _type = type;
            _luminosity = 6;
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

        public void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            spriteBatch.Draw(content.Load<Texture2D>(_type), new Vector2(_x, _y), null, new Color(_luminosity, _luminosity, _luminosity, 255), 0f, Vector2.Zero, _scale / 64, SpriteEffects.None, 0f);
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
            if (newTexture == "ladder") _type = newTexture;
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
            get { return 16; }
        }

        public int Height
        {
            get { return 16; }
        }

        public int Luminosity
        {
            get { return _luminosity/36; }
        }

        public void DecreaseLuminosity(int luminosity)
        {
            if (luminosity < 0) luminosity = 0;
            _luminosity = luminosity * 36;
        }

    }
}

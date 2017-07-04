﻿using Microsoft.Xna.Framework;
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
        protected Vector2 _position;
        [field: NonSerialized]
        public Texture2D _texture;
        float _scale;
        string _type;
        bool _isBreakable;
        bool _isAir;
        int x;
        int y;
        public float moveSpeed = 0.1f;
        int _luminosity;
        public bool _isInContactWithTheSky;
        bool _isSetable;

        public Bloc(Vector2 position, float scale, string type, ContentManager content)
        {
            _position = position;
            _texture = content.Load<Texture2D>(type);
            _scale = scale;
            _type = type;
            x = (int)position.X;
            y = (int)position.Y;
            //_luminosity = 6;
            if (type != "air")
            {
                _isBreakable = true;
                _isSetable = true;
                _isAir = false;
            }
            else
            {
                _isBreakable = false;
                _isSetable = false;
                _isAir = true;
            }
        }

        public void Reload(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_type);
            _position = new Vector2(x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, new Color(_luminosity, _luminosity, _luminosity, 255), 0f, Vector2.Zero, _scale / 64, SpriteEffects.None, 0f);
        }
    
        public bool IsSetable
        {
            get { return _isSetable; }
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
           /* if (newTexture == "ladder")*/ _type = newTexture;
            if (newTexture != "air")
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

        public int Luminosity
        {
            get { return _luminosity / 36; }
        }

        public void ChangeLuminosity(int luminosity)
        {
            if (luminosity < 0) luminosity = 0;
            _luminosity = luminosity * 36;
        }

    }
}

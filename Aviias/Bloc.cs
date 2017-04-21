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
    class Bloc
    {
        public Vector2 _position;
        public Texture2D _texture;
        public float _scale;
        public int _type;

        public Bloc(Vector2 position, float scale, int type, ContentManager content)
        {
            _position = position;

            if (type == 1) _texture = content.Load<Texture2D>("dirt");
            else if (type == 2) _texture = content.Load<Texture2D>("grass_side");
            else if (type == 3) _texture = content.Load<Texture2D>("bedrock");
            else if (type == 4) _texture = content.Load<Texture2D>("stone");
            else _texture = content.Load<Texture2D>("air");

            _scale = scale;
            _type = type;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
        }

   /*     public double Height => _texture.Height * _scale;

        public double Width => _texture.Width * _scale;*/

        public int Type => _type;
    }
}

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
        Vector2 _position;
        public Texture2D _texture;
        float _scale;
        string _type;

        public Bloc(Vector2 position, float scale, string type, ContentManager content)
        {
            _position = position;
            _texture = content.Load<Texture2D>(type);
            _scale = scale;
            _type = type;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, _scale / 64, SpriteEffects.None, 0f);
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
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias.IA
{
    [Serializable]
    public class Soul
    {
        Vector2 _position;
        Texture2D _texture;
        int _damages;
        int _health;

        public Soul(Vector2 position, ContentManager content, int damages, int health)
        {
            _damages = damages;
            _health = health;
            _position = position;
            _texture = content.Load<Texture2D>("soul");
        }

        public Vector2 Position => _position;
        public Texture2D Texture => _texture;
        public int Damages => _damages;
        public int Health => _health;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
        }
    }
}

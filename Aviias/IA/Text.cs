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
    public class Text
    {
        [field: NonSerialized]
        SpriteFont _msgFont;
        string[] _msgTab;

        public Text(ContentManager Content)
        {
            _msgFont = Content.Load<SpriteFont>("msg_font");
            _msgTab = new string[10];
        }

        public void Reload(ContentManager content)
        {
            _msgFont = content.Load<SpriteFont>("msg_font");
        }

        public void DisplayText(string msg, Vector2 position, SpriteBatch spriteBatch, Color color)
        {
            string _msg = msg;
            int i = 0;
            if (msg != null)
            {
                while (_msg.Length > 50)
                {
                    _msgTab[i] = _msg.Substring(0, 50);
                    _msg = _msg.Remove(0, 50);
                    i++;
                }
                _msgTab[i] = _msg;
                for (int j = 0; j < i + 1; j++)
                {
                    Vector2 _position = new Vector2(position.X, position.Y - 60 + j * 12);
                    if (_msgTab[j] != null) spriteBatch.DrawString(_msgFont, _msgTab[j], _position, color);
                }
            }
        }

        public void DisplayText(string msg, Vector2 position, SpriteBatch spriteBatch, Color color, float taille)
        {
            string _msg = msg;
            int i = 0;
            if (msg != null)
            {
                while (_msg.Length > 50)
                {
                    _msgTab[i] = _msg.Substring(0, 50);
                    _msg = _msg.Remove(0, 50);
                    i++;
                }
                _msgTab[i] = _msg;
                for (int j = 0; j < i + 1; j++)
                {
                    Vector2 _position = new Vector2(position.X, position.Y - 60 + j * 12);
                    if (_msgTab[j] != null) spriteBatch.DrawString(_msgFont, _msgTab[j], _position, color, 0.0f, new Vector2(0, 0), taille, SpriteEffects.None, 0f);
                }
            }
        }
    }
}

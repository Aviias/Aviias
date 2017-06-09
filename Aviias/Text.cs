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
    public class Text
    {
        SpriteFont msg_font;
        string[] _msgTab;

        public Text(ContentManager Content)
        {
            msg_font = Content.Load<SpriteFont>("msg_font");
            _msgTab = new string[10];
        }

        public void DisplayText(string msg, Vector2 position, SpriteBatch spriteBatch, Color color)
        {
            string _msg = msg;
            int i = 0;
            if (msg != null)
            {
                while (_msg.Length > 60)
                {
                    _msgTab[i] = _msg.Substring(0, 60);
                    _msg = _msg.Remove(0, 60);
                    i++;
                }
                _msgTab[i] = _msg;
                for (int j = 0; j < i + 1; j++)
                {
                    Vector2 _position = new Vector2(position.X, position.Y - 60 + j * 12);
                    if (_msgTab[j] != null) spriteBatch.DrawString(msg_font, _msgTab[j], _position, color);
                }
            }
        }
    }
}

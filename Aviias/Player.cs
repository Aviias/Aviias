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
    public class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;
        public bool Active;
        public int Health;
        Text text;
        public bool _displayPos;
        string _str;
        List<Quest> _activeQuest;
        List<Ressource> _inventory;

        public int Width
        {
            get { return PlayerTexture.Width; }
        }

        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        public float X
        {
            get { return Position.X; }
        }

        public float Y
        {
            get { return Position.Y; }
        }

        public void Initialize(Texture2D texture, Vector2 position, ContentManager content)
        {
            PlayerTexture = texture;
            Position = position;
            Active = true;
            Health = 100;
            text = new Aviias.Text(content);
            _displayPos = true;
        }

        public Vector2 PlayerPosition
        {
            get { return Position; }
        }

        public void AddQuest(Quest quest)
        {
            _activeQuest.Add(quest);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);

              if (_displayPos) text.DisplayText((Position.X + " - " + Position.Y), new Vector2(Position.X, Position.Y - 30), spriteBatch, Color.Red);
            // if (_displayPos) text.DisplayText(((int)Position.X/64 + " - " + (int)Position.Y/64), new Vector2(Position.X, Position.Y - 50), spriteBatch);
            // if (_displayPos) text.DisplayText(("Si la memoire est a la tete ce que le passe, peut-on y acceder a six"), new Vector2(Position.X, Position.Y - 50), spriteBatch, Color.Black);
            if (_displayPos) text.DisplayText(_str, new Vector2(Position.X, Position.Y - 50), spriteBatch, Color.Black);
        }

        public void AddStr(string str)
        {
             _str += str;
        }

    }
}

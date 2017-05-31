using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Timers;

namespace Aviias
{
    class Monster : IMonster
    {
        Text text;
        Player _ctx;
        int _id;
        int _health;
        float _speed;
        Vector2 _pos;
        bool _isDie;
        readonly double _regenerationRate;
        int _damageDealing;
        int _resistance;
        Texture2D _texture;
        float _monsterTimer = 2;         
        const float _monsterTIMER = 2;

        Random rnd = new Random();

        public Monster(int health, float speed, double regenerationRate, int damageDealing, int resistance, ContentManager content, Texture2D texture, Vector2 pos)
        {
            _id = rnd.Next(0, int.MaxValue);
            _health = health;
            _speed = speed;
            _isDie = false;
            _regenerationRate = regenerationRate;
            _damageDealing = damageDealing;
            _resistance = resistance;
            text = new Aviias.Text(content);
            _pos = pos;
            _texture = texture;
        }

        public int Width
        {
            get { return _texture.Width; }
        }

        public int Height
        {
            get { return _texture.Height; }
        }

        public float moveSpeed
        {
            get { return _speed; }
        }

        public int ID
        {
            get { return _id; }
        }

        public int Health
        {
            get { return _health; }
        }

        public Vector2 MonsterPosition
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public bool IsDie
        {
            get { return _isDie; }
            set { _isDie = value; }
        }

        public int Damage
        {
            get { return _damageDealing; }
        }

        public void GetDamage(int damage)
        {
            int newHealth = _health - (damage - _resistance);
            if (newHealth <= 0)
            {
                _isDie = true;
            } else
            {
                _health = newHealth;
            }
            
        }

        public float posX
        {
            get { return _pos.X; }
            set { _pos.X = value; }
        }

        public float posY
        {
            get { return _pos.Y; }
            set { _pos.Y = value; }
        }

        public void MoveOnPlayer(Player player)
        {
            float alpha = (float)Math.Atan2((player.Y - posY), (player.X - posX));
            Vector2 direction = AngleToVector(alpha);
            Vector2 move = new Vector2(direction.X * _speed, direction.Y * _speed);
            _pos = new Vector2(posX + move.X, posY + move.Y);
        }

        public void Fight(Player player, GameTime gametime)
        {
            Rectangle playerRect;
            Rectangle monsterRect;
            float elapsed = (float)gametime.ElapsedGameTime.TotalMilliseconds / 1000;
            _monsterTimer -= elapsed;

            playerRect = new Rectangle((int)player.X, (int)player.Y, player.Width, player.Height);
            monsterRect = new Rectangle((int)posX, (int)posY, Width, Height);
            if (playerRect.Intersects(monsterRect))
            {
                if (monsterRect.Left <= playerRect.Right || monsterRect.Right == playerRect.Left || monsterRect.Top <= playerRect.Bottom || monsterRect.Bottom == playerRect.Top)
                {
                    if (_monsterTimer < 1)
                    {
                        player.GetDamage(Damage);
                        _monsterTimer = _monsterTIMER;
                    }
                }
            }
        }

        
        internal void Update(Player player, GameTime gametime)
        {
            MoveOnPlayer(player);
            Fight(player, gametime);                                  
        }

        Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            text.DisplayText(("Life : " + _health), new Vector2(_pos.X + 60, _pos.Y - 30), spriteBatch, Color.Orange);
        }
    }
}

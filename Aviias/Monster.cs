using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aviias
{
    class Monster
    {
        Player _ctx;
        int _id;
        int _health;
        float _speed;
        public Vector2 _pos;
        bool _isDie;
        readonly double _regenerationRate;
        int _damageDealing;
        int _resistance;
        Texture2D _texture;

        Random rnd = new Random();

        public Monster(int health, float speed, double regenerationRate, int damageDealing, int resistance)
        {
            _id = rnd.Next(0, int.MaxValue);
            _health = health;
            _speed = speed;
           
            _isDie = false;
            _regenerationRate = regenerationRate;
            _damageDealing = damageDealing;
            _resistance = resistance;
            
        }

        public int Width
        {
            get { return _texture.Width; }
        }

        public int Height
        {
            get { return _texture.Height; }
        }

        public void Initialize(Texture2D texture, Vector2 pos)
        {
            _pos = pos;
            _texture = texture;
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
        }

        public bool IsDie
        {
            get { return _isDie; }
        }

        public void GetDamage(int damage)
        {
            _health = _health - ( damage - _resistance);
        }

        public float posX
        {
            get { return _pos.X; }
        }

        public float posY
        {
            get { return _pos.Y; }
        }


        internal void Update(Player player)
        {
            float alpha = (float)Math.Atan2((player.Y - posY), (player.X - posX));
            Vector2 direction = AngleToVector(alpha);
            Vector2 move = new Vector2(direction.X * _speed, direction.Y * _speed);
            _pos = new Vector2(posX + move.X, posY + move.Y);
            
        }

        Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _pos, null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
        }
    }
}

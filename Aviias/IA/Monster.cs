using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Timers;
using Aviias.IA;

namespace Aviias
{
    [Serializable]
    class Monster : Physics
    {
        Text text;
        int _id;
        int _health;
        float _speed;
        [field: NonSerialized]
        Vector2 _pos;
        bool _isDie;
        double _regenerationRate;
        int _damageDealing;
        int _resistance;
        [field: NonSerialized]
        Texture2D _texture;
        Timer monsterTimer = new Timer(2f);

        Random rnd = new Random();

        public Monster(int health, float speed, double regenerationRate, int damageDealing, int resistance, ContentManager content, Texture2D texture, Vector2 pos)
            : base(false,4,-12)
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

        public Text Txt
        {
            get { return text; }
            set { text = value; }
        }

        public int Height
        {
            get { return _texture.Height; }
            
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public float moveSpeed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Resistance
        {
            get { return _resistance; }
            set { _resistance = value; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public double RegenarationRate
        {
            get { return _regenerationRate; }
            set { _regenerationRate = value; }
        }

        public Vector2 MonsterPosition
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public float Y
        {
            get { return _pos.Y; }
            set { _pos.Y = value; }
        }

        public bool IsDie
        {
            get { return _isDie; }
            set { _isDie = value; }
        }

        public int Damage
        {
            get { return _damageDealing; }
            set { _damageDealing = value; }
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
            Vector2 move = new Vector2(direction.X * _speed, /*direction.Y * _speed*/0);
            _pos = new Vector2(posX + move.X, posY /*+ move.Y*/);
        }

        public void Fight(Player player, GameTime gametime)
        {
            Rectangle playerRect;
            Rectangle monsterRect;
            monsterTimer.Decrem(gametime);

            playerRect = new Rectangle((int)player.X, (int)player.Y, player.Width, player.Height);
            monsterRect = new Rectangle((int)posX, (int)posY, Width, Height);
            if (playerRect.Intersects(monsterRect))
            {
                if (monsterRect.Left <= playerRect.Right || monsterRect.Right == playerRect.Left || monsterRect.Top <= playerRect.Bottom || monsterRect.Bottom == playerRect.Top)
                {
                    if (monsterTimer.IsDown() && player.IsStopDamage == false)
                    {
                        player.GetDamage(Damage);
                        monsterTimer.ReInit();
                    }
                }
            }
        }

        
        internal void Update(Player player, GameTime gametime)
        {
            UpdatePhysics(Game1.map, this);
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

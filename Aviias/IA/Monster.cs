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
   /*     [field: NonSerialized]
        Vector2 _pos;*/
        bool _isDie;
        double _regenerationRate;
        int _damageDealing;
        int _baseHealth;
        int _baseDamageDealing;
        int _resistance;
        [field: NonSerialized]
        Texture2D _texture;
        Timer monsterTimer = new Timer(2f);
        bool _reactToLight;
        bool _bonusLightGiven;
        List<Soul> _souls = new List<Soul>(16);
        public Timer _healthRegenerationTimer = new Timer(4f);

        Random rnd = new Random();

        public Monster(int health, float speed, double regenerationRate, int damageDealing, int resistance, ContentManager content, Texture2D texture, Vector2 pos)
            : base(false,4,-12, pos)
        {
            _id = rnd.Next(0, int.MaxValue);
            _health = health;
            _speed = speed;
            _isDie = false;
            _regenerationRate = regenerationRate;
            _damageDealing = damageDealing;
            _resistance = resistance;
            text = new Aviias.Text(content);
         //   _pos = pos;
            _texture = texture;
            _baseDamageDealing = damageDealing;
            _baseHealth = health;
        }

        public int BaseHealth => _baseHealth;
        public int BaseDamage => _baseDamageDealing;

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
            _healthRegenerationTimer.ReInit();
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

        public void ReactToLight()
        {
            if (Game1.map._blocs[(int)MonsterPosition.X / 16, (int)MonsterPosition.Y / 16].Luminosity < 3) _reactToLight = true;
            else
            {
                if (_bonusLightGiven)
                {
                    _bonusLightGiven = false;
                    _health -= 25;
                }
                _reactToLight = false;
            }

            if (_reactToLight && !_bonusLightGiven)
            {
                _health += 25;
                _bonusLightGiven = true;
            }
        }

        public bool CheckSoul(List<Soul> souls)
        {
            foreach(Soul soul in souls)
            {
                Rectangle soulRect = new Rectangle((int)soul.Position.X, (int)soul.Position.Y, soul.Texture.Width, soul.Texture.Height);
                Rectangle monsterRect = new Rectangle((int)MonsterPosition.X, (int)MonsterPosition.Y, Texture.Width, Texture.Height);
                if (soulRect.Intersects(monsterRect)) return true;
            }
            return false;
        }

        public bool EatSoul(List<Soul> souls)
        {
            foreach (Soul soul in souls)
            {
                Rectangle soulRect = new Rectangle((int)soul.Position.X, (int)soul.Position.Y, soul.Texture.Width, soul.Texture.Height);
                Rectangle monsterRect = new Rectangle((int)MonsterPosition.X, (int)MonsterPosition.Y, Texture.Width, Texture.Height);
                if (soulRect.Intersects(monsterRect))
                {
                    _health += soul.Health;
                    _damageDealing += soul.Damages;
                    return true;
                }
                    //return eaten soul
            }
            return false;
        }

        public void ActualizeHealthRegeneration(GameTime gametime)
        {
            _healthRegenerationTimer.Decrem(gametime);
            if (_healthRegenerationTimer.IsDown() && _health < _baseHealth)
            {
                _health++;
            }

        }

        public void MoveOnPlayer(Player player)
        {
            float alpha = (float)Math.Atan2((player.Y - posY), (player.X - posX));
            Vector2 direction = AngleToVector(alpha);
            Vector2 move = new Vector2(direction.X * _speed, /*direction.Y * _speed*/0);
            //  if (move.X )
            if (GetCollisionSide(GetBlocsAround(Game1.map), _texture).Contains(1))
            {

            }
            else _pos = new Vector2(posX + move.X, posY /*+ move.Y*/);
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
            UpdatePhysics(Game1.map, _texture);
            MoveOnPlayer(player);
            Fight(player, gametime);
            ReactToLight();
            ActualizeHealthRegeneration(gametime);                           
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

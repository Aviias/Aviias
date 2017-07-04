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
        int nextAction;
        int _health;
        float _speed;
        bool _isDie;
        double _regenerationRate;
        int _damageDealing;
        int _baseHealth;
        float _baseEnergy;
        int _baseDamageDealing;
        int _resistance;
        [field: NonSerialized]
        Texture2D _texture;
        Timer monsterTimer = new Timer(2f);
        bool _reactToLight;
        bool _bonusLightGiven;
        List<Soul> _souls = new List<Soul>(16);
        public Timer _healthRegenerationTimer = new Timer(4f);
        Timer _energyRegenTimer = new Timer(2f);
        float _energy;
        bool _collisions;
        float _yVelocity;
        Timer EngeryDamageTimer = new Timer(1f);
        Timer JumpTimer = new Timer(50f);
        public int[] proba;
        public int[] _points;
        Timer stopDamageTimer = new Timer(4f);
        Timer stopDamageCDTimer = new Timer(5f);
        bool _isStopDamage;
        Genetic genetic = new Genetic();

        public Monster(int health, float speed, double regenerationRate, int damageDealing, int resistance, ContentManager content, Texture2D texture, Vector2 pos, float energy)
            : base(false,4,-10, pos)
        {
            _id = Game1.random.Next(0, int.MaxValue);
            _health = health;
            _speed = speed;
            _isDie = false;
            _regenerationRate = regenerationRate;
            _damageDealing = damageDealing;
            _resistance = resistance;
            text = new Text(content);
            _texture = texture;
            _baseDamageDealing = damageDealing;
            _baseHealth = health;
            _baseEnergy = energy;
            _energy = energy;
            proba = new int[6];
            nextAction = 0;
            _points = new int[5] { 0, 0, 0, 0, 0 };
            _isStopDamage = false;
            genetic.AddMonster(this);
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

        public bool IsStopDamage
        {
            get { return _isStopDamage; }
            set { _isStopDamage = value; }
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

        public float Energy
        {
            get { return _energy; }
            set { _energy = value; }
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
                    _points[3] += 10;
                    return true;
                }
                    //return eaten soul
            }
            return false;
        }

        public void ActualizeEnergieRegeneration(GameTime gametime)
        {
            _energyRegenTimer.Decrem(gametime);
            if (_energyRegenTimer.IsDown() && _energy < _baseEnergy)
            {
                _energy += 1.5f;
            }
        }

        public void ActualizeHealthRegeneration(GameTime gametime)
        {
            _healthRegenerationTimer.Decrem(gametime);
            if (_healthRegenerationTimer.IsDown() && _health < _baseHealth)
            {
                _health++;
            }

        }

        public void Flight(Player player, Map map, GameTime gametime)
        {
            JumpTimer.Decrem(gametime);
            Vector2 move;
            float alpha = (float)Math.Atan2((player.Y - posY), (player.X - posX));
            Vector2 direction = AngleToVector(alpha);
            if (GetCollisionSide(GetBlocsAround(map)).Contains(3) && JumpTimer.IsDown() && (GetCollisionSide(GetBlocsAround(map)).Contains(2) || GetCollisionSide(GetBlocsAround(map)).Contains(1)))
            {
                Jump(map, this.Texture);
                JumpTimer.ReInit();
            }

            move = new Vector2(direction.X * _speed * (-1), /*direction.Y * _speed*/0);
            
            
            _pos = new Vector2(posX + move.X, posY /*+ move.Y*/);
        }

        public void MoveOnPlayer(Player player, Map map, GameTime gametime)
        {
            JumpTimer.Decrem(gametime);
            Vector2 move;
            float alpha = (float)Math.Atan2((player.Y - posY), (player.X - posX));
            Vector2 direction = AngleToVector(alpha);
            if (GetCollisionSide(GetBlocsAround(map)).Contains(3) && JumpTimer.IsDown() && (GetCollisionSide(GetBlocsAround(map)).Contains(2) || GetCollisionSide(GetBlocsAround(map)).Contains(1)))
            {
                Jump(map,this.Texture);
                JumpTimer.ReInit();
            }
            move = new Vector2(direction.X * _speed, /*direction.Y * _speed*/0);

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
                        if (Energy < (Energy * (0.75)))
                        {
                            player.GetDamage(Damage);
                            _points[1] += Damage;
                            monsterTimer.ReInit();
                            Energy = Energy - 0.8f;
                        }
                        else
                        {
                            player.GetDamage(Damage + (Damage/2));
                            monsterTimer.ReInit();
                            Energy = Energy - 15f;
                        }
                        
                    }
                }
            }
        }

        public List<int> GetCollisionSide(List<Bloc> _blocs)
        {
            List<int> result = new List<int>(16);
            Rectangle monsterRect;
            Rectangle monsterRect2;
            monsterRect = new Rectangle((int)MonsterPosition.X, (int)MonsterPosition.Y, Texture.Width, Texture.Height);
            monsterRect2 = new Rectangle((int)MonsterPosition.X, (int)MonsterPosition.Y + 1, Texture.Width, Texture.Height);

            Rectangle rectTest = new Rectangle((int)MonsterPosition.X, (int)MonsterPosition.Y - 10, Texture.Width, Texture.Height);

            for (int i = 0; i < _blocs.Count; i++)
            {
                if (_blocs[i] != null)
                {
                    Rectangle blocRect;
                    blocRect = new Rectangle((int)_blocs[i].posX, (int)_blocs[i].posY, _blocs[i].Width, _blocs[i].Height);
                    if (monsterRect.Intersects(blocRect))
                    {
                        if (monsterRect.Bottom > blocRect.Top && monsterRect.Bottom < blocRect.Bottom)
                        {
                            result.Add(3);
                            if (!rectTest.Intersects(blocRect)) Y -= 1;
                        }
                        if (monsterRect.Top < blocRect.Bottom && monsterRect.Top > blocRect.Top) result.Add(4);
                        if (rectTest.Left < blocRect.Right && rectTest.Left > blocRect.Left) result.Add(2);
                        if (rectTest.Right > blocRect.Left && rectTest.Right < blocRect.Right) result.Add(1);
                        _collisions = true;
                    }
                    if (monsterRect2.Intersects(blocRect))
                    {
                        if (monsterRect2.Bottom > blocRect.Top && monsterRect2.Bottom < blocRect.Bottom)
                        {
                            _yVelocity = 0;
                            result.Add(3);
                        }
                    }
                }
            }
            return result;
        }



        internal List<Bloc> GetBlocsAround(Map map)
        {
            int nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);

            for (int a = (int)(MonsterPosition.Y / 16); a < (MonsterPosition.Y / 16) + 8; a++)
            {
                for (int b = (int)(MonsterPosition.X / 16); b < (MonsterPosition.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && a < map._worldHeight && b < map._worldWidth && map._blocs[b, a] != null && map._blocs[b, a].Type != "air" && map._blocs[b, a].Type != "ladder")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        nbBlocs++;
                    }
                }
            }

            return _blocs;
        }

        public void Dodge()
        {
            if (stopDamageCDTimer.IsDown())
            {
                IsStopDamage = true;
                stopDamageCDTimer.ReInit();
            }
        }

        internal void ChooseAction()
        {
            int prob = Game1.random.Next(1, (proba[0] + proba[1] + proba[2] + proba[3] + proba[4] + proba[5]));
            if (prob <= proba[0]) nextAction = 0;           //Move on player
            else if (prob <= proba[1]) nextAction = 1;      //Flight
            else if (prob <= proba[2]) nextAction = 2;      //Fight
            else if (prob <= proba[3]) nextAction = 3;      //Block
            else if (prob <= proba[4]) nextAction = 4;      //GetOrb / EatMonster
            else nextAction = 5;                            //Do nothing
        }

        internal void DoSomething(Player player, Map map, GameTime gametime)
        {
            switch(nextAction)
            {
                case 0:
                    MoveOnPlayer(player, map, gametime);
                    break;
                case 1:
                    Flight(player, map, gametime);
                    break;
                case 2:
                    Fight(player, gametime);
                    break;
                case 3:
                    MoveOnPlayer(player, map, gametime);
                    break;
                case 4:
                    MoveOnPlayer(player, map, gametime);
                    break;
                case 5:
                    MoveOnPlayer(player, map, gametime);
                    break;
            }
        }

        internal void UpdatePoints()
        {
            // 0 distance
            if (Math.Abs((Math.Abs((int)MonsterPosition.X - (int)Game1.player.Position.X) - Math.Abs((int)MonsterPosition.Y - (int)Game1.player.Position.Y))) < 40) _points[0] += 3;
            else if (Math.Abs((Math.Abs((int)MonsterPosition.X - (int)Game1.player.Position.X) - Math.Abs((int)MonsterPosition.Y - (int)Game1.player.Position.Y))) < 100) _points[0] += 1;

            // 1 dommages infligés
            // 2 dommages bloqués / 2

            // 3 orbes ou monstres gobés
            // 4
        }

        internal void Update(Player player, GameTime gametime, Map map)
        {
            EngeryDamageTimer.Decrem(gametime);
            stopDamageCDTimer.Decrem(gametime);
            stopDamageTimer.Decrem(gametime);

            if (stopDamageTimer.IsDown())
            {
                IsStopDamage = false;
                stopDamageTimer.ReInit();
            }

            if (_health >= 50)
            {
                MoveOnPlayer(player, map, gametime);
                Energy = Energy - 0.2f;
            }
            else
            {
              Flight(player, map, gametime);
              Energy = Energy - 0.2f;
                               
            }


            Fight(player, gametime);

            if (Energy < 0f && EngeryDamageTimer.IsDown())
            {
                this.GetDamage(1);
                EngeryDamageTimer.ReInit();
            }

            UpdatePhysics(map, Texture);
                
            ReactToLight();
            ActualizeHealthRegeneration(gametime);
            ActualizeEnergieRegeneration(gametime);                       
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

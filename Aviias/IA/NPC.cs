using Aviias.IA;
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
    class NPC : Physics
    {
        [field: NonSerialized]
        Texture2D _texture;
        [field: NonSerialized]
        public bool _isQuestActive;
        public int _nbQuest;
        Text _text;
        Quest _questActive;
        public bool _isTalking;
        static int _id;
        int trueid = 0;
        string _textureName;
        float x;
        float y;
        bool _isMoving;
        Timer _moving;
        int _direction;
        Timer _movingCooldown;
        

        public NPC(ContentManager content, string texture, SpriteBatch spriteBatch, Vector2 position, int nbQuest)
            :base(false, 4, -10, position)
        {
            _texture = content.Load<Texture2D>(texture);
            _pos = position;
            _text = new Text(content);
            _nbQuest = nbQuest;
            _id++;
            trueid = _id;
            x = position.X;
            y = position.Y;
            _textureName = texture;
            _isMoving = false;
            _moving = new Timer(0f);
            _direction = 0;
            _isQuestActive = false;
            _isTalking = false;
            _movingCooldown = new Timer(0f);
        }

        public void Interact(Player player)
        {
            foreach(Quest quest in player._activeQuest)
            {
                if (quest.Type == 1 && quest.EndNpc == trueid)
                {
                    quest.GetReward(player);
                    quest._startNpc._nbQuest--;
                    quest._startNpc._isQuestActive = false;
                    player.RemoveQuest(_questActive);
                    quest._startNpc._isTalking = false; ;
                    player._activeQuest.Remove(quest);
                    break;
                }

              /*  if (quest.CheckGoal(player, trueid))
                {
                    player.AddInventory(800, "dirt");
                    player._activeQuest.Remove(quest);
                    break;
                }*/
                if (quest.Type == 1)
                {

                }
            }

            if (_isQuestActive)
            {
                if (_questActive.CheckGoal(player, _id)) EndQuest(player);
            }
            else if (_isTalking == false)
            {
                GiveQuest(player);
                _isTalking = true;
            }
        }

        public void Update(GameTime gametime, SpriteBatch spriteBatch)
        {

            UpdatePhysics(Game1.map, _texture);
            RandomMove(gametime);
        }

        public void GraphicUpdate(SpriteBatch spriteBatch)
        {
            if (_isTalking) Talk(_questActive, spriteBatch);
        }

        public void GiveQuest(Player player)
        {
            //generer quete

            if (!_isQuestActive && _nbQuest > 0)
            {
                int idEnd = trueid == 1 ? 2 : 1;
                _questActive = new Quest(Game1.random.Next(0, 1), _id, idEnd, this);
                player.AddQuest(_questActive);
                //   Talk(_questActive, _spriteBatch);
                _isQuestActive = true;
                _nbQuest--;
                _isTalking = true;
            }
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _pos, null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
            GraphicUpdate(spriteBatch);
        }

        public void EndQuest(Player player)
        {
            foreach (KeyValuePair<string, int> entry in _questActive._goal)
            {
               // player.DecreaseInventory(entry.Value, entry.Key);
            }
            _questActive.GetReward(player);
            _nbQuest--;
            _isQuestActive = false;
            player.RemoveQuest(_questActive);
            _isTalking = false;
            if (player._success._quest != 5) player._success._quest++;
        }

        public void Talk(Quest quest, SpriteBatch spriteBatch)
        {
            _text.DisplayText(quest.Spitch, new Vector2(_pos.X, _pos.Y - 50), spriteBatch, Color.Black);
        }

        public void Reload(ContentManager content)
        {
            _pos = new Vector2(x, y);
            _texture = content.Load<Texture2D>(_textureName);
        }

        public void RandomMove(GameTime gametime)
        {
            if (_isMoving && !_moving.IsDown())
            {
                if (_direction == 0) _pos.X += 1;
                else _pos.X -= 1;
                _moving.Decrem(gametime);
                if (_moving.IsDown())
                {
                    _isMoving = false;
                    _movingCooldown = new Timer(10);
                }
            }

            _movingCooldown.Decrem(gametime);

            if (!_isMoving && _movingCooldown.IsDown())
            {
                int rand = Game1.random.Next(1, 100);
                if (rand <= 10)
                {
                    _moving = new Timer(Game1.random.Next(2, 4));
                    _direction = Game1.random.Next(0, 2);
                    _isMoving = true;
                }
            }
        }

        public Vector2 Position => _pos;
        public int Id => trueid;
    }
}

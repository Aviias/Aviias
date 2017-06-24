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
    public class NPC
    {
        [field: NonSerialized]
        Texture2D _texture;
        [field: NonSerialized]
        Vector2 _position;
        public bool _isQuestActive;
        public int _nbQuest;
        Text _text;
        Quest _questActive;
        [field: NonSerialized]
        SpriteBatch _spriteBatch;
        public bool _isTalking;
        static int _id;
        int trueid = 0;
        Random rand = new Random();
        string _textureName;
        float x;
        float y;
        

        public NPC(ContentManager content, string texture, SpriteBatch spriteBatch, Vector2 position, int nbQuest)
        {
            _texture = content.Load<Texture2D>(texture);
            _position = position;
            _text = new Text(content);
            _spriteBatch = spriteBatch;
            _nbQuest = nbQuest;
            _id++;
            trueid = _id;
            x = position.X;
            y = position.Y;
            _textureName = texture;
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
            else if (_isTalking == false) GiveQuest(player);
         //   _isTalking = true;
        }

        public void Update()
        {
            if (_isTalking) Talk(_questActive, _spriteBatch);
        }

        public void GiveQuest(Player player)
        {
            //generer quete

            if (!_isQuestActive && _nbQuest > 0)
            {
                int idEnd = trueid == 1 ? 2 : 1;
                _questActive = new Quest(1, _id, idEnd, this);
                player.AddQuest(_questActive);
                //   Talk(_questActive, _spriteBatch);
                _isQuestActive = true;
                _nbQuest--;
                _isTalking = true;
            }
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
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
        }

        public void Talk(Quest quest, SpriteBatch spriteBatch)
        {
            _text.DisplayText(quest.Spitch, new Vector2(_position.X, _position.Y - 50), spriteBatch, Color.Black);
        }

        public void Reload(ContentManager content)
        {
            _position = new Vector2(x, y);
            _texture = content.Load<Texture2D>(_textureName);
        }

        public Vector2 Position => _position;

        public int Id => trueid;
    }
}

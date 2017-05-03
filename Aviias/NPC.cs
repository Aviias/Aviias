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
    public class NPC
    {
        Texture2D _texture;
        Vector2 _position;
        bool _isQuestActive;
        int _nbQuest;
        Text _text;
        Quest _questActive;
        SpriteBatch _spriteBatch;
        public bool _isTalking;
        int _id;

        public NPC(ContentManager content, string texture, SpriteBatch spriteBatch)
        {
            _texture = content.Load<Texture2D>(texture);
            _position = new Vector2(50, 50);
            _text = new Text(content);
            _spriteBatch = spriteBatch;
        }

        public void Interact(Player player)
        {
            if (_isQuestActive) EndQuest();
            else GiveQuest(player);
            _isTalking = true;
        }

        public void GiveQuest(Player player)
        {
            //generer quete

            if (!_isQuestActive && _nbQuest > 0)
            {
                _questActive = new Quest();
                player.AddQuest(_questActive);
                Talk(_questActive, _spriteBatch);
            }
            _isQuestActive = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
        }

        public void EndQuest()
        {
            //check conditions quete
            _nbQuest--;
            _isQuestActive = false;
        }

        public void Talk(Quest quest, SpriteBatch spriteBatch)
        {
            _text.DisplayText(quest.Spitch, new Vector2(_position.X, _position.Y - 50), spriteBatch, Color.Black);
        }
    }
}

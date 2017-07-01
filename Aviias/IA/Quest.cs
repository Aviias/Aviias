using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    [Serializable]
    class Quest
    {
        Dictionary <string, int> _reward;
        int _type;
        int _idStartNpc;
        public NPC _startNpc;
        int _idEndNpc;
        string _spitch;
        int _id;
        internal Dictionary<string, int> _goal;

        public Quest(int type, int idStartNpc, int idEndNpc, NPC startNPC)
        {
            _type = type;
            _id = ++_id;
            _idStartNpc = idStartNpc;
            _idEndNpc = idEndNpc;
            _goal = new Dictionary<string, int>(4);
            _reward = new Dictionary<string, int>(2);
             _goal.Add("stone", 8);
            _goal.Add("wood_shovel", 1);
            _startNpc = startNPC;
            CreateSpitch();
            CreateReward();
        }

        internal bool CheckGoal(Player player, int npcId)
        {
            if (_type == 0)
            {
                foreach (KeyValuePair<string, int> goal in _goal)
                {
                    if (player._inv.Quantity(goal.Key) > goal.Value) return false;
                }
                return true;
            }
            else
            {
                if (_idEndNpc == npcId)
                {
                    return true;
                }
            }
            return false;
        }

        void CreateReward()
        {
            _reward.Add("heal_potion", Game1.random.Next(1, 3));
        }

        void CreateSpitch()
        {
            if (_type == 0)
            {
                string goal = "Salut toi ! ";
                goal += "Ramene moi ces ressources ";
                goal += "et ne demande pas pourquoi.\n";
                /*foreach (KeyValuePair<string, int> entry in _goal)
                {
                    goal += entry.Key + " x " + entry.Value + "\n";
                }*/
                _spitch = goal;
            }
            else
            {
                string goal = "Salut toi !\n";
                goal += "Peux-tu aller voir " + _idEndNpc;
                goal += "pour moi ?\n";
                _spitch = goal;
            }
        }

        internal void GetReward(Player player)
        {
            foreach(KeyValuePair<string, int> entry in _reward)
            {
                 player._inv.AddInventory(entry.Value, entry.Key);
            }
        }

        public string Spitch => _spitch;

        public int EndNpc => _idEndNpc;

        public int StartNpc => _idStartNpc;

        public int Type => _type;
    }
}

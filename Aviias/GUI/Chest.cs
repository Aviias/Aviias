using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    [Serializable]
    public class Chest : Bloc
    {
        Random random = new Random();
        _cell[] _content;

        public Chest(Vector2 position, float scale, string type, ContentManager content) : base (position, scale, type, content)
        {
            _content = new _cell[20];
            Fill();
        }

        void Fill()
        {
            for(int i = 0; i < random.Next(3, 9); i++)
            {
                AddChest(random.Next(2, 12), "dirt");
            }
        }

        [Serializable]
        public struct _cell
        {
            [field: NonSerialized]
            public Vector2 Position;
            public bool IsFull { get; set; }
            public string _name { get; set; }
            public int _quantity { get; set; }
            public Ressource _ressource { get; set; }
        }

        public void AddChest(int quantity, string name)
        {
            for (int i = 0; i < _content.Length; i++)
            {
                if (_content[i]._name == name && _content[i]._quantity == 0)
                {
                    _content[i].IsFull = true;
                    _content[i]._quantity += quantity; return;
                }
                else if (_content[i]._name == name)
                {
                    _content[i]._quantity += quantity; return;
                }
            }

            for (int i = 0; i < _content.Length; i++)
            {
                if (!_content[i].IsFull && _content[i]._name != name) { _content[i]._name = name; _content[i]._quantity = quantity; _content[i].IsFull = true; _content[i]._ressource = new Ressource(name); break; }
            }
        }
    }
}

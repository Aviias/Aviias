using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    class Craft
    {
        _craft[] _cellCraft;
        

        public Craft(Dictionary<int, Ressource> ressource)
        {
            _cellCraft = new _craft[40];
            for (int i = 0; i < 40; i++)
            {
                _cellCraft[i] = new _craft();
                _cellCraft[i]._name = "";
                _cellCraft[i]._quantity = -1;

            }
            AddCraft("planche", 4, Add(1, "bois"));
            AddCraft("stick", 4, Add(2, "planche"));
        }

        struct _craft
        {
            public string _name { get; set; }
            public bool IsCraftable { get; set; }
            public int _quantity { get; set; }
            public Dictionary<int, Ressource> _ressource;
        }

        public Dictionary<int,Ressource> Add(int number, string ressource)
        {
            Dictionary<int, Ressource> _ressource = new Dictionary<int, Ressource>();
            Ressource t = new Ressource(ressource);
            _ressource.Add(number, t);

            return _ressource;
        }

        public void AddCraft(string name, int quantity, Dictionary<int, Ressource> ressource)
        {
            for(int i=0; i<_cellCraft.Length; i++)
            {
                if(_cellCraft[i]._name == "")
                {
                    _cellCraft[i]._name = name;
                    _cellCraft[i]._quantity = quantity;
                    _cellCraft[i]._ressource = ressource;
                }
            }
        }


        public void IsCraftable(Inventory._cell[] inventory)
        {
            //if()
        }
    }
}

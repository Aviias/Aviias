using System.Collections.Generic;

namespace Aviias
{
    public class Craft
    {
        public _craft[] _cellCraft;

        public Craft()
        {
            _cellCraft = new _craft[40];
            for (int i = 0; i < 40; i++)
            {
                _cellCraft[i] = new _craft();
                _cellCraft[i]._name = "";
                _cellCraft[i]._quantity = -1;
                
            }
            AddCraft("oak_plank", 4, Add(1, "oak_wood"));
            AddCraft("stick", 4, Add(2, "oak_plank"));
        }

        public struct _craft
        {
            public string _name { get; set; }
            public bool IsCraftable { get; set; }
            public int _quantity { get; set; }
            public Dictionary<int, Ressource> _ressource { get; set; }
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
                    break;
                }
            }
        }
        public void IsCraftable(Inventory._cell[] inventory)
        {
            for(int i=0; i<_cellCraft.Length; i++)
            {
                if (_cellCraft[i]._name != "")
                {
                    foreach (KeyValuePair<int, Ressource> element in _cellCraft[i]._ressource)
                    {
                        int count = 0;
                        for (int j = 0; j < inventory.Length; j++)
                        {
                            if (element.Value.Name == inventory[j]._ressource.Name && element.Key <= inventory[j]._quantity)
                            {
                                count++;
                            }
                        }
                        if (count == _cellCraft[i]._ressource.Count)
                        {
                            _cellCraft[i].IsCraftable = true;
                        }
                        else
                        {
                            _cellCraft[i].IsCraftable = false;
                        }
                    }
                }
            }
        }
    }
}

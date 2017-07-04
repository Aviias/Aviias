using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Aviias
{
    [Serializable]
    public class Craft
    {
        public _craft[] _cellCraft;

        public struct _craft
        {
            public string _name { get; set; }
            public bool IsCraftable { get; set; }
            public int _quantity { get; set; }
            public Vector2 _position { get; set; }
            public int _width { get; set; }
            public int _height { get; set; }
            public Dictionary<int, string> _ressource { get; set; }
        }

        public Craft()
        {
            _cellCraft = new _craft[40];
            for (int i = 0; i < 40; i++)
            {
                _cellCraft[i] = new _craft();
                _cellCraft[i]._name = "";
                _cellCraft[i]._quantity = -1;
                _cellCraft[i]._width = 70;
                _cellCraft[i]._height = 69;
                _cellCraft[i].IsCraftable = false;
                _cellCraft[i]._ressource = new Dictionary<int, string>();

            }
            AddCraft("oak_plank", 4, 1, "oak_wood");
            AddCraft("stick", 4, 2, "oak_plank");
            AddCraft("cobblestone", 1, 1, "stone");
            AddCraft("door_wood", 1, 6, "oak_plank");
            AddCraft("coal", 1, 1, "coal_ore");
            AddCraft("iron_ingot", 1, 1, "iron_ore");
            AddCraft("gold_ingot", 1, 1, "gold_ore");
            AddCraft("diamond", 1, 1, "diamond_ore");
            // Wood
            AddCraft("wood_shovel", 1, 2, "stick", 1, "oak_plank");
            AddCraft("wood_axe", 1, 2, "stick", 3, "oak_plank");
            AddCraft("wood_pickaxe", 1, 2, "stick", 3, "oak_plank");
            AddCraft("wood_sword", 1, 1, "stick", 2, "oak_plank");
            //Cobble
            AddCraft("stone_shovel", 1, 2, "stick", 1, "cobblestone");
            AddCraft("stone_axe", 1, 2, "stick", 3, "cobblestone");
            AddCraft("stone_pickaxe", 1, 2, "stick", 3, "cobblestone");
            AddCraft("stone_sword", 1, 1, "stick", 2, "cobblestone");
            //Iron
            AddCraft("iron_shovel", 1, 2, "stick", 1, "iron_ingot");
            AddCraft("iron_axe", 1, 2, "stick", 3, "iron_ingot");
            AddCraft("iron_pickaxe", 1, 2, "stick", 3, "iron_ingot");
            AddCraft("iron_sword", 1, 1, "stick", 2, "iron_ingot");
            //Gold
            AddCraft("gold_shovel", 1, 2, "stick", 1, "gold_ingot");
            AddCraft("gold_axe", 1, 2, "stick", 3, "gold_ingot");
            AddCraft("gold_pickaxe", 1, 2, "stick", 3, "gold_ingot");
            AddCraft("gold_sword", 1, 1, "stick", 2, "gold_ingot");
            //Diamond
            AddCraft("diamond_shovel", 1, 2, "stick", 1, "diamond");
            AddCraft("diamond_axe", 1, 2, "stick", 3, "diamond");
            AddCraft("diamond_pickaxe", 1, 2, "stick", 3, "diamond");
            AddCraft("diamond_sword", 1, 1, "stick", 2, "diamond");
            //AddCraft("torche", 1, 1, "stick", 1, "coal_ore");
        }

        public void AddCraft(string name, int quantity, int number, string ressource)
        {
            for (int i = 0; i < _cellCraft.Length; i++)
            {
                if (_cellCraft[i]._name == "")
                {
                    _cellCraft[i]._name = name;
                    _cellCraft[i]._quantity = quantity;
                    _cellCraft[i]._ressource.Add(number, ressource);
                    break;
                }
            }
        }

        public void AddCraft(string name, int quantity, int number, string ressource, int number2, string ressource2)
        {
            for (int i = 0; i < _cellCraft.Length; i++)
            {
                if (_cellCraft[i]._name == "")
                {
                    _cellCraft[i]._name = name;
                    _cellCraft[i]._quantity = quantity;
                    _cellCraft[i]._ressource.Add(number, ressource);
                    _cellCraft[i]._ressource.Add(number2, ressource2);
                    break;
                }
            }
        }


        public Vector2 Position
        {
            get { return Position; }
        }

        public void IsCraftable(Inventory._cell[] inventory)
        {
            for (int i = 0; i < _cellCraft.Length; i++)
            {
                if (_cellCraft[i]._name != "")
                {
                    int count = 0;
                    for (int j = 0; j < _cellCraft.Length; j++)
                    {
                        foreach (KeyValuePair<int, string> element in _cellCraft[i]._ressource)
                        {
                            if (inventory[j]._ressource.Name == element.Value && element.Key <= inventory[j]._quantity)
                            {
                                count++;
                            }
                        }
                    }
                    if (count == _cellCraft[i]._ressource.Count)
                    {
                        _cellCraft[i].IsCraftable = true;
                    }
                    else
                    {
                        _cellCraft[i].IsCraftable = false;
                        _cellCraft[i]._position = new Vector2(100, 100);
                    }
                }
            }
        }
    }
}


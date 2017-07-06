using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Aviias
{
    [Serializable]
    public class Craft
    {
        public List<_craft> _cellCraft;

        [Serializable]
        public class _craft
        {
            public string _name { get; set; }
            public bool IsCraftable { get; set; }
            public int _quantity { get; set; }
            [field: NonSerialized]
            public Vector2 _position; //{ get; set; }
            public int _width { get; set; }
            public int _height { get; set; }
            public Dictionary<string, int> _ressource { get; set; }
        }

        public Craft()
        {
            _cellCraft = new List<_craft>();
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
            AddCraft("torche", 1, 1, "stick", 1, "coal_ore");
            AddCraft("apple_golden", 1, 8, "gold_ingot");
        }

        public void AddCraft(string name, int quantity, int number, string ressource)
        {
            AddCraft(name, quantity, number, ressource, 0, null);
        }

        public void AddCraft(string name, int quantity, int number, string ressource, int number2, string ressource2)
        {
            _craft craft = new _craft();
            craft._name = name;
            craft._quantity = quantity;
            craft._ressource = new Dictionary<string, int>();
            craft._ressource.Add(ressource, number);
            if (number2 > 0) craft._ressource.Add(ressource2, number2);
            craft._width = 70;
            craft._height = 69;
            _cellCraft.Add(craft);
        }


        public Vector2 Position
        {
            get { return Position; }
        }

        public void IsCraftable(Inventory._cell[] inventory)
        {
            foreach (_craft craft in _cellCraft)
            {
                IsCraftable(inventory, craft);
            }
        }

        void IsCraftable(Inventory._cell[] inventory, _craft craft)
        {
            int count = 0;
            foreach (KeyValuePair<string, int> resource in craft._ressource)
            {
                foreach (Inventory._cell item in inventory)
                {
                    if (item._name == resource.Key && item._quantity >= resource.Value) count++;
                }

            }

            if (count == craft._ressource.Count)
            {
                craft.IsCraftable = true;
            }
            else
            {
                craft.IsCraftable = false;
                craft._position = new Vector2(100, 100);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    [Serializable]
    public class Structure
    {
        public Dictionary<string, string[,]> structures = new Dictionary<string, string[,]>(8);

        public Structure()
        {
            structures["treeA"] = _treeModelA;
            structures["treeB"] = _treeModelB;
            structures["houseA"] = _houseModelA;
            structures["mobTowerA"] = _mobTowerModelA;
        }

        string[,] _treeModelA =  new string[,] 
        { 
            { "air", "oak_leaves", "oak_leaves", "oak_leaves", "air"}, 
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"}
        };

        string[,] _treeModelB = new string[,]
        {
            { "air", "air", "oak_leaves", "air", "air"},
            { "air", "oak_leaves", "oak_leaves", "oak_leaves", "air"},
            { "air", "air", "oak_wood", "air", "air"},
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "air", "air", "oak_wood", "air", "air"},
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"},
        };

        string[,] _houseModelA = new string[,]
        {
            {"air", "air", "air", "oak_wood", "oak_wood", "oak_wood", "air", "air", "air"},
            {"air", "air", "oak_wood", "oak_plank", "oak_plank", "oak_plank", "oak_wood", "air", "air"},
            {"air", "oak_wood", "oak_plank", "oak_plank", "glass", "oak_plank", "oak_plank", "oak_wood", "air"},
            {"oak_wood", "oak_plank", "oak_plank", "oak_plank", "glass", "oak_plank", "bookshelf", "oak_plank", "oak_wood"},
            {"oak_wood", "oak_plank", "oak_plank", "oak_plank", "oak_plank", "oak_plank", "bookshelf", "oak_plank", "oak_wood"},
            {"oak_wood", "oak_wood", "oak_wood", "oak_wood", "oak_wood", "oak_wood", "oak_wood", "oak_wood", "oak_wood"},
            {"oak_wood", "oak_plank", "oak_plank", "oak_plank", "oak_plank", "oak_plank", "oak_plank", "ladder", "oak_wood"},
            {"door_upper", "oak_plank", "oak_plank", "glass", "glass", "glass", "oak_plank", "ladder", "oak_wood"},
            {"door_lower", "oak_plank", "oak_plank", "oak_plank", "oak_plank", "oak_plank", "furnace_off", "ladder", "oak_wood"},
            {"stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick"}
        };

        string[,] _mobTowerModelA = new string[,]
        {
            {"stonebrick", "air", "stonebrick", "air", "stonebrick", "air", "stonebrick", "air", "stonebrick", "air", "stonebrick", "air", "stonebrick" },
            {"stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "stonebrick", "ladder", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick" },
            {"stonebrick", "air", "ladder", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "air", "ladder", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "air", "ladder", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },

            {"stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "ladder", "stonebrick", "stonebrick", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "ladder", "air", "air", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "ladder", "air", "air", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "ladder", "air", "air", "stonebrick" },

            {"stonebrick", "ladder", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick" },
            {"stonebrick", "ladder", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "ladder", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },
            {"stonebrick", "ladder", "air", "air", "air", "air", "air", "air", "air", "air", "air", "air", "stonebrick" },

            {"stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "ladder", "stonebrick", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "air", "ladder", "air", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "air", "ladder", "air", "stonebrick" },
            {"stonebrick", "air", "air", "air", "air", "air", "air", "air", "air", "air", "ladder", "air", "stonebrick" },
            {"stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick", "stonebrick" }
        };
    }
}

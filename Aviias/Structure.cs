using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Structure
    {
        public Dictionary<string, string[,]> structures = new Dictionary<string, string[,]>(8);

        public Structure()
        {
            structures["tree"] = _treeModel;
        }

        string[,] _treeModel =  new string[,] { 
            { "air", "oak_leaves", "oak_leaves", "oak_leaves", "air"}, 
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"} };
    }
}

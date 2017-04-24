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

        /* public Structure(string model)
         {
             // if (model == "tree") 
         }*/

   /*     public Bloc[,] ConvertStructure(string[,] model)
        {
            Bloc[,] test;
            for (int i = 0; i < model.GetLength(0); i++)
            {
                for (int j = 0; j < model.GetLength(1); j++)
                {
                    test[i, j] = new Bloc(new Vector2(i, j), _scale, id, content);
                }
            }
            return new Bloc[1, 1];
        }*/

        string[,] _treeModel =  new string[,] { 
            { "air", "oak_leaves", "oak_leaves", "oak_leaves", "air"}, 
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves", "oak_leaves"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"},
            { "air", "air", "oak_wood", "air", "air"} };
    }
}

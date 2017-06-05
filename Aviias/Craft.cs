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
        public Craft(Dictionary<int, Ressource> ressource)
        {
            //Ressource toto = new Ressource("oak-wood");
            //Ressource tptp = ressource.Add(1, toto);
            //AddCraft("planche", 4, );
        }
        
        struct _craft
        {
            public string _name { get; set; }
            public bool IsCraftable { get; set; }
            public int _quantity { get; set; }
            public Dictionary<int, Ressource> _ressource;
        }

        public void AddCraft(string name, int quantity, Dictionary<int, Ressource> ressource)
        {
            
        }

        //public Craft()
        //{
        //    _planche = new Dictionary<int, int>();
        //    _stick = new Dictionary<int, int>();
        //    _pelleEnBois = new Dictionary<int, int>();
        //}

        //public int Planche()
        //{
        //    _planche.Add(1, "bois");
        //    int Out = 4;
        //    return Out;
        //}

        //public int Stick()
        //{
        //    _stick.Add(2, _ressource.Id("planche"));
        //    int Out = 4;
        //    return Out;
        //}

        //public int PelleEnBois()
        //{
        //    _pelleEnBois.Add(1, _ressource.Id("planche"));
        //    _pelleEnBois.Add(2, _ressource.Id("stick"));
        //    int Out = 1;
        //    return Out;
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Ressource
    {
        Dictionary<int, string> _ressource;

        public Ressource()
        {
            _ressource = new Dictionary<int, string>();
            Add();
        }

        internal void Add()
        {
        string _name;
        static int _id;
        int id;
        public Ressource()
        {
            _name = "dirt";
            id = _id;
            _id++;
        }



        public string Name => _name;

        public int Id => id;
            _ressource.Add(1, "bois");
            _ressource.Add(2, "pierre");
            _ressource.Add(3, "terre");
            _ressource.Add(4, "pelouse");
            _ressource.Add(5, "planche");
        }

        public int Id(string ressource)
        {
            foreach (var i in _ressource)
                {
                if (i.Value == ressource) return i.Key;
                else return -1;
                }
        }
    }
}

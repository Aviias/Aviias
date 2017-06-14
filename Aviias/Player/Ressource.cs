using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Ressource
    {
        string _name;
        bool _isUsable;

        public Ressource(string name)
            {
                _name = name;

        }

            public string Name => _name;
            
    }
}

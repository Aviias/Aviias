﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Ressource
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
    }
}

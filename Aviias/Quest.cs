using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    public class Quest
    {
        Ressource[] _reward;
        string _type;
        int _idStartNpc;
        int _idEndNpc;
        string _spitch;

        public Quest()
        {
            CreateSpitch();
        }

        void CreateSpitch()
        {
            _spitch = "testpvofdvoidijgojerignreingernglkfdjgklmjdfkgjlmdfgjlfdmjglfdjgkldfmjgldfmgkjlmdfgkjdflmgkjdflmgkjdflmg";
        }

        public string Spitch => _spitch;
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias.IA
{
    [Serializable]
    class Spawn
    {
        [field: NonSerialized]
        Vector2 pos;
        Random _rnd;
        int _worldHeight;
        int _worldWidth;

        public Spawn(Map map)
        {
            _rnd = new Random();
            _worldHeight = map._worldHeight;
            _worldWidth = map._worldWidth;
        }

        public Vector2 SpawnOnSurface(Map map)
        {
            Vector2 spawn;
            int x = _rnd.Next(0, _worldWidth) * 16;
            spawn = new Vector2(x, FindYPos(map, x));
            return spawn;
        }

        public int FindYPos (Map map, int x)
        {
            int spawnok = 0;
            for(int y = 1; y < _worldHeight; y++)
            {
                if(map._blocs[x/16,y].Type == "air")
                {
                    spawnok = y;
                }
            }
            return spawnok;
        }

        
    }
}

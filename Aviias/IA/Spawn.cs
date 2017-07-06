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
            int x = _rnd.Next(5, _worldWidth) * 16;
            spawn = new Vector2(x, FindYPosSurface(map, x));
            return spawn;
        }

        public int FindYPosSurface (Map map, int x)
        {
            int spawnok = 0;
            for(int y = 1; y < _worldHeight; y++)
            {
                if(map._blocs[x/16,y].Type == "air")
                {
                    spawnok = y;
                }
            }
            return spawnok - 1;
        }

        public Vector2 SpawnOnCave(Map map)
        {
            int i = _rnd.Next(0, map.AllCave.Count - 1);
            Vector2 cave = map.AllCave[i];
            return cave;
        }

    }
}

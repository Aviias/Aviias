using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    class Map
    {
        readonly int _worldWidth;
        readonly int _worldHeight;
        Bloc[,] blocs;
        public const int _scale = 32;
        Random random = new Random();
        int prob;
        int columnHeight;

        public Map(int worldHeight, int worldWidth)
        {
            _worldHeight = worldHeight;
            _worldWidth = worldWidth;
            columnHeight = 5;
        }

        int id;

        public Bloc[,] GenerateMap(ContentManager content)
        {
            blocs = new Bloc[_worldWidth, _worldHeight];

            for (int i = 0; i < _worldWidth; i++)
            {
                prob = NextInt(1, 100);

                if (prob <= 15) columnHeight++;
                if (prob >= 85) columnHeight--;
                if (columnHeight < 3) columnHeight = 3;

                prob = NextInt(1, 3);

                for (int j = 0; j < _worldHeight; j++)
                {
                    if (j == _worldHeight - 1) id = 3;
                    else if (j > _worldHeight - columnHeight)
                    {
                        if (j > 0 && blocs[i, j - 1] != null && blocs[i, j - 1].Type == 0) id = 2;
                        else if (prob >= 4) id = 4;
                        else id = 1;
                        prob++;
                    }
                    else id = 0;
                    blocs[i,j] = new Bloc(new Vector2(i * (_scale), j * (_scale)), _scale, id, content);
                }

            }

            return blocs;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _worldHeight; i++)
            {
                for (int j = 0; j < _worldWidth; j++)
                {
                    if (blocs[j,i] != null) blocs[j,i].Draw(spriteBatch);
                }
            }
        }

        int NextInt(int min, int max)
        {
             return random.Next(min, max);
        }
    }
}

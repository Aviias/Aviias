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
        const int _scale = 16;
        Random random = new Random();
        int prob;
        int columnHeight;
        const int _ironRate = 5;
        const int _coalRate = 5;
        int _oreRandom = 0;
        bool _oreGeneration;
        int _treeRate = 1;
        int _treeGeneration = 0;
        string[,] _structureModel;
        Structure structures = new Structure();

        public Map(int worldHeight, int worldWidth)
        {
            _worldHeight = worldHeight;
            _worldWidth = worldWidth;
            columnHeight = worldHeight - 32;
        }

        string id;

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
                    if (j == _worldHeight - 1) id = "bedrock";
                    else if (j > _worldHeight - columnHeight)
                    {
                        if (j > 0 && blocs[i, j - 1] != null && blocs[i, j - 1].Type == "air") id = "grass_side";
                        else if (prob >= 4)
                        {
                            // Ore generation
                            if (_worldHeight - j < 150)
                            {
                                if (_worldHeight - j < 100)
                                {
                                    _oreRandom = NextInt(1, 100);
                                    if (blocs[i, j - 1] != null || blocs[i - 1, j] != null && (blocs[i, j - 1].Type == "iron_ore" || blocs[i - 1, j].Type == "iron_ore")) _oreRandom /= 2;
                                    if (_oreRandom <= _ironRate)
                                    {
                                        id = "iron_ore";
                                        _oreGeneration = true;
                                    }
                                }
                                if (_worldHeight - j < 150)
                                {
                                    _oreRandom = NextInt(1, 100);
                                    if (blocs[i, j - 1] != null || blocs[i - 1, j] != null && (blocs[i, j - 1].Type == "coal_ore" || blocs[i - 1, j].Type == "coal_ore")) _oreRandom /= 2;
                                    if (_oreRandom <= _coalRate)
                                    {
                                        id = "coal_ore";
                                        _oreGeneration = true;
                                    }
                                }
                            }
                            if (!_oreGeneration) id = "stone";
                            _oreGeneration = false;
                        }
                        else id = "dirt";
                        prob++;
                    }
                    else id = "air";
                    blocs[i, j] = new Bloc(new Vector2(i * (_scale), j * (_scale)), _scale, id, content);
                }

                // Structures generation
                for (int k = 0; k < _worldWidth; k++)
                {
                    for (int l = 0; l < _worldHeight; l++)
                    {
                        if (k > 6 && l > 6 && blocs[k - 3, l - 1] != null && blocs[k - 3, l - 1].Type != "oak_wood" && blocs[k - 3, l] != null && blocs[k - 3, l].Type == "grass_side")
                        {
                            _treeGeneration = NextInt(1, 400);
                            if (_treeGeneration <= _treeRate)
                            {
                                _structureModel = structures.structures["tree"];
                                AddTree(k, l, _structureModel, content);
                            }
                        }
                    }
                }

                // Cave generation

                blocs[0, 0] = new Bloc(new Vector2(0, 0), _scale, "bedrock", content);
            }

            return blocs;
        }

        public bool isAir(int x, int y, int sizeX, int sizeY)
        {
            for (int width = x; width < sizeX; width++)
            {
                for (int height = y; height < sizeY; height++)
                {

                }
            }
            return true;
        }

        

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            for (int i = 0; i < _worldHeight; i++)
            {
                for (int j = 0; j < _worldWidth; j++)
                {
                    if (blocs[j, i] != null) blocs[j, i].Draw(spriteBatch);
                    if (j== 1 && i == 1)
                    {
                        spriteBatch.DrawString(font, Convert.ToString(blocs[j,i].GetPosBlock.X), new Vector2(-10, -10), Color.Red);
                        spriteBatch.DrawString(font, Convert.ToString(blocs[j,i].GetPosBlock.Y), new Vector2(-20, -20), Color.Red);
                    }
                    
                }
            }


        }

        public void AddTree(int x, int y, string[,] treeModel, ContentManager content)
        {
            int lengthX = treeModel.GetLength(0);
            int lengthY = treeModel.GetLength(1);
            for (int i = 0; i < lengthY; i++)
            {
                for (int j = 0; j < lengthX; j++)
                {
                    if (x - lengthY >= 0 && y - lengthX >= 0 && treeModel[j, i] != null && treeModel[j, i] != "air" && blocs[x - lengthY + i, y - lengthX + j] != null) blocs[x - lengthY + i, y - lengthX + j]._texture = content.Load<Texture2D>(treeModel[j, i]);
                }
            }
        }

        public void FindBreakBlock(Vector2 pos, Player player, ContentManager Content)
        {
            
            for (int i = 0; i < _worldHeight - 1; i++)
            {
                for (int j = 0; i < _worldWidth - 1; i++)
                {
                    if (blocs[j,i].GetPosBlock == pos)
                    {                       
                        player.breakBloc(blocs[j, i], pos, Content, blocs, i, j);
                    }
                }
            }          
        }

        int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}

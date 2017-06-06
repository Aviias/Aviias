using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    class Map
    {
        public readonly int _worldWidth;
        public readonly int _worldHeight;
        public Bloc[,] _blocs;
        const int _scale = 16;
        Random random = new Random();
        int prob;
        int columnHeight;
        const int _ironRate = 1;
        const int _coalRate = 1;
        int _oreRandom = 0;
        bool _oreGeneration;
        int _treeRate = 1;
        int _treeGeneration;
        string[,] _structureModel;
        Structure structures = new Structure();
        int _caveWallRate;
        int _mountainRate = 2;
        int _mountainViolence = 2;
        int _mountainStep;
        int _mountainProb;
        bool _isMountain;
        int _mountainSize;
        int _mtest;

        public int WorldWidth
        {
            get { return _worldWidth; }
        }

        public int WorldHeight
        {
            get { return _worldHeight; }
        }

        public Map(int worldHeight, int worldWidth)
        {
            _worldHeight = worldHeight;
            _worldWidth = worldWidth;
            columnHeight = worldHeight / 2;
            _caveWallRate = 2;
        }

        string id;

        public Bloc[,] GenerateMap(ContentManager content)
        {
            _blocs = new Bloc[_worldWidth, _worldHeight];

            for (int i = 0; i < _worldWidth; i++)
            {
                if (_mountainStep <= 0)
                {
                    _mountainStep = 0;
                    _isMountain = false;
                }
                if (_mountainStep == 0)
                {
                    _mountainProb = NextInt(1, 100);
                    if (_mountainProb < _mountainRate)
                    {
                        _mountainStep = 10;
                        _isMountain = true;
                        _mountainSize = NextInt(8, 20);
                    }
                }
                if (_isMountain)
                {
                    if (_mountainStep < _mountainSize / 2) _mountainProb = NextInt(0, _mountainViolence);
                    else _mountainProb = NextInt(-_mountainViolence, 0);
                    columnHeight += _mountainProb;
                    _mountainStep--;

                }
                else
                {
                    prob = NextInt(1, 100);

                    if (prob <= 15) columnHeight++;
                    if (prob >= 85) columnHeight--;
                }

                if (columnHeight < 3) columnHeight = 3;

                prob = NextInt(1, 3);

                for (int j = 0; j < _worldHeight; j++)
                {
                    if (j == _worldHeight - 1) id = "bedrock";
                    else if (j > _worldHeight - columnHeight)
                    {
                        if (j > 0 && _blocs[i, j - 1] != null && _blocs[i, j - 1].Type == "air") id = "grass_side";
                        else if (prob >= 4)
                        {
                            // Ore generation
                            if (_worldHeight - j < 150)
                            {
                                if (_worldHeight - j < 100)
                                {
                                    _oreRandom = NextInt(1, 300);
                                    if (_blocs[i, j - 1] != null || _blocs[i - 1, j] != null && (_blocs[i, j - 1].Type == "iron_ore" || _blocs[i - 1, j].Type == "iron_ore")) _oreRandom /= 20;
                                    if (_oreRandom <= _ironRate)
                                    {
                                        id = "iron_ore";
                                        _oreGeneration = true;
                                    }
                                }
                                if (_worldHeight - j < 150)
                                {
                                    _oreRandom = NextInt(1, 300);
                                    if (_blocs[i, j - 1] != null || _blocs[i - 1, j] != null && (_blocs[i, j - 1].Type == "coal_ore" || _blocs[i - 1, j].Type == "coal_ore")) _oreRandom /= 20;
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
                    _blocs[i, j] = new Bloc(new Vector2(i * (_scale), j * (_scale)), _scale, id, content);
                }

                // Structures generation
                for (int k = 5; k < _worldWidth - 5; k++)
                {
                    for (int l = 5; l < _worldHeight - 5; l++)
                    {
                        if (k > 6 && l > 6 && _blocs[k - 3, l] != null && _blocs[k - 3, l].Type == "grass_side")
                        {
                            _treeGeneration = NextInt(1, 2500);
                            if (_treeGeneration <= _treeRate)
                            {
                                _treeGeneration = NextInt(1, 3);
                                if (_treeGeneration == 1) _structureModel = structures.structures["mobTowerA"];
                                else _structureModel = structures.structures["mobTowerA"];
                                AddTree(k, l, _structureModel, content);
                            }
                        }
                    }
                }

                _blocs[0, 0] = new Bloc(new Vector2(0, 0), _scale, "bedrock", content);
                // Cave generation
                for (int o = 130; o < _worldHeight; o++)
                {
                    for (int p = 10; p < _worldWidth; p++)
                    {
                        // if (p < _worldWidth - 20 && o < _worldHeight - 20 && blocs[p, o] != null && (NextInt(0, 120000) < _caveWallRate || GetAdjacentWalls(p, o, 4, 4) > 2)) FillAir(p, o, content);
                        if (p < _worldWidth - 20 && o < _worldHeight - 20 && _blocs[p, o] != null && NextInt(0, 120000) < _caveWallRate) FillAir(p, o, content);
                    }
                }
            }

            return _blocs;
        }

        public void FillAir(int x, int y, ContentManager content)
        {
            int caveRandomX = NextInt(2, 6);
            int caveRandomY = NextInt(2, 6);

            for (int i = y - caveRandomY; i < y + caveRandomY; i++)
            {
                for (int j = x - caveRandomX; j < x + caveRandomX; j++)
                {
                    if (_blocs[j, i] != null && _blocs[j, i].Type != "bedrock") _blocs[j, i]._texture = content.Load<Texture2D>("air");
                }
            }
        }

        public int GetAdjacentWalls(int x, int y, int scopeX, int scopeY)
        {
            int startX = x - scopeX;
            int startY = y - scopeY;
            int endX = x + scopeX;
            int endY = y + scopeY;

            int iX = startX;
            int iY = startY;

            int wallCounter = 0;

            for (iY = startY; iY <= endY; iY++)
            {
                for (iX = startX; iX <= endX; iX++)
                {
                    if (!(iX == x && iY == y))
                    {
                        if (_blocs[iX, iY] != null && _blocs[iX, iY].Type == "air")
                        {
                            wallCounter += 1;
                        }
                    }
                }
            }
            return wallCounter;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            int xx = x / 16;
            int yy = y / 16;
            for (int i = yy - 60; i < yy + 60; i++)
            {
                for (int j = xx - 80; j < xx + 80; j++)
                {
                    if (i >= 0 && j >= 0 && i < _worldHeight && j < _worldWidth && _blocs[j, i] != null) _blocs[j, i].Draw(spriteBatch);
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
                    if (x - lengthY >= 0 && y - lengthX >= 0 && treeModel[j, i] != null && treeModel[j, i] != "air" && _blocs[x - lengthY + i, y - lengthX + j] != null) _blocs[x - lengthY + i, y - lengthX + j].ChangeBloc(treeModel[j, i], content);
                }
            }
        }

        bool isThereATreeHere(int x, int y)
        {
            for (int i = y - 20; i < y + 10; i++)
            {
                for (int j = x - 20; j < x + 10; j++)
                {
                    if (_blocs[i, j] != null && (_blocs[i, j].Type == "oak_wood" || _blocs[i, j].Type == "oak_leaves")) return true;
                }
            }
            return false;
        }

        public void FindBreakBlock(Vector2 pos, Player player, ContentManager Content)
        {
            float clickCoordX = pos.X;
            float clickCoordY = (float)1.007 * pos.Y + (float)8.06;
            int i = 0;
            bool isFind = false;

            //log.WriteLine("=========================   clickCoordX = " + clickCoordX + ", clickCoordY = " + clickCoordY);

            while ((i < _worldHeight) && (isFind == false))
            {
                int j = 0;
                while ((j < _worldWidth) && (isFind == false))
                {
                        /*
                        if (blocs[i, j].IsBreakable )
                            log.WriteLine("bloc[ " + i + "," + j + "] X = " + blocs[i, j].GetPosBlock.X + ", Y = " + blocs[i, j].GetPosBlock.Y + " Breakable, type = "+ blocs[i, j].Type );
                        else
                            log.WriteLine("bloc[ " + i + "," + j + "] X = " + blocs[i, j].GetPosBlock.X + ", Y = " + blocs[i, j].GetPosBlock.Y + " Not Breakable, type = " + blocs[i, j].Type);
                        */
                        if ((clickCoordX >= _blocs[i, j].GetPosBlock.X) && (clickCoordX < (_blocs[i, j].GetPosBlock.X + _scale)))
                        {
                            if ((clickCoordY >= _blocs[i, j].GetPosBlock.Y) && (clickCoordY < (_blocs[i, j].GetPosBlock.Y + _scale)))
                            {
                                player.breakBloc(_blocs[j, i], Content, _blocs, i, j, 16);
                                isFind = true;
                            }
                            //log.WriteLine("=========================   Trouve = i " + i + ", j = " + j);
                            //player.breakBloc(_blocs[i, j], Content, _blocs, i, j, _scale);
                        }
                    j++;
                }
                i++;
            }
        }

        public float GetDistance(Vector2 one, Vector2 two)
        {
            return (Math.Abs(one.X - two.X) + Math.Abs(one.Y - two.Y));
        }

        public void DebugBloc(int i, int j, StreamWriter log)
        {
            log.WriteLine("bloc[ " + i + "," + j +"] X = " + _blocs[i, j].GetPosBlock.X + ", Y = " + _blocs[i, j].GetPosBlock.Y);
        }

        int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }
        

    }
}
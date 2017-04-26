﻿using Microsoft.Xna.Framework;
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

        public Map(int worldHeight, int worldWidth)
        {
            _worldHeight = worldHeight;
            _worldWidth = worldWidth;
            columnHeight = worldHeight / 2;
            _caveWallRate = 1;
        }

        string id;

        public Bloc[,] GenerateMap(ContentManager content)
        {
            blocs = new Bloc[_worldWidth, _worldHeight];

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
                for (int k = 5; k < _worldWidth - 5; k++)
                {
                    for (int l = 5; l < _worldHeight - 5; l++)
                    {
                        if (k > 6 && l > 6 && blocs[k - 3, l] != null && blocs[k - 3, l].Type == "grass_side")
                        {
                            _treeGeneration = NextInt(1, 1000);
                            if (_treeGeneration <= _treeRate)
                            {
                                _structureModel = structures.structures["tree"];
                                AddTree(k, l, _structureModel, content);
                            }
                        }
                    }
                }

                // Cave generation
                for (int o = 150; o < _worldHeight; o++)
                {
                    for (int p = 10; p < _worldWidth; p++)
                    {
                        if (p < 190 && o < 190 && blocs[p, o] != null && (NextInt(0, 9000) < _caveWallRate)) FillAir(p, o, content);
                    }
                }
            }

            return blocs;
        }

        public void FillAir(int x, int y, ContentManager content)
        {
             int caveRandomX = NextInt(2, 4);
             int caveRandomY = NextInt(1, 5);

            for (int i = y - caveRandomY; i < y + caveRandomY; i++)
            {
                for (int j = x - caveRandomX; j < x + caveRandomX; j++)
                {
                    if (blocs[j, i] != null && blocs[j, i].Type != "bedrock") blocs[j, i]._texture = content.Load<Texture2D>("air");
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
                        if (blocs[iX, iY] != null && blocs[iX, iY].Type == "air")
                        {
                            wallCounter += 1;
                        }
                    }
                }
            }
            return wallCounter;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _worldHeight; i++)
            {
                for (int j = 0; j < _worldWidth; j++)
                {
                    if (blocs[j, i] != null) blocs[j, i].Draw(spriteBatch);
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
                    if (x - lengthY >= 0 && y - lengthX >= 0 && treeModel[j, i] != null && treeModel[j, i] != "air" && blocs[x - lengthY + i, y - lengthX + j] != null) blocs[x - lengthY + i, y - lengthX + j].ChangeBloc(treeModel[j, i], content);
                }
            }
        }

        bool isThereATreeHere(int x, int y)
        {
            for(int i = y - 20; i < y + 10; i++)
            {
                for(int j = x - 20; j < x + 10; j++)
                {
                    if (blocs[i, j] != null && (blocs[i, j].Type == "oak_wood" || blocs[i, j].Type == "oak_leaves")) return true;
                }
            }
            return false;
        }

        int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}

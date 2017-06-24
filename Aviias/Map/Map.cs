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
    [Serializable]
    public class Map
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
        bool _isNight;
        public int skyLuminosity;

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
            skyLuminosity = 6;
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

                    if (prob <= 8) columnHeight++;
                    if (prob >= 92) columnHeight--;
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
                    if (j >= 0 && j <= 3)
                    {
                        _blocs[i, j]._isInContactWithTheSky = true;
                    }
                    else if (_blocs[i, j - 1] != null && _blocs[i, j - 1]._isInContactWithTheSky)
                    {
                        if (_blocs[i, j - 1].Type == "air")
                        {
                                _blocs[i, j]._isInContactWithTheSky = true;
                        }
                    /*    else
                        {
                            _blocs[i, j - 1].ChangeLuminosity(4);
                        }*/
                    }
                }

                // Structures generation
                for (int l = 5; l < _worldWidth - 5; l++)
                {
                    for (int k = 5; k < _worldHeight - 5; k++)
                    {
                        if (k > 6 && l > 6 && l < 200 && _blocs[k - 3, l] != null && _blocs[k - 3, l].Type == "grass_side")
                        {
                            int rand = NextInt(1, 8500);
                            if (rand == 1)
                            {
                                rand = NextInt(1, 3);
                                if (rand == 1)
                                {
                                    _structureModel = structures.structures["houseA"];
                                }
                                else _structureModel = structures.structures["mobTowerA"];
                                AddHouse(k, l, _structureModel, content);
                                k += _structureModel.Length * 2;
                                l += _structureModel.Length * 2;
                            }
                        }

                        if (k > 6 && l > 6 && l < 200 && _blocs[k - 3, l] != null && _blocs[k - 3, l].Type == "grass_side")
                        {
                            _treeGeneration = NextInt(1, 2300);
                            if (_treeGeneration <= _treeRate)
                            {
                                _treeGeneration = NextInt(1, 3);
                                if (_treeGeneration == 1) _structureModel = structures.structures["treeA"];
                                else _structureModel = structures.structures["treeB"];
                                AddTree(k, l, _structureModel, content);
                                k += _structureModel.Length * 2;
                                l += _structureModel.Length * 2;
                            }
                        }
                    }
                }

                _blocs[0, 0] = new Bloc(new Vector2(0, 0), _scale, "bedrock", content);
                _blocs[20, 20] = new Bloc(new Vector2(20*16, 20*16), _scale, "bedrock", content);
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


        public void AddHouse(int x, int y, string[,] houseModel, ContentManager content)
        {
            int lengthX = houseModel.GetLength(0);
            int lengthY = houseModel.GetLength(1);
            for (int i = 0; i < lengthY; i++)
            {
                for (int j = 0; j < lengthX; j++)
                {
                    if (x - lengthY >= 0 && y - lengthX >= 0 && houseModel[j, i] != null && houseModel[j, i] != "air" && _blocs[x - lengthY + i, y - lengthX + j] != null) _blocs[x - lengthY + i, y - lengthX + j].ChangeBloc(houseModel[j, i], content);
                }
            }
        }

        public void FindBreakBlock(Vector2 pos, Player player, ContentManager Content, StreamWriter log)
        {
           // float clickCoordY = (float)1.007 * pos.Y + (float)8.06;
            int  i = (int)pos.X / _scale;
            int j = (int)pos.Y / _scale;

            if ( (i>=0) && (i< _worldHeight) && (j>=0) && (j< _worldWidth) ) player.breakBloc(_blocs[i, j], Content, _blocs, i, j, _scale, log);

        }

        public void SetBloc(Vector2 pos, ContentManager Content, Player player, string name)
        {
            int i = (int)pos.X / _scale;
            int j = (int)pos.Y / _scale;
            if ((i >= 0) && (i < _worldHeight) && (j >= 0) && (j < _worldWidth)) player.setbloc(_blocs[i, j], Content, _blocs, i, j, _scale, name);

        }

        public float GetDistance(Vector2 one, Vector2 two)
        {
            return (Math.Abs(one.X - two.X) + Math.Abs(one.Y - two.Y));
        }

        public void DebugBloc(int i, int j, StreamWriter log)
        {
            log.WriteLine("bloc[ " + i + "," + j + "] X = " + _blocs[i, j].GetPosBlock.X + ", Y = " + _blocs[i, j].GetPosBlock.Y);
        }

        int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public void ActualizeShadow(int x, int y)
        {
            int xx = x / 16;
            int yy = y / 16;
            for (int i = yy - 60; i < yy + 60; i++)
            {
                for (int j = xx - 80; j < xx + 80; j++)
                {
                    if (i > 0 && j > 0 && j < _worldHeight && i < _worldWidth)
                    {
                        if (_blocs[i, j] != null && _blocs[i, j - 1]!= null && _blocs[i, j].Type == "air")
                        {
                            if (_blocs[i, j - 1].Type == "air" && _blocs[i, j - 1]._isInContactWithTheSky)
                            {
                                _blocs[i, j]._isInContactWithTheSky = true;
                            }
                        }
                    }
                }
            }

            for (int i = yy - 60; i < yy + 60; i++)
            {
                for (int j = xx - 80; j < xx + 80; j++)
                {
                    if (i > 2 && j > 2 && j < _worldHeight - 2 && i < _worldWidth - 2 && _blocs[i, j-1] != null && _blocs[i, j] != null)
                    {
                        if (_blocs[i, j]._isInContactWithTheSky)
                        {
                            _blocs[i, j].ChangeLuminosity(skyLuminosity);
                        }

                        if (_blocs[i, j].Type == "air")
                        {
                            if (_blocs[i, j - 1].Type == "air" && _blocs[i, j - 1]._isInContactWithTheSky)
                            {
                                _blocs[i, j]._isInContactWithTheSky = true;
                            }

                            if (!_blocs[i, j]._isInContactWithTheSky)
                            {

                                _blocs[i, j].ChangeLuminosity(GetBiggestNumber(_blocs[i - 1, j].Luminosity, _blocs[i + 1, j].Luminosity, _blocs[i, j - 1].Luminosity, _blocs[i, j + 1].Luminosity) - 1);
                            }

                            /*  if (!_blocs[i, j]._isInContactWithTheSky)
                              {
                                  _blocs[i, j].ChangeLuminosity(_blocs[i, j - 1].Luminosity - 1);
                              }*/
                        }
                        else
                        {
                            if (_blocs[i, j - 1].Type == "air" && _blocs[i, j - 1]._isInContactWithTheSky)
                            {
                                _blocs[i, j]._isInContactWithTheSky = true;
                            }

                            if (!_blocs[i, j]._isInContactWithTheSky)
                            {

                                _blocs[i, j].ChangeLuminosity(GetBiggestNumber(_blocs[i - 1, j].Luminosity, _blocs[i + 1, j].Luminosity, _blocs[i, j - 1].Luminosity, _blocs[i, j + 1].Luminosity) - 2);
                            }
                        }
                        if (_blocs[i, j]._isInContactWithTheSky) _blocs[i, j].ChangeLuminosity(skyLuminosity);
                        if (TorchUpdate(i, j) > _blocs[i, j].Luminosity) _blocs[i, j].ChangeLuminosity(TorchUpdate(i, j));
                    }

                   
                }
            }
        }

        public int TorchUpdate(int x, int y)
        {
             int xx = x - 6;
             int yy = y - 6;
             int shortest = 7;

             for (int i = xx; i < x + 6; i++)
             {
                 for (int j = yy; j < y + 6; j++)
                 {
                     if (i > 0 && j > 0 && i < _worldWidth && j < _worldHeight && _blocs[i, j] != null && _blocs[i, j].Type == "torche")
                     {
                       if (Math.Abs(x - i) + Math.Abs(y - j) < shortest) shortest = (Math.Abs(x - i) + Math.Abs(y - j));
                     } 
                 }
             }
                  return Math.Abs(shortest - 7);
        }

        public void TimeForward()
        {
            if (_isNight)
            {
                skyLuminosity++;
            }
            else
            {
                skyLuminosity--;
            }
            if (skyLuminosity == 1 || skyLuminosity == 7) _isNight = !_isNight;
        }

        public void Reload(ContentManager content)
        {
            foreach(Bloc bloc in _blocs)
            {
                bloc.Reload(content);
            }
        }

        int GetBiggestNumber(int a, int b, int c, int d)
        {
            int[] num = new int[4] {a, b, c, d};

            return num.Max();
        }
    }
}
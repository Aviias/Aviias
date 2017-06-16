using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias.IA
{
    class Physics 
    {
        int _yVelocity;
        bool _collisions;
        int _nbBlocs;
        public int _jumpHeight;
        bool flyMod;
        public int _gravity;

        public Physics(bool fly, int gravity, int jumpHeight)
        {
            flyMod = fly;
            _gravity = gravity;
            _jumpHeight = jumpHeight;
        }

        internal void UpdatePhysics(Map map, Monster monster)
        {
            if (!flyMod)
            {
                if (IsInAir(map, monster))
                {
                    monster.Y += _yVelocity;
                    if (_yVelocity < 60) _yVelocity += _gravity;
                }
                else
                {
                    //monster.Y += 50;
                    _yVelocity = 0;
                }
            }
        }

        public List<Bloc> GetBlocsAround(Map map, Monster monster)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);

            for (int a = (int)(monster.MonsterPosition.Y / 16); a < (monster.MonsterPosition.Y / 16) + 8; a++)
            {
                for (int b = (int)(monster.MonsterPosition.X / 16); b < (monster.MonsterPosition.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && a < map._worldHeight && b < map._worldWidth && map._blocs[b, a] != null && map._blocs[b, a].Type != "air" && map._blocs[b, a].Type != "ladder")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        _nbBlocs++;
                    }
                }
            }

            return _blocs;
        }

        public bool IsInAir(Map map, Monster monster)
        {
            List<int> list = new List<int>(16);
            list = GetCollisionSide(GetBlocsAround(map, monster), monster);
            if (list.Contains(3)) return false;
            return true;
        }

        internal void Jump(Map map, Monster monster)
        {
            if (!IsInAir(map, monster))
            {
                _yVelocity = _jumpHeight;
                monster.Y -= 10;
            }
        }

        public void UpdateCollision(Map map, Monster monster)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);
            for (int a = (int)(monster.MonsterPosition.Y / 16); a < (monster.MonsterPosition.Y / 16) + 8; a++)
            {
                for (int b = (int)(monster.MonsterPosition.X / 16); b < (monster.MonsterPosition.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && a < map._worldHeight && b < map._worldWidth && map._blocs[b, a] != null && map._blocs[b, a].Type != "air")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        _nbBlocs++;
                    }
                }
            }

            List<int> list = new List<int>(16);
            list = GetCollisionSide(_blocs, monster);
        }

        public List<int> GetCollisionSide(List<Bloc> _blocs, Monster monster)
        {
            List<int> result = new List<int>(16);
            Rectangle playerRect;
            Rectangle playerRect2;
            playerRect = new Rectangle((int)monster.MonsterPosition.X, (int)monster.MonsterPosition.Y, monster.Texture.Width, monster.Texture.Height);
            playerRect2 = new Rectangle((int)monster.MonsterPosition.X, (int)monster.MonsterPosition.Y + 1, monster.Texture.Width, monster.Texture.Height);

            Rectangle rectTest = new Rectangle((int)monster.MonsterPosition.X, (int)monster.MonsterPosition.Y - 10, monster.Texture.Width, monster.Texture.Height);

            for (int i = 0; i < _blocs.Count; i++)
            {
                if (_blocs[i] != null)
                {
                    Rectangle blocRect;
                    blocRect = new Rectangle((int)_blocs[i].posX, (int)_blocs[i].posY, _blocs[i].Width, _blocs[i].Height);
                    if (playerRect.Intersects(blocRect))
                    {
                        if (playerRect.Bottom > blocRect.Top && playerRect.Bottom < blocRect.Bottom)
                        {
                            result.Add(3);
                            if (!rectTest.Intersects(blocRect)) monster.Y -= 1;
                        }
                        if (playerRect.Top < blocRect.Bottom && playerRect.Top > blocRect.Top) result.Add(4);
                        //  if (playerRect.Left < blocRect.Right && playerRect.Left > blocRect.Left) result.Add(2);
                        //  if (playerRect.Right > blocRect.Left && playerRect.Right < blocRect.Right) result.Add(1);
                        if (rectTest.Left < blocRect.Right && rectTest.Left > blocRect.Left) result.Add(2);
                        if (rectTest.Right > blocRect.Left && rectTest.Right < blocRect.Right) result.Add(1);
                        _collisions = true;
                    }
                    if (playerRect2.Intersects(blocRect))
                    {
                        if (playerRect2.Bottom > blocRect.Top && playerRect2.Bottom < blocRect.Bottom)
                        {
                            _yVelocity = 0;
                            result.Add(3);
                        }
                    }
                }
            }
            return result;
        }
    }
}

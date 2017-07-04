using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias.IA
{
    class Physics 
    {
        public Vector2 _pos;
        int _yVelocity;
        bool _collisions;
        int _nbBlocs;
        public int _jumpHeight;
        bool flyMod;
        public int _gravity;


        public Physics(bool fly, int gravity, int jumpHeight, Vector2 position)
        {
            flyMod = fly;
            _gravity = gravity;
            _jumpHeight = jumpHeight;
            _pos = position;
        }

        internal void UpdatePhysics(Map map, Texture2D texture)
        {
            if (!flyMod)
            {
                if (IsInAir(map, texture))
                {
                    _pos.Y += _yVelocity;
                    if (_yVelocity < 60) _yVelocity += _gravity;
                }
                else
                {
                    _yVelocity = 0;
                }
            }
        }

        public List<Bloc> GetBlocsAround(Map map)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);

            for (int a = (int)(_pos.Y / 16); a < (_pos.Y / 16) + 8; a++)
            {
                for (int b = (int)(_pos.X / 16); b < (_pos.X) / 16 + 8; b++)
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

        public bool IsInAir(Map map, Texture2D texture)
        {
            List<int> list = new List<int>(16);
            list = GetCollisionSide(GetBlocsAround(map), texture);
            if (list.Contains(3)) return false;
            return true;
        }

        internal void Jump(Map map, Texture2D texture)
        {
            if (!IsInAir(map, texture))
            {
                _yVelocity = _jumpHeight;
                _pos.Y -= 10;
            }
        }

        public void UpdateCollision(Map map, Texture2D texture)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);
            for (int a = (int)(_pos.Y / 16); a < (_pos.Y / 16) + 8; a++)
            {
                for (int b = (int)(_pos.X / 16); b < (_pos.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && a < map._worldHeight && b < map._worldWidth && map._blocs[b, a] != null && map._blocs[b, a].Type != "air")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        _nbBlocs++;
                    }
                }
            }

            List<int> list = new List<int>(16);
            list = GetCollisionSide(_blocs, texture);
        }

        public List<int> GetCollisionSide(List<Bloc> _blocs, Texture2D texture)
        {
            List<int> result = new List<int>(16);
            Rectangle playerRect;
            Rectangle playerRect2;
            playerRect = new Rectangle((int)_pos.X, (int)_pos.Y, texture.Width, texture.Height);
            playerRect2 = new Rectangle((int)_pos.X, (int)_pos.Y + 1, texture.Width, texture.Height);
           

            Rectangle rectTest = new Rectangle((int)_pos.X, (int)_pos.Y - 10, texture.Width, texture.Height);

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
                            //result.Add(3);
                            
                            _yVelocity = 0;
                            _pos.Y -= 1;
                            //     if (!rectTest.Intersects(blocRect)) _pos.Y -= 1;
                        }
                        if (playerRect.Top < blocRect.Bottom && playerRect.Top > blocRect.Top) result.Add(4);
                        //  if (playerRect.Left < blocRect.Right && playerRect.Left > blocRect.Left) result.Add(2);
                        //  if (playerRect.Right > blocRect.Left && playerRect.Right < blocRect.Right) result.Add(1);
                        if (rectTest.Left < blocRect.Right && rectTest.Left > blocRect.Left) result.Add(2);
                        if (rectTest.Right > blocRect.Left && rectTest.Right < blocRect.Right) result.Add(1);
                        //_collisions = true;
                    }
                    if (playerRect2.Intersects(blocRect))
                    {
                        if (playerRect2.Bottom  > blocRect.Top && playerRect2.Bottom < blocRect.Bottom)
                        {
                            //_pos.Y -= 1;
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

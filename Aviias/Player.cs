﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;

namespace Aviias
{
    public class Player
    {
        Map _ctx;
        public Texture2D PlayerTexture;
        public Vector2 Position;
        public bool Active;
        public int Health;
        Text text;
        public bool _displayPos;
        string _str;
        public List<Quest> _activeQuest;
        internal Dictionary<Ressource, int> _inventory;
        public bool isInAir;
        public float _yVelocity;
        public double _gravity;
        public float _jumpHeight;
        bool _collisions;
        int _nbBlocs;
        float _moveSpeed;
        public bool flyMod;

        public int Width
        {
            get { return PlayerTexture.Width; }
        }

        public int Height
        {
            get { return PlayerTexture.Height; }
        }

        public float X
        {
            get { return Position.X; }
           
        }

        public float Y
        {
            get { return Position.Y; }
        }

        public void Initialize(Texture2D texture, Vector2 position, ContentManager content)
        {
            PlayerTexture = texture;
            Position = position;
            Active = true;
            Health = 100;
            text = new Aviias.Text(content);
            _displayPos = true;
            _gravity = 1;
            _jumpHeight = -20;
            _moveSpeed = 0.8f;
            _activeQuest = new List<Quest>(8);
            _inventory = new Dictionary<Ressource, int>(8);
            _inventory.Add(new Ressource(), 10);
        }

        public void DecreaseInventory(int quantity, string name)
        {
            foreach (KeyValuePair<Ressource, int> entry in _inventory)
            {
                if (entry.Key.Name == name) _inventory[entry.Key] -= quantity;
            }
        }

        public void AddInventory(int quantity, string name)
        {
            foreach (KeyValuePair<Ressource, int> entry in _inventory)
            {
                if (entry.Key.Name == name) _inventory[entry.Key] += quantity;
            }
        }

        public Vector2 PlayerPosition
        {
            get { return Position; }
        }

        public void AddQuest(Quest quest)
        {
            _activeQuest.Add(quest);
        }

        public void RemoveQuest(Quest quest)
        {
            _activeQuest.Remove(quest);
        }
        
        public Vector2 CursorPos()
        {             
            int posX = Cursor.Position.X;
            int posY = Cursor.Position.Y;

            Vector2 cursorPos = new Vector2(posX, posY);
            return cursorPos;
            
        }

        public void breakBloc(Bloc bloc, Vector2 position, ContentManager content, Bloc[,] blocs, int i, int j)
        {
            if (bloc != null)
            {
                if (bloc._isBreakable)
                {
                    bloc = new Bloc(position, 16, "air", content);
                    blocs[j, i] = bloc;
                }
            }

        }

        internal void Jump(Map map)
        {
            if (!IsInAir(map))
            {
                _yVelocity = _jumpHeight;
                Position.Y -= 10;
            }
        }

        internal void Update(Map map)
        {
            if (!flyMod)
            {
                if (IsInAir(map))
                {
                    Position.Y += _yVelocity;
                    if (_yVelocity < 12) _yVelocity += (float)_gravity;
                }
                else
                {
                    _yVelocity = 0;
                }
            }
        }

        internal bool IsInAir(Map map)
        {
            List<int> list = new List<int>(16);
            list = GetCollisionSide(GetBlocsAround(map));
            if (list.Contains(3)) return false;
            return true;
        }

        internal void UpdateCollision(Map map, Player player)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);

            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 8; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && map._blocs[b, a] != null && map._blocs[b, a].Type != "air")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        _nbBlocs++;
                    }
                }
            }
            
            List<int> list = new List<int>(16);
            list = GetCollisionSide(_blocs);
        }

        public List<int> GetCollisionSide(List<Bloc> _blocs)
        {
            List<int> result = new List<int>(16);
            Rectangle playerRect;
            Rectangle playerRect2;
            playerRect = new Rectangle((int)Position.X, (int)Position.Y, PlayerTexture.Width, PlayerTexture.Height);
            playerRect2 = new Rectangle((int)Position.X, (int)Position.Y + 1, PlayerTexture.Width, PlayerTexture.Height);

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
                            Position.Y -= 1;
                        }
                         if (playerRect.Top < blocRect.Bottom && playerRect.Top > blocRect.Top) result.Add(4);
                         if (playerRect.Left < blocRect.Right && playerRect.Left > blocRect.Left) result.Add(2);
                         if (playerRect.Right > blocRect.Left && playerRect.Right < blocRect.Right) result.Add(1);
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

        internal List<Bloc> GetBlocsAround(Map map)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);

            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 8; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && map._blocs[b, a] != null && map._blocs[b, a].Type != "air")
                    {
                        _blocs.Add(map._blocs[b, a]);
                        _nbBlocs++;
                    }
                }
            }

            return _blocs;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);

              if (_displayPos) text.DisplayText((Position.X  + " - " + Position.Y), new Vector2(Position.X, Position.Y - 30), spriteBatch, Color.Red);
            // if (_displayPos) text.DisplayText(((int)Position.X/64 + " - " + (int)Position.Y/64), new Vector2(Position.X, Position.Y - 50), spriteBatch);
            // if (_displayPos) text.DisplayText(("Si la memoire est a la tete ce que le passe, peut-on y acceder a six"), new Vector2(Position.X, Position.Y - 50), spriteBatch, Color.Black);
            // if (_displayPos) text.DisplayText(_str, new Vector2(Position.X, Position.Y - 50), spriteBatch, Color.Black);
            //text.DisplayText((Convert.ToString(_nbBlocs)), new Vector2(Position.X, Position.Y - 50), spriteBatch, Color.Red);
        }

        public void AddStr(string str)
        {
             _str += str;
        }

        public Dictionary<Ressource, int> Inventory => _inventory;
    }
}
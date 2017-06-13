﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Aviias
{
    public class Inventory
    {
        Player _player;
        public _cell[]  _cellArray;
        private Text text;
        public Craft _craft;
        public Inventory(Player player)
        {
            _player = player;
            _cellArray = new _cell[40];
            _craft = new Craft();
            for (int i = 0; i < 40; i++)
            {
                _cellArray[i] = new _cell();
                _cellArray[i]._name = "";
                _cellArray[i]._id = i;
                _cellArray[i]._ressource = new Ressource("air");

                UpdatePosition(i);
            }
        }

        public void UpdatePosition(int i)
        {
            int _difX = 77;
            float _difY = 82;
            if (i == 0)
            {
                _cellArray[i].Position = new Vector2(_player.Position.X - 325, _player.Position.Y + 49);
            }
            else
            {
                if (i > 0 && i < 10)
                {
                    _cellArray[i].Position = new Vector2(_cellArray[i - 1].Position.X + _difX, _cellArray[0].Position.Y);
                }
                else
                {
                    if ((i % 10) == 0)
                    {
                        _cellArray[i].Position = new Vector2(_cellArray[0].Position.X, _cellArray[0].Position.Y + _difY);
                        _difY = _difY + _difY;
                    }
                    else
                    {
                        _cellArray[i].Position = new Vector2(_cellArray[i - 1].Position.X + _difX, _cellArray[0].Position.Y + _difY);
                    }
                }
            }
        }

        public _cell PositionToolBar(int i)
        {
            int _difX = 77;
            if (i == 0)
            {
                _cellArray[i].Position = new Vector2(_player.Position.X - 390, _player.Position.Y + 484);
                return _cellArray[i];
            }
            else if (i > 0 && i < 10)
            {
                _cellArray[i].Position = new Vector2(_cellArray[i - 1].Position.X + _difX, _cellArray[0].Position.Y);
                return _cellArray[i];
            }
            return _cellArray[40];
        }

        public bool IsFull(int i)
        {
            if(_cellArray[i]._quantity == 0)
            {
                _cellArray[i].IsFull = false;
            }
            return _cellArray[i].IsFull;
        }

        public _cell[] Array
        {
            get { return _cellArray; }
        }

        public struct _cell
        {
            public Vector2 Position { get; set; }
            public bool IsFull { get; set; }
            public string _name { get; set; }
            public int _quantity { get; set; }
            public int _id { get; set; }
            public Ressource _ressource { get; set; }
        }

        public void AddInventory(int quantity, string name)
        {
            //foreach ( entry in _cellArray)
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i]._name == name) { _cellArray[i]._quantity += quantity; return; }
            }

            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (!_cellArray[i].IsFull && _cellArray[i]._name != name) { _cellArray[i]._name = name; _cellArray[i]._quantity = quantity; _cellArray[i].IsFull = true; _cellArray[i]._ressource = new Ressource(name); break; }
            }
        }

        public bool IsOnInventory(string name)
        {
            int i = 0;
            while (i < _cellArray.Length - 1 && _cellArray[i]._name != name)
            {               
                i++;
            }

            if (_cellArray[i]._name == name)
            {
                return true;

            }else
            {
                return false;
            }
           
        }

        public string GetName(Vector2 pos)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i].Position.X >= pos.X && _cellArray[i].Position.X < pos.X + 70 && _cellArray[i].Position.Y >= pos.Y && _cellArray[i].Position.Y < pos.Y + 69)
                {
                    return _cellArray[i]._name;
                }
            }

            return null;
        }
        

        public void DecreaseInventory(int quantity, string name)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i]._name == name)
                {
                    _cellArray[i]._quantity -= quantity;
                    return;
                }
               
            }
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            text = new Text(content);
            spriteBatch.Draw(content.Load<Texture2D>("Inventaire"), new Vector2(_player.Position.X - 400, _player.Position.Y - 400), null, Color.White, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, 0f);
            spriteBatch.Draw(content.Load<Texture2D>("babyplayer"), new Vector2(_player.Position.X - 10, _player.Position.Y - 350), null, Color.White, 0f, Vector2.Zero, 3.4f,
                SpriteEffects.None, 0f);
            for (int i=0; i<40; i++)
            {
                UpdatePosition(i);
                if (_cellArray[i]._name != "" && IsFull(i))
                {
                    spriteBatch.Draw(content.Load<Texture2D>(_cellArray[i]._name), _cellArray[i].Position, null, Color.White, 0f, Vector2.Zero, 0.8f,
                        SpriteEffects.None, 0f);
                    text.DisplayText("" + _cellArray[i]._quantity, new Vector2(_cellArray[i].Position.X, _cellArray[i].Position.Y + 100), spriteBatch, Color.Black);
                }
            }
            int count = 0;
            _craft.IsCraftable(_cellArray);
            for (int i = 0; i < _craft._cellCraft.Length; i++)
            {
                if (_craft._cellCraft[i].IsCraftable == true)
                {
                    if(count == 0)
                    {
                        spriteBatch.Draw(content.Load<Texture2D>("craft"), new Vector2(_player.Position.X + 500, _player.Position.Y - 400), null, Color.White, 0f, Vector2.Zero, 1.1f,
                            SpriteEffects.None, 0f);
                        spriteBatch.Draw(content.Load<Texture2D>(_craft._cellCraft[i]._name), new Vector2(_player.Position.X + 513, _player.Position.Y - 387), null, Color.White, 0f, Vector2.Zero, 0.8f,
                            SpriteEffects.None, 0f);
                        count++;
                    }
                    else if(count == 1)
                    {
                        spriteBatch.Draw(content.Load<Texture2D>("craft"), new Vector2(_player.Position.X + 500, _player.Position.Y - 325), null, Color.White, 0f, Vector2.Zero, 1.1f,
                            SpriteEffects.None, 0f);
                        spriteBatch.Draw(content.Load<Texture2D>(_craft._cellCraft[i]._name), new Vector2(_player.Position.X + 513, _player.Position.Y - 312), null, Color.White, 0f, Vector2.Zero, 0.8f,
                            SpriteEffects.None, 0f);
                        count++;
                    }
                    else if(count == 2)
                    {
                        spriteBatch.Draw(content.Load<Texture2D>("craft"), new Vector2(_player.Position.X + 500, _player.Position.Y - 250), null, Color.White, 0f, Vector2.Zero, 1.1f,
                            SpriteEffects.None, 0f);
                        spriteBatch.Draw(content.Load<Texture2D>(_craft._cellCraft[i]._name), new Vector2(_player.Position.X + 513, _player.Position.Y - 237), null, Color.White, 0f, Vector2.Zero, 0.8f,
                            SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }

}

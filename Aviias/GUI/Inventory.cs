using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Aviias
{
    [Serializable]
    public class Inventory
    {
        int _actualCell;
        Player _player;
        public _cell[]  _cellArray;
        private Text text;
        public Craft _craft;
        public Button _buttonCraft;
        
        public Inventory(Player player)
        {
            _player = player;
            _cellArray = new _cell[40];
            _craft = new Craft();
            _actualCell = 0;
            
            for (int i = 0; i < 40; i++)
            {
                _cellArray[i] = new _cell();
                _cellArray[i]._name = "";
                _cellArray[i]._width = 70;
                _cellArray[i]._height = 69;
                _cellArray[i]._ressource = new Ressource("air");
            }
        }

        public void UpdatePosition(int i, Camera2D camera)
        {
            int _difX = 77;
            int _difY = 80;
            float x = camera.Position.X + 650 + (i % 10) * _difX;
            float y = camera.Position.Y + 590 + (i / 10) * _difY;
            _cellArray[i].Position = new Vector2(x, y);
        }

        public _cell PositionToolBar(int i, Camera2D camera)

        {
            int _difX = 77;
            if (i == 0)
            {
                _cellArray[i].Position = new Vector2(camera.Position.X + 584, camera.Position.Y + 1022);
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

        public Vector2 PositionCellToolBar(Camera2D camera)
        {
            float DifX = 77;
            if (_actualCell == 0)
            {
                return new Vector2(camera.Position.X + 575, camera.Position.Y + 1012);
            }
            else
            {
                return new Vector2(camera.Position.X + DifX * _actualCell, camera.Position.Y + 474);
            }
        }

        public _cell[] Array
        {
            get { return _cellArray; }
        }

        public int ActualCell
        {
            get { return _actualCell; } 
            set { _actualCell = value; }           
        }

        public string GetNameBloc(int x)
        {
            return _cellArray[x]._name;
        }

        [Serializable]
        public struct _cell
        {
            [field: NonSerialized]
            public Vector2 Position;
            public bool IsFull { get; set; }
            public string _name { get; set; }
            public int _quantity { get; set; }
            public int _width { get; set; }
            public int _height { get; set; }
            public Ressource _ressource { get; set; }
        }

        public Vector2 Position { get { return Position; } set { Position = value; } }

        public void ReinitCell(int i)
        {
            _cellArray[i] = new _cell();
            _cellArray[i]._name = "";
            _cellArray[i]._quantity = 0;
            _cellArray[i].IsFull = false;
            _cellArray[i]._width = 70;
            _cellArray[i]._height = 69;
            _cellArray[i]._ressource = new Ressource("air");
        }

        public void AddInventory(int quantity, string name)
        {
            //foreach ( entry in _cellArray)
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i]._name == name && _cellArray[i]._quantity == 0)
                {
                    _cellArray[i].IsFull = true;
                    _cellArray[i]._quantity += quantity; return;
                }
                else if (_cellArray[i]._name == name)
                {
                    _cellArray[i]._quantity += quantity; return;
                }
            }

            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (!_cellArray[i].IsFull && _cellArray[i]._name != name) { _cellArray[i]._name = name; _cellArray[i]._quantity = quantity; _cellArray[i].IsFull = true; _cellArray[i]._ressource = new Ressource(name); break; }
            }
        }

        public bool IsOnInventory(string name)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
            if (_cellArray[i]._name == name && IsFull(i))
                {
                    return true;
                }
            }
            return false;         
        }

        public void ChangePlace(int i, int j)
        {
            if( i != -1)
            {
                bool full = _cellArray[i].IsFull;
                string name = _cellArray[i]._name;
                int quantity = _cellArray[i]._quantity;
                Ressource res = _cellArray[i]._ressource;

                _cellArray[i].IsFull = _cellArray[j].IsFull;
                _cellArray[i]._name = _cellArray[j]._name;
                _cellArray[i]._quantity = _cellArray[j]._quantity;
                _cellArray[i]._ressource = _cellArray[j]._ressource;

                _cellArray[j].IsFull = full;
                _cellArray[j]._name = name;
                _cellArray[j]._quantity = quantity;
                _cellArray[j]._ressource = res;
            }
            
        }

        public int Quantity(string name)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if (_cellArray[i]._name == name && IsFull(i))
                {
                    return _cellArray[i]._quantity;
                }
            }
            return -1;
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

        public bool PutableBloc(List<string> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == name)
                {
                    return false;
                }
            }

            return true;
        }

        public void DecreaseInventory(int quantity, string name)
        {
            for (int i = 0; i < _cellArray.Length; i++)
            {
                if(_cellArray[i]._name == name && _cellArray[i]._quantity == 1)
                {
                    _cellArray[i].IsFull = false;
                    _cellArray[i]._quantity -= quantity;
                    return;
                }
                else if (_cellArray[i]._name == name)
                {
                    _cellArray[i]._quantity -= quantity;
                    return;
                }
               
            }
        }

        public void PositionCraft(int i, Camera2D camera, int count)
        {
            if (count == 0 && _craft._cellCraft[i].IsCraftable)
            {
                _craft._cellCraft[i]._position = new Vector2(camera.Position.X + 1475, camera.Position.Y + 140);
            }
            else if (_craft._cellCraft[i].IsCraftable)
            {
                _craft._cellCraft[i]._position = new Vector2(_craft._cellCraft[i - 1]._position.X, _craft._cellCraft[i - 1]._position.Y + 75);
            }
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content, Camera2D camera)
        {
            _craft.IsCraftable(_cellArray);
            text = new Text(content);
            spriteBatch.Draw(content.Load<Texture2D>("Inventaire"), new Vector2(camera.Position.X + 576, camera.Position.Y + 140), null, Color.White, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, 0f);
            spriteBatch.Draw(content.Load<Texture2D>("babyplayer"), new Vector2(camera.Position.X + 965, camera.Position.Y + 190), null, Color.White, 0f, Vector2.Zero, 3.4f,
                SpriteEffects.None, 0f);
            for (int i=0; i<40; i++)
            {
                UpdatePosition(i, camera);
                if (_cellArray[i]._name != "" && IsFull(i))
                {
                    spriteBatch.Draw(content.Load<Texture2D>(_cellArray[i]._name), _cellArray[i].Position, null, Color.White, 0f, Vector2.Zero, 0.8f,
                        SpriteEffects.None, 0f);
                    text.DisplayText("" + _cellArray[i]._quantity, new Vector2(_cellArray[i].Position.X, _cellArray[i].Position.Y + 100), spriteBatch, Color.Black);
                }
               // spriteBatch.Draw(content.Load<Texture2D>("bedrock"), _cellArray[i].Position, null, Color.White, 0f, Vector2.Zero, 1f,
                 //       SpriteEffects.None, 0f);
            }
            int count = 0;
            for (int i = 0; i < _craft._cellCraft.Length; i++)
            {
                _craft.IsCraftable(_cellArray);
                if (_craft._cellCraft[i].IsCraftable)
                {
                    PositionCraft(i, camera, count);
                    _buttonCraft = new Button(_craft._cellCraft[i]._position, 70, 69, "craft", _craft._cellCraft[i]._name);
                    _buttonCraft.Draw(spriteBatch, content);
                    spriteBatch.Draw(content.Load<Texture2D>(_craft._cellCraft[i]._name), new Vector2(_craft._cellCraft[i]._position.X + 12, _craft._cellCraft[i]._position.Y +12), null, Color.White, 0f, Vector2.Zero, 0.8f,
                            SpriteEffects.None, 0f);
                    count++;
                }
            }
        }
    }

}

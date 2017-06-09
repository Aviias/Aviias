using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MonoGame.Extended;

namespace Aviias
{
    public class Player
    {
        Map _ctx;
        Texture2D PlayerTexture;
        public Vector2 Position;
        bool Active;
        int _health;
        Text text;
        public bool _displayPos;
        string _str;
        public List<Quest> _activeQuest;
        internal Dictionary<Ressource, int> _inventory;
        int _resistance;
        int _damage;
        bool _isDie;
        List<int> list = new List<int>(16);

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        List<Monster> monsters = new List<Monster>();
        MouseState mouseState = Mouse.GetState();
        public bool isInAir;
        public float _yVelocity;
        public double _gravity;
        public float _jumpHeight;
        bool _collisions;
        int _nbBlocs;
        float _moveSpeed;
        public bool flyMod;
        public bool IsInventoryOpen;
        Map map;

        float _playerTimer = 1.2f;
        const float _playerTIMER = 1.2f;

        float _inventoryTimer = 1.3f;
        const float _inventoryTIMER = 1.3f;

        float _craftTimer = 1.5f;
        const float _craftTIMER = 1.5f;

        float _blocBreakTimer = 1.5f;
        const float _blocBreakTIMER = 1.5f;
        float _blockDurationTimer = 1.5f;
        const float _blockDurationTIMER = 1.5f;
        //   MonoGame.Extended.Camera2D Camera;
        float _playerMoveSpeed;

        internal Inventory _inv;

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

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public int Resistance
        {
            get { return _resistance; }
            set { _resistance = value; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public void Initialize(Texture2D texture, Vector2 position, ContentManager content)
        {
            PlayerTexture = texture;
            Position = position;
            Active = true;
            _resistance = 0;
            _damage = 20;
            _health = 100;
            text = new Aviias.Text(content);
            _displayPos = true;
            _isDie = false;
            _gravity = 1;
            _jumpHeight = -12;
            _moveSpeed = 0.8f;
            _activeQuest = new List<Quest>(8);
            _inv = new Inventory(this);
            _inv.AddInventory(2, "oak_wood");
            _inv.AddInventory(4, "oak_plank");
            _inv.AddInventory(500, "dirt");
            _inv.AddInventory(70, "stone");
            _inv.AddInventory(12, "bedrock");
            _inv.AddInventory(45, "bookshelf");
            _inv.AddInventory(27, "coal_ore");
            _inv.AddInventory(117, "glass");
            _inv.AddInventory(80, "iron_ore");
            _inv.AddInventory(1000, "stonebrick");
            _inv.AddInventory(247, "oak_leaves");
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
        /*
        public Vector2 CursorPos()
        {             
            int posX = Cursor.Position.X;
            int posY = Cursor.Position.Y;

            Vector2 cursorPos = new Vector2(posX, posY);
            return cursorPos;
            
        }
        */
        public float PlayerMoveSpeed
        {
            get { return _playerMoveSpeed; }
            set { _playerMoveSpeed = value; }
        }

        public bool IsDie
        {
            get { return _isDie; }
            set { _isDie = value; }
        }

        public void GetDamage(int damage)
        {
            int newHealth = _health - (damage - _resistance);
            if (newHealth <= 0)
            {
                _isDie = true;
            }
            else
            {
                _health = newHealth;
            }
        }

        public void breakBloc(Bloc bloc, ContentManager content, Bloc[,] blocs, int i, int j, int scale, StreamWriter log)
        {
            Bloc bloc1;
            if (bloc != null)
            {
                //log.WriteLine("---- > breakBloc i=" + i + " j=" + j);
                if (bloc.IsBreakable)
                {
                    bloc1 = new Bloc(blocs[i,j].GetPosBlock ,scale, "air", content);
                    blocs[i, j] = bloc1;
                    //log.WriteLine("---- > breakBloc block.X = " + blocs[i, j].GetPosBlock.X + " block.Y = " + blocs[i, j].GetPosBlock.Y);
                }
            }
        }

        bool IsOnLadder(Map map)
        {
            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 3; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 1; b++)
                {
                    if (map._blocs[b, a] != null && map._blocs[b, a].Type == "ladder") return true;
                }
            }
            return false;
        }

        internal void Jump(Map map)
        {
            if (!IsInAir(map) && !IsOnLadder(map))
            {
                _yVelocity = _jumpHeight;
                Position.Y -= 10;
            }
        }

        internal void Update(Map map)
        {
            if (!flyMod && !IsOnLadder(map))
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

        internal void Update(Player player, Camera2D Camera, List<NPC> _npc, GameTime gameTime, ContentManager Content, StreamWriter log, Map map, List<Monster> monsters)
        {
            currentKeyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            list = GetCollisionSide(GetBlocsAround(map));
            
            _inventoryTimer -= elapsed;
            _craftTimer -= elapsed;
            _playerTimer -= elapsed;
            _blocBreakTimer -= elapsed;

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (!list.Contains(2)) Position.X -= _playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (!list.Contains(1)) Position.X += _playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                Position.Y -= _playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                Camera.Move(new Vector2(0, +_playerMoveSpeed));
                player.Position.Y += _playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.E) && _inventoryTimer < 1)
            {
                IsInventoryOpen = !IsInventoryOpen;
                _inventoryTimer = _inventoryTIMER;
            }

            if (currentKeyboardState.IsKeyDown(Keys.C) && _craftTimer < 1 && IsInventoryOpen)
            {
                for (int i = 0; i < _inv._craft._cellCraft.Length; i++)
                {
                    if (_inv._craft._cellCraft[i].IsCraftable == true)
                    {
                        _inv.AddInventory(_inv._craft._cellCraft[i]._quantity, _inv._craft._cellCraft[i]._name);

                        foreach (KeyValuePair<int, Ressource> element in _inv._craft._cellCraft[i]._ressource)
                        {
                            _inv.DecreaseInventory(element.Key, element.Value.Name);
                        }
                        break;
                    }
                }
                _craftTimer = _craftTIMER;

            }


            if (currentKeyboardState.IsKeyDown(Keys.Space) /*|| currentKeyboardState.IsKeyDown(Keys.Up)*/)
            {
                Jump(map);
            }

               if ((System.Windows.Forms.Control.MouseButtons & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left)
               {

                mouseState = Mouse.GetState();
                Vector2 position = new Vector2(mouseState.X, mouseState.Y);
                position = Camera.ScreenToWorld(position);

                for (int i = 0; i < monsters.Count; i++)
                {
                    if (position.X >= monsters[i].MonsterPosition.X && position.X <= monsters[i].MonsterPosition.X + monsters[i].Width && position.Y >= monsters[i].MonsterPosition.Y && position.Y <= monsters[i].MonsterPosition.Y + monsters[i].Height)
                    {
                        if (_playerTimer < 1 && Vector2.Distance(player.PlayerPosition, position) <= 400)
                        {
                            monsters[i].GetDamage(player.Damage);
                            if (monsters[i].IsDie)
                            {
                                monsters.Remove(monsters[i]);
                            }
                            _playerTimer = _playerTIMER;
                        }
                            
                    }
                }
                
                if (mouseState.LeftButton == ButtonState.Pressed && _blocBreakTimer < 1)
                {
                    _blockDurationTimer -= elapsed;
                    //if (_blockDurationTimer < 1)
                    //{
                        map.FindBreakBlock(position, player, Content, log);
                        _blocBreakTimer = _blocBreakTIMER;
                        _blockDurationTimer = _blockDurationTIMER;
                    //}
                    
                }
              
            }

            if (currentKeyboardState.IsKeyDown(Keys.P))
            {
                if (player._displayPos) player._displayPos = false;
                else player._displayPos = true;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                player.AddStr("a");
            }
            if (currentKeyboardState.IsKeyDown(Keys.I))
            {
                foreach (NPC npc in _npc) if (map.GetDistance(player.PlayerPosition, npc.Position) < 400) npc.Interact(player);
            }

            if (currentKeyboardState.IsKeyDown(Keys.G))
            {
                flyMod = !flyMod;
            }

            if (currentKeyboardState.IsKeyDown(Keys.N))
            {
                for(int i = 0; i < _inv._cellArray.Length; i++)
                {
                    if(_inv._cellArray[i]._name == "heal_potion" && _inv._cellArray[i]._quantity >= 1)
                    {
                        RegenerateHealth(50);
                        _inv.DecreaseInventory(1, "heal_potion");
                    }
                }
            }
        }

        internal void RegenerateHealth(int quantity)
        {
            Health += quantity;
            if (Health > 100) Health = 100;
        }

        internal void UpdatePlayerCollision(GameTime gameTime, Player player, List<Monster> monsters)
        {
            Rectangle playerRect;
            Rectangle monsterRect;
            

            playerRect = new Rectangle((int)player.X, (int)player.Y, player.Width, player.Height);

            for (int i = 0; i < monsters.Count; i++)
            {
                monsterRect = new Rectangle((int)monsters[i].posX, (int)monsters[i].posY, monsters[i].Width, monsters[i].Height);
                if (playerRect.Intersects(monsterRect))
                {
                    // Collision between player and monster
                    if (Math.Abs(playerRect.Center.X - monsterRect.Center.X) > Math.Abs(playerRect.Center.Y - monsterRect.Center.Y))
                    {
                        if (playerRect.Center.X < monsterRect.Center.X)
                        {
                            monsters[i].posX = playerRect.Right - monsters[i].moveSpeed;
                        }
                        if (playerRect.Center.X > monsterRect.Center.X)
                        {
                            monsters[i].posX = playerRect.Left - monsters[i].Width - monsters[i].moveSpeed;
                        }
                    }
                    else
                    {
                        if (playerRect.Center.Y < monsterRect.Center.Y)
                        {
                            monsters[i].posY = playerRect.Bottom - monsters[i].moveSpeed;
                        }
                        if (playerRect.Center.Y > monsterRect.Center.Y)
                        {
                            monsters[i].posY = playerRect.Top - monsters[i].Height - monsters[i].moveSpeed;
                        }
                    }
                }
            }

        }
            internal void UpdateCollision(Map map, Player player) {

            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);
            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 8; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 8; b++)
                {
                    if (a >= 0 && b >= 0 && a < map._worldHeight && b < map._worldWidth && map._blocs[b, a] != null && map._blocs[b, a].Type != "air")
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

            Rectangle rectTest = new Rectangle((int)Position.X, (int)Position.Y - 10, PlayerTexture.Width, PlayerTexture.Height);

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
                            if(!rectTest.Intersects(blocRect)) Position.Y -= 1;
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

        internal List<Bloc> GetBlocsAround(Map map)
        {
            _nbBlocs = 0;

            List<Bloc> _blocs = new List<Bloc>(16);

            for (int a = (int)(Position.Y / 16); a < (Position.Y / 16) + 8; a++)
            {
                for (int b = (int)(Position.X / 16); b < (Position.X) / 16 + 8; b++)
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

        public string ImageHealth(int health)
        {
            return (Math.Floor((double)health / 10)*10).ToString();
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
            spriteBatch.Draw(content.Load<Texture2D>(ImageHealth(_health)), new Vector2(Position.X - 950, Position.Y - 500), null, Color.White, 0f, Vector2.Zero, 1.1f,
               SpriteEffects.None, 0f);
            if (_displayPos) text.DisplayText((Position.X  + " - " + Position.Y), new Vector2(Position.X, Position.Y - 30), spriteBatch, Color.Red);
            text.DisplayText(("" +_health + "/"  + "100"), new Vector2(Position.X - 785, Position.Y - 420), spriteBatch, Color.White);

            if(IsInventoryOpen)
            {
                _inv.Draw(spriteBatch, content);
            }else
            {
                spriteBatch.Draw(content.Load<Texture2D>("Barre d'inventaire"), new Vector2(Position.X - 400, Position.Y +474), null, Color.White, 0f, Vector2.Zero, 1f,
                    SpriteEffects.None, 0f);
                for(int i=0; i<10; i++)
                {
                    if (_inv.PositionToolBar(i).IsFull == true)
                    {
                        spriteBatch.Draw(content.Load<Texture2D>(_inv.PositionToolBar(i)._name), _inv.PositionToolBar(i).Position, null, Color.White, 0f, Vector2.Zero, 0.8f,
                            SpriteEffects.None, 0f);
                        text.DisplayText("" + _inv.PositionToolBar(i)._quantity, new Vector2(_inv.PositionToolBar(i).Position.X, _inv.PositionToolBar(i).Position.Y + 100), spriteBatch, Color.Black);
                    }
                }
            }        
        }

        public void AddStr(string str)
        {
             _str += str;
        }

        public Dictionary<Ressource, int> Inventory => _inventory;
    }
}
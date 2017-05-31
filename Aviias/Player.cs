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
        List<Quest> _activeQuest;
        List<Ressource> _inventory;
        int _resistance;
        int _damage;
        bool _isDie;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        List<Monster> monsters = new List<Monster>();
        MouseState mouseState = Mouse.GetState();
        Map map;

        float _playerTimer = 1.2f;
        const float _playerTIMER = 1.2f;

        float _blocBreakTimer = 1.5f;
        const float _blocBreakTIMER = 1.5f;
        float _blockDurationTimer = 1.5f;
        const float _blockDurationTIMER = 1.5f;
        //   MonoGame.Extended.Camera2D Camera;
        float _playerMoveSpeed;
        



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
        }

        public Vector2 PlayerPosition
        {
            get { return Position; }
        }

        public void AddQuest(Quest quest)
        {
            _activeQuest.Add(quest);
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

        internal void UpdatePlayer(GameTime gameTime, List<Monster> monsters, Map map, Player player, ContentManager Content, StreamWriter log, List<NPC> _npc, Camera2D Camera)
        {

            //player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            //player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
            currentKeyboardState = Keyboard.GetState();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            _playerTimer -= elapsed;
            _blocBreakTimer -= elapsed;
            

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                Camera.Move(new Vector2(-_playerMoveSpeed, 0));
                player.Position.X -= _playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                Camera.Move(new Vector2(+_playerMoveSpeed, 0));
                player.Position.X += _playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                Camera.Move(new Vector2(0, -_playerMoveSpeed));
                player.Position.Y -= _playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                Camera.Move(new Vector2(0, +_playerMoveSpeed));
                player.Position.Y += _playerMoveSpeed;
            }

            if ((System.Windows.Forms.Control.MouseButtons & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left)
            {
                MouseState mouseState = Mouse.GetState();
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
                    if (_blockDurationTimer < 1)
                    {
                        map.FindBreakBlock(position, player, Content, log);
                        _blocBreakTimer = _blocBreakTIMER;
                        _blockDurationTimer = _blockDurationTIMER;
                    }
                    
                }
               
                   
                 
                    
                //log.WriteLine("mouse.X = " + position.X + ", mouse.Y = " + position.Y);
                //log.WriteLine("camera.X = " + _camera.Position.X + ", camera.Y = " + _camera.Position.Y);
                //map.DebugBloc(0, 0, log);


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
                foreach (NPC npc in _npc) npc.Interact(player);
            }


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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PlayerTexture, Position, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);

              if (_displayPos) text.DisplayText((Position.X + " - " + Position.Y), new Vector2(Position.X, Position.Y - 30), spriteBatch, Color.Red);
            text.DisplayText(("Life : " + _health), new Vector2(Position.X, Position.Y - 60), spriteBatch, Color.Orange);
            // if (_displayPos) text.DisplayText(((int)Position.X/64 + " - " + (int)Position.Y/64), new Vector2(Position.X, Position.Y - 50), spriteBatch);
            // if (_displayPos) text.DisplayText(("Si la memoire est a la tete ce que le passe, peut-on y acceder a six"), new Vector2(Position.X, Position.Y - 50), spriteBatch, Color.Black);
            if (_displayPos) text.DisplayText(_str, new Vector2(Position.X, Position.Y - 50), spriteBatch, Color.Black);
        }

        public void AddStr(string str)
        {
             _str += str;
        }

    }
}

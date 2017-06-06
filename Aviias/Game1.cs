﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Aviias
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Monster monster;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        float playerMoveSpeed;
        // Texture2D texture;
        Map map = new Map(200, 200);
        Random random = new Random();
        int prob = 3;
        BoxingViewportAdapter _viewportAdapter;
        const int WindowWidth = 1920;
        const int WindowHeight = 1080;
        Camera2D _camera;
        SpriteFont msg_font;
        public List<NPC> _npc;
        SpriteFont font;
        List<Monster> monsters = new List<Monster>();
        MouseState mouseState = Mouse.GetState();
        Bloc bloc;
        List<int> list = new List<int>(16);
        Ressource _testRessource = new Ressource();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.PreferredBackBufferWidth = WindowWidth;

            Window.AllowUserResizing = true;
        }

        public Camera2D Camera
        {
            get
            {
                return _camera;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player();
            playerMoveSpeed = 6.0f;
            monster = new Monster(100, 1.0f, 0.05, 10, 5 );

            // Add a new monster in the list
            monsters.Add(monster);
            base.Initialize();
            map.GenerateMap(Content);
            this.IsMouseVisible = true;

            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, WindowWidth, WindowHeight);
            _camera = new Camera2D(_viewportAdapter);
            _camera.LookAt(new Vector2(player.Position.X + 10, player.Position.Y + 15));
            _npc = new List<NPC>(8);
            _npc.Add(new NPC(Content, "pnj", spriteBatch, new Vector2(500, 250), 1));
            _npc.Add(new NPC(Content, "pnj", spriteBatch, new Vector2(1400, 300), 0));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
           
            Vector2 playerPosition = new Vector2(1500, 45 + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("tt"), playerPosition, Content);

            Vector2 monsterPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            monster.Initialize(Content.Load<Texture2D>("mob"), monsterPosition);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.M))
            {
                map = new Map(200, 200);
                map.GenerateMap(Content);
            }
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
           
            UpdatePlayer(gameTime);          
            UpdateMonster(gameTime);
            UpdateCollision(gameTime);
            player.Update(map);
            player.UpdateCollision(map, player);
            Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);
            base.Update(gameTime);
        }

        private void UpdateCollision(GameTime gameTime)
        {
            Rectangle playerRect;
            Rectangle monsterRect;

            playerRect = new Rectangle((int)player.X, (int)player.Y, player.Width, player.Height);

            for(int i = 0; i<monsters.Count; i++)
            {
                monsterRect = new Rectangle((int)monsters[i].posX, (int)monsters[i].posY, monsters[i].Width, monsters[i].Height);
                if(playerRect.Intersects(monsterRect))
                {
                    // Collision between player and monster
                    if (Math.Abs(playerRect.Center.X - monsterRect.Center.X) > Math.Abs(playerRect.Center.Y - monsterRect.Center.Y) )
                    {
                        if (playerRect.Center.X < monsterRect.Center.X)
                        {
                            monster.posX = playerRect.Right - monster.moveSpeed;
                        }
                        if (playerRect.Center.X > monsterRect.Center.X)
                        {
                            monster.posX = playerRect.Left - monster.Width - monster.moveSpeed;
                        }
                    }
                    else
                    {
                        if (playerRect.Center.Y < monsterRect.Center.Y)
                        {
                            monster.posY = playerRect.Bottom - monster.moveSpeed;
                        }
                        if (playerRect.Center.Y > monsterRect.Center.Y)
                        {
                            monster.posY = playerRect.Top - monster.Height - monster.moveSpeed;
                        }
                    }
                }   
            }
        }

        private void UpdateMonster(GameTime gameTime)
        {
            monster.Update(player);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            list = player.GetCollisionSide(player.GetBlocsAround(map));

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (!list.Contains(2)) player.Position.X -= playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (!list.Contains(1)) player.Position.X += playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                player.Position.Y -= playerMoveSpeed;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (!list.Contains(3)) player.Position.Y += playerMoveSpeed;
            }

            if ((System.Windows.Forms.Control.MouseButtons & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left)
            {
                
                    Vector2 posBloc = new Vector2(mouseState.X, mouseState.Y);
                    map.FindBreakBlock(posBloc, player, Content);
                    //player.breakBloc(bloc, player.CursorPos(), Content);
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
                foreach (NPC npc in _npc)
                {
                    if (map.GetDistance(player.PlayerPosition, npc.Position) < 400) npc.Interact(player);
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                player.Jump(map);
            }

            if (currentKeyboardState.IsKeyDown(Keys.G))
            {
                player.flyMod = !player.flyMod;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
            map.Draw(spriteBatch, (int)player.Position.X, (int)player.Position.Y);
            monster.Draw(spriteBatch);
            player.Draw(spriteBatch);
            //   foreach (NPC npc in _npc) if (npc._isTalking) npc.Talk(new Quest(), spriteBatch);
            foreach (NPC npc in _npc)
            {
                npc.Draw(spriteBatch);
                npc.Update();
            }
      /*        if (list.Contains(1)) spriteBatch.DrawString(font, "1", new Vector2(player.Position.X - 20, player.Position.Y - 20), Color.Red);
            foreach (KeyValuePair<Ressource, int> entry in player._inventory)
            {
                if (entry.Key.Name == "dirt")
                {
                    spriteBatch.DrawString(font, entry.Value.ToString(), new Vector2(player.Position.X - 10, player.Position.Y - 100), Color.Black);
                }
            }
           spriteBatch.DrawString(font,Convert.ToString(player.CursorPos().X), new Vector2(10, 10), Color.Red);
            spriteBatch.DrawString(font, Convert.ToString(player.CursorPos().Y), new Vector2(200, 10), Color.Red);
            if (bloc != null)
            {
                spriteBatch.DrawString(font, Convert.ToString(bloc.GetPosBlock.X), new Vector2(10, 50), Color.Red);
                spriteBatch.DrawString(font, Convert.ToString(bloc.GetPosBlock.Y), new Vector2(200, 50), Color.Red);
            }
            if (list.Contains(2)) spriteBatch.DrawString(font, "2", new Vector2(player.Position.X - 20, player.Position.Y - 40), Color.Red);
            if (list.Contains(3)) spriteBatch.DrawString(font, "3", new Vector2(player.Position.X - 20, player.Position.Y - 60), Color.Red);
            if (list.Contains(4)) spriteBatch.DrawString(font, "4", new Vector2(player.Position.X - 20, player.Position.Y - 80), Color.Red);*/
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

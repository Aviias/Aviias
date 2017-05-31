﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.IO;
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
        // Texture2D texture;
        Map map = new Map(200, 200);
        Random random = new Random();
        BoxingViewportAdapter _viewportAdapter;
        const int WindowWidth = 1920;
        const int WindowHeight = 1088;
        Camera2D _camera;
        List<NPC> _npc;
        SpriteFont font;
        List<Monster> monsters = new List<Monster>();
        StreamWriter log; // Debug file
        Random rnd = new Random();

        float _spawnTimer = 10f;
        const float _spawnTIMER = 10f;

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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            player = new Player();
            player.PlayerMoveSpeed = 8.0f;
            Vector2 monsterPosition;
            
            int monsterneed = 5 - monsters.Count;
            if (monsterneed != 0)
            {
                for (int i = 0; i < monsterneed; i++)
                {
                    int posX = rnd.Next(0, map.WorldWidth * 10);
                    int posY = rnd.Next(0, map.WorldHeight * 10);
                    monsterPosition = new Vector2(posX, posY);
                    monster = new Monster(100, 1.0f, 0.05, 1, 5, Content, Content.Load<Texture2D>("alienmonster"), monsterPosition);   
                    monsters.Add(monster);
                }
            }
            
            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }
            
            // Add a new monster in the list            
            base.Initialize();
            map.GenerateMap(Content);
            IsMouseVisible = true;
            
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, WindowWidth, WindowHeight);
            _camera = new Camera2D(_viewportAdapter);
            _camera.LookAt(new Vector2(player.Position.X + 10, player.Position.Y + 15));
            _npc = new List<NPC>(8);
            _npc.Add(new NPC(Content, "pnj", spriteBatch));
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
           
            Vector2 playerPosition = new Vector2(1500, 945 + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("babyplayer"), playerPosition, Content);

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
            {
                log.Close(); // Debug close
                Exit();
            }

            Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);
            // TODO: Add your update logic here
            currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.M))
            {
                map = new Map(200, 200);
                map.GenerateMap(Content);
            }

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].IsDie == false && monsters[i] != null)
                {
                    monsters[i].Update(player, gameTime);
                }
                else
                {
                    monsters.RemoveAt(i);
                }
                    
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            _spawnTimer -= elapsed;

            if (_spawnTimer < 1)
            {
                int posX = rnd.Next(0, map.WorldWidth * 10);
                int posY = rnd.Next(0, map.WorldHeight * 10);
                Vector2 monsterPosition = new Vector2(posX, posY);
                monster = new Monster(100, 1.0f, 0.05, 1, 5, Content, Content.Load<Texture2D>("alienmonster"), monsterPosition);               
                monsters.Add(monster);
                _spawnTimer = _spawnTIMER;
            }

            player.UpdatePlayer(gameTime, monsters, map, player, Content, log, _npc, _camera);          
            player.UpdatePlayerCollision(gameTime, player, monsters);
            base.Update(gameTime);
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
            for (int i = 0; i < monsters.Count; i++)
            {
               monsters[i].Draw(spriteBatch);
            }
            if (player.IsDie == false)
            {
                player.Draw(spriteBatch);
            }
            
            foreach (NPC npc in _npc) npc.Draw(spriteBatch);
            foreach (NPC npc in _npc) if (npc._isTalking) npc.Talk(new Quest(), spriteBatch);
            

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

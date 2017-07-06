﻿using Aviias.IA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aviias
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Player player;
   //     Monster monster;
        Wolf wolf;
        Drake drake;
        MouseState mouseState = Mouse.GetState();
        Gloutogobe glouto;
        Button _die;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        // Texture2D texture;
        public static Map map;
        public static Random random = new Random();
        BoxingViewportAdapter _viewportAdapter;
        BoxingViewportAdapter _viewportInterface;
        const int WindowWidth = 1920;
        const int WindowHeight = 1080;
        Camera2D _camera;
        Camera2D _interface;
        internal static List<NPC> _npc;
        SpriteFont font;
        List<Monster> monsters = new List<Monster>();
        StreamWriter log; // Debug file
        Random rnd = new Random();
        Timer spawnTimer = new Timer(10f);
        Timer phenixTimer = new Timer(90f);
        Menu _menu;
        MenuPlay _menuPlay;
        List<int> list = new List<int>(16);
        Texture2D _gameover;
        Timer _nightDay = new Timer(150f);
        //Ressource _testRessource = new Ressource();
        Spawn spawnMonster;
        internal Genetic genetic = new Genetic();
        static internal Song sAmbiant;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            Window.Position = Point.Zero;
        }

        public Camera2D Camera
        {
            get
            {
                return _camera;
            }
        }

        internal Genetic Genetic => genetic;

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player();
            player.PlayerMoveSpeed = 8.0f;
            Vector2 monsterPosition;
            map = new Map(200, 500);
            _menu = new Menu();
            _menu.Initialize();
           
            
           
            
            Vector2 playerPosition = new Vector2(1500, 345 + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("face"), playerPosition, Content, map);
            map.GenerateMap(Content);
            map.ActualizeShadow((int)player.Position.X, (int)player.Position.Y);
            IsMouseVisible = true;

            _npc = new List<NPC>(8);
            _npc.Add(new NPC(Content, "Face1", spriteBatch, new Vector2(500, 250), 5));
            _npc.Add(new NPC(Content, "Face2", spriteBatch, new Vector2(1400, 300), 3));

            spawnMonster = new Spawn(map);
            int monsterneed = 8 - monsters.Count;
            if (monsterneed != 0)
            {
                for (int i = 0; i < monsterneed; i++)
                {
                    monsterPosition = spawnMonster.SpawnOnSurface(map);
                    wolf = new Wolf(Content, Content.Load<Texture2D>("Wolfface"), monsterPosition, new ushort[5] { 10, 10, 10, 10, 10 });
                    monsters.Add(wolf);
                }
            }
            
            Vector2 gloutoPos = spawnMonster.SpawnOnSurface(map);
            glouto = new Gloutogobe(Content, Content.Load<Texture2D>("wolfface"), gloutoPos, new ushort[5] { 10, 10, 10, 10, 10 });
            glouto.LoadContent(Content, "wolface", "wolfleft", "wolfright", 50f, 3);
            monsters.Add(glouto);
            
            base.Initialize();

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();

            _camera.LookAt(new Vector2(player.Position.X + 10, player.Position.Y + 15));
            MediaPlayer.Play(sAmbiant);
            MediaPlayer.IsRepeating = true;
            
            RunGeneration();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, WindowWidth, WindowHeight);
            _camera = new Camera2D(_viewportAdapter);
            _interface = new Camera2D(_viewportAdapter);

            for (int i = 0; i < _npc.Count; i++)
            {
                _npc[i].LoadContent(Content);
            }

            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i].Type() == "wolf")
                {
                    monsters[i].LoadContent(Content, "Wolfface", "Wolfleft", "Wolfright", 50f, 5);
                }
            }
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);         

            font = Content.Load<SpriteFont>("msg_font");
            player.LoadContent(Content);
            _gameover = Content.Load<Texture2D>("gameover");
            sAmbiant = Content.Load<Song>("Sounds/ambiant");
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
            phenixTimer.Decrem(gameTime);
            mouseState = Mouse.GetState();
            _die = new Button(new Vector2(Camera.Position.X + 838, Camera.Position.Y + 883), 600, 100, "v", "v");
            Vector2 position = new Vector2(mouseState.X, mouseState.Y);
            position = Camera.ScreenToWorld(position);
            map.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || _menu._close)
            {
                player.Save(map);
                Exit();
            }

            if (_menu.Jouer() == false)
            {
                _menu.Update(gameTime, Content);
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Pressed && player.IsDie)
                {
                    if (position.X >= _die._position.X && position.Y >= _die._position.Y && position.X <= _die._position.X + _die._width && position.Y <= _die._position.Y + _die._height)
                    {
                        player.IsDie = false;
                        player.Health = 100;
                    }
                }
                if (player.IsDie == false)
                {
                   // Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);
                    // TODO: Add your update logic here

                    previousKeyboardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();

                    player.Update(map);
                    player.UpdatePlayerCollision(gameTime, player, monsters);
                    if (player.Position.X >= map._worldWidth*16 - WindowWidth / 2 - 64) Camera.Position = new Vector2(map._worldWidth*16 - WindowWidth - 64, player.Position.Y - WindowHeight / 2);
                    else if (player.Position.X <= WindowWidth / 2 + 64) Camera.Position = new Vector2(64, player.Position.Y - WindowHeight / 2);
                    else Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);

                    for (int i = 0; i < monsters.Count; i++)
                    {
                        if (monsters[i] != null && monsters[i].IsDie == false && monsters[i].Type() != "glouto")
                        {
                          monsters[i].Update(player, gameTime, map);
                        }

                        list = player.GetCollisionSide(player.GetBlocsAround(map));

                        Camera.Move(new Vector2(-player.PlayerMoveSpeed, 0));
                    }

                    
                    if (glouto.StepEvolve == 1)
                    {
                        glouto.LoadContent(Content, "wolface", "wolfleft", "wolfright", 50f, 3); ;
                    }
                    if (glouto.StepEvolve == 2)
                    {
                        glouto.LoadContent(Content, "boeuface", "boeufleft", "boeufright", 50f, 3);
                    }
                    if (glouto.StepEvolve == 3)
                    {
                        glouto.LoadContent(Content, "larveface", "larveleft", "larveright", 50f, 3);
                    }
                    glouto.Update(monsters, player, Content, gameTime, map);

                    player.Update(player, Camera, _npc, gameTime, Content, log, map, monsters);
                    player.UpdatePlayerCollision(gameTime, player, monsters);
                    foreach (NPC npc in _npc) npc.Update(gameTime, spriteBatch);
                    base.Update(gameTime);

                    spawnTimer.Decrem(gameTime);
                    _nightDay.Decrem(gameTime);
                    if (_nightDay.IsDown())
                    {
                        map.TimeForward();
                        _nightDay.ReInit();
                        map.ActualizeShadow((int)player.Position.X, (int)player.Position.Y);
                    }
                    if (phenixTimer.IsDown())
                    {
                        Vector2 monsterPosition = spawnMonster.SpawnOnSurface(map);
                        drake = new Drake(Content, Content.Load<Texture2D>("phenixface"), monsterPosition, new ushort[5] { 10, 10, 10, 10, 10 });
                        drake.LoadContent(Content, "phenixface", "phenixleft", "phenixright", 80f, 4);
                        monsters.Add(drake);

                        Vector2 gloutoPos = spawnMonster.SpawnOnSurface(map);
                        glouto = new Gloutogobe(Content, Content.Load<Texture2D>("wolfface"), gloutoPos, new ushort[5] { 10, 10, 10, 10, 10 });
                        glouto.LoadContent(Content, "wolface", "wolfleft", "wolfright", 50f, 3);
                        monsters.Add(glouto);

                        phenixTimer.ReInit();
                    }

                    if (spawnTimer.IsDown())
                    {                                                   
                                               
                        int monsterneed = 8 - monsters.Count;
                        if (monsterneed != 0)
                        {
                            RunGeneration();
                            for (int i = 0; i < monsterneed; i++)
                            {                              
                                Vector2 monsterPosition = spawnMonster.SpawnOnSurface(map);
                                ushort[] test = new ushort[5];
                                string[] strTest = new string[5];
                                string strTestt;
                                strTestt = (genetic.Population.BestChromosome.ToString());
                                strTest = strTestt.Split(' ');
                                for (int j = 0; j < test.Length; j++) test[j] = Convert.ToUInt16(strTest[j]);
                              //  test = (Array)genetic.Population.BestChromosome;
                                // wolf = new Wolf(Content, Content.Load<Texture2D>("Wolfface"), monsterPosition, new ushort[5] { 10, 10, 10, 10, 10 });
                                wolf = new Wolf(Content, Content.Load<Texture2D>("Wolfface"), monsterPosition, test);
                                wolf.LoadContent(Content, "Wolfface", "Wolfleft", "Wolfright", 50f, 5);
                                monsters.Add(wolf);
                            }

                            spawnTimer.ReInit();
                        }
                    }
                }

            }
        }

        public void RunGeneration()
        {
            genetic.RunGeneration();
        }

        public void Reload()
        {
            sAmbiant = Content.Load<Song>("Sounds/ambiant");
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var viewMatrix = _camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, 0, 0f, -1f);

            if (_menu.Jouer() == false)
            {
                spriteBatch.Begin();
                _menu.Draw(spriteBatch, Content);
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Silver);

                spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
                map.Draw(spriteBatch, (int)player.Position.X, (int)player.Position.Y);
                for (int i = 0; i < monsters.Count; i++)
                {
                    if (monsters[i] != null && monsters[i].Type() != "glouto") monsters[i].Draw(spriteBatch);
                }

                glouto.Draw(spriteBatch);
                //   foreach (NPC npc in _npc) if (npc._isTalking) npc.Talk(new Quest(), spriteBatch);
                foreach (NPC npc in _npc)
                {
                    npc.Draw(spriteBatch, Content);
                }
                if(player.IsDie)
                {
                    _camera.LookAt(new Vector2(player.Position.X, player.Position.Y));
                    spriteBatch.Draw(_gameover, new Rectangle((int)player.Position.X - 962, (int)player.Position.Y - 544, WindowWidth, WindowHeight), Color.White);
                    _die.Draw(spriteBatch, Content, new Vector2(_die._position.X, _die._position.Y));
                }

                spriteBatch.End();

                spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
                if (player.IsDie == false)
                {
                    player.Draw(spriteBatch, Content, _camera);
                }
                spriteBatch.End();
                base.Draw(gameTime);
                // TODO: Add your drawing code here
            }
        }
    }
}

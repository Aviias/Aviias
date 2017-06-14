using Microsoft.Xna.Framework;
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
        public static Player player;
        Monster monster;
        Wolf wolf;
        Drake drake;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        // Texture2D texture;
        public static Map map;
        Random random = new Random();
        BoxingViewportAdapter _viewportAdapter;
        const int WindowWidth = 1920;
        const int WindowHeight = 1080;
        Camera2D _camera;
        public List<NPC> _npc;
        SpriteFont font;
        List<Monster> monsters = new List<Monster>();
        StreamWriter log; // Debug file
        Random rnd = new Random();
        Timer spawnTimer = new Timer(10f);
        List<int> list = new List<int>(16);
        Texture2D _gameover;
        //Ressource _testRessource = new Ressource();
        

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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player();
            player.PlayerMoveSpeed = 8.0f;
            Vector2 monsterPosition;
            map = new Map(200, 200);

            int monsterneed = 2 - monsters.Count;
            if (monsterneed != 0)
            {
                for (int i = 0; i < monsterneed; i++)
                {
                    
                    int posX = rnd.Next(0, map.WorldWidth * 10);
                    int posY = rnd.Next(0, map.WorldHeight * 10);
                    monsterPosition = new Vector2(posX, posY);
                    wolf = new Wolf(Content, Content.Load<Texture2D>("loup"), monsterPosition);
                    //monster = new Monster(100, 1.0f, 0.05, 1, 5, Content, Content.Load<Texture2D>("alienmonster"), monsterPosition);
                    
                    monsters.Add(wolf);
                }
            }
               
            Vector2 playerPosition = new Vector2(1500, 345 + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("babyplayer"), playerPosition, Content, map);
            map.GenerateMap(Content);
            map.ActualizeShadow((int)player.Position.X, (int)player.Position.Y);
            IsMouseVisible = true;

            
            _npc = new List<NPC>(8);
            _npc.Add(new NPC(Content, "pnj", spriteBatch, new Vector2(500, 250), 5));
            _npc.Add(new NPC(Content, "pnj", spriteBatch, new Vector2(1400, 300), 3));

            base.Initialize();

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.ApplyChanges();

            _camera.LookAt(new Vector2(player.Position.X + 10, player.Position.Y + 15));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, WindowWidth, WindowHeight);
            _camera = new Camera2D(_viewportAdapter);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");

            _gameover = Content.Load<Texture2D>("gameover3");
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
            if (player.IsDie == false)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);
                // TODO: Add your update logic here

                previousKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();

                player.Update(map);
                player.UpdatePlayerCollision(gameTime, player, monsters);
                Camera.Position = new Vector2(player.Position.X - WindowWidth / 2, player.Position.Y - WindowHeight / 2);

                for (int i = 0; i < monsters.Count; i++)
                {
                    if (monsters[i].IsDie == false && monsters[i] != null)
                    {
                        monsters[i].Update(player, gameTime);
                    }

                    list = player.GetCollisionSide(player.GetBlocsAround(map));

                    Camera.Move(new Vector2(-player.PlayerMoveSpeed, 0));
                }

                player.Update(player, Camera, _npc, gameTime, Content, log, map, monsters);
         //       player.UpdatePlayerCollision(gameTime, player, monsters);
                base.Update(gameTime);


                spawnTimer.Decrem(gameTime);


                if (spawnTimer.IsDown())

                {
                    int posX = rnd.Next(0, map.WorldWidth * 10);
                    int posY = rnd.Next(0, map.WorldHeight * 10);
                    Vector2 monsterPosition = new Vector2(posX, posY);
                    drake = new Drake(Content, Content.Load<Texture2D>("drake"), monsterPosition);

         //       monsters.Add(monster);
               /// monsters.Add(monster);
                  //  monsters.Add(drake);
                   // spawnTimer.ReInit();
                }
             //       spawnTimer.ReInit();
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
            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].Draw(spriteBatch);
            }
            
            
                //   foreach (NPC npc in _npc) if (npc._isTalking) npc.Talk(new Quest(), spriteBatch);
                foreach (NPC npc in _npc)
                {
                    npc.Draw(spriteBatch);
                    npc.Update();
                }

            if (player.IsDie == false)
            {
                player.Draw(spriteBatch, Content);
            }
            else
            {
                _camera.LookAt(new Vector2(player.Position.X, player.Position.Y));
                spriteBatch.Draw(_gameover, new Rectangle((int)player.Position.X - 962, (int)player.Position.Y - 544, WindowWidth, WindowHeight), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
            
        }
    }
}

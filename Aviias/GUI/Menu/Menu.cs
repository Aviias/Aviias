using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    class Menu
    {
        Button _jouer;
        Button _quitter;
        MenuPlay _menuPlay;
        public bool _close { get; set; }
        bool Swap;
        public void Initialize()
        {
            _menuPlay = new MenuPlay();
            _jouer = new Button(new Vector2(305, 655),600,100);
            _quitter = new Button(new Vector2(684 , 800), 600, 100);
        }

        internal void Update(GameTime gameTime, ContentManager Content)
        {
            MouseState mouseState = Mouse.GetState();
            if (_jouer.IsPressed(mouseState) && mouseState.Position.X >= _jouer._position.X && mouseState.Position.Y >= _jouer._position.Y && mouseState.Position.X <= _jouer._position.X + _jouer._width && mouseState.Position.Y <= _jouer._position.Y + _jouer._height)
            {
                _jouer._texture = ".\\Menu\\Button\\jouer_rouge";
                Swap = true;
            }
            else
            {
                _jouer._texture = ".\\Menu\\Button\\jouer_gris";
            }
            if(Swap == true)
            {
                _menuPlay.Update(gameTime, Content);
            }
            else
            {
                if (_quitter.IsPressed(mouseState) && mouseState.Position.X >= _quitter._position.X && mouseState.Position.Y >= _quitter._position.Y && mouseState.Position.X <= _quitter._position.X + _quitter._width && mouseState.Position.Y <= _quitter._position.Y + _quitter._height)
                {
                    _quitter._texture = ".\\Menu\\Button\\quitter_rouge";
                    _close = true;
                }
                else
                {
                    _quitter._texture = ".\\Menu\\Button\\quitter_gris";
                }
            }
        }

        public bool Jouer()
        {
            return _menuPlay.IsTrue;
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            if(Swap == true)
            {
                _menuPlay.Draw(spriteBatch, content);
            }else
            {
                spriteBatch.Draw(content.Load<Texture2D>(".\\Menu\\Background\\menuv2"), new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f,
                    SpriteEffects.None, 0f);
                _jouer.Draw(spriteBatch, content, new Vector2(_jouer._position.X, _jouer._position.Y + 20));
                _quitter.Draw(spriteBatch, content, new Vector2(_quitter._position.X, _quitter._position.Y + 20));
            }
        }

    }
}

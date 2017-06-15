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
    class MenuPlay
    {
        Button _new;
        Button _load;
        

        public MenuPlay()
        {
            _new = new Button(new Vector2(305, 655), 600, 100);
            _load = new Button(new Vector2(684, 800), 600, 100);
            _new.IsTrue = false;
        }

        internal void Update(GameTime gameTime, ContentManager Content)
        {
            MouseState mouseState = Mouse.GetState();
            if (_new.IsPressed(mouseState) && mouseState.Position.X >= _new._position.X && mouseState.Position.Y >= _new._position.Y && mouseState.Position.X <= _new._position.X + _new._width && mouseState.Position.Y <= _new._position.Y + _new._height)
            {
                _new._texture = ".\\Menu\\Button\\nouvelle_rouge";
                _new.IsTrue = true;
            }
            else
            {
                _new._texture = ".\\Menu\\Button\\nouvelle_gris";
                _new.IsTrue = false;

            }
        }

        public bool IsTrue
        {
           get { return _new.IsTrue; }
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            spriteBatch.Draw(content.Load<Texture2D>(".\\Menu\\Background\\menu_partie"), new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
            _new.Draw(spriteBatch, content, new Vector2(_new._position.X, _new._position.Y + 20));
        }

    }
}

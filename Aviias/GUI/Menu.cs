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

        public void Initialize()
        {
            _jouer = new Button(new Vector2(0,0),600,100);
        }

        internal void Update(GameTime gameTime, ContentManager Content)
        {
            _jouer.Update(gameTime, Content);
        }

        public bool Jouer()
        {
            return false;
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {

            spriteBatch.Draw(content.Load<Texture2D>(".\\Menu\\Background\\menuv2"), new Vector2(0,0), null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
            _jouer.Draw(spriteBatch, content);
        }

    }
}

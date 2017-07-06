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
    class MenuCommande
    {
        Button _back;
        bool IsTrue = false;

        public MenuCommande()
        {
            _back = new Button(new Vector2(21, -1), 100, 50, ".\\Menu\\Button\\retour_gris", ".\\Menu\\Button\\retour_gris");
        }

        internal void Update(GameTime gameTime, ContentManager Content)
        {
            MouseState mouseState = Mouse.GetState();
            if (IsTrue) return;
            IsTrue = false;
            if (_back.IsPressed(mouseState) && mouseState.Position.X >= _back._position.X && mouseState.Position.Y >= _back._position.Y && mouseState.Position.X <= _back._position.X + _back._width && mouseState.Position.Y <= _back._position.Y + _back._height)
            {
                Menu.SwapCommande = false;
                IsTrue = false;
            }
        }

        public bool IsTrueF
        {
            get { return IsTrue; }
            set { IsTrue = value; }
        }

        internal void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            spriteBatch.Draw(content.Load<Texture2D>(".\\Menu\\Background\\menu_commandes"), new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f,
               SpriteEffects.None, 0f);
            _back.Draw(spriteBatch, content, new Vector2(_back._position.X, _back._position.Y + 20));
        }

    }
}


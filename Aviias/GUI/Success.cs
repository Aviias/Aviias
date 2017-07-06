using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Aviias
{
    [Serializable]
    internal class Success
    {
        public Text _text;
        public int _jump { get; set; }
        public int _monster1 { get; set; }
        public int _monster2 { get; set; }
        public int _breakblock1 { get; set; }
        public int _breakblock2 { get; set; }
        public int _pickaxeIron { get; set; }
        public int _pickaxeDiamond { get; set; }
        public int _quest { get; set; }
        public int _goldenApple { get; set; }
        public int _craft { get; set; }


        public Success()
        {
        }

        

        internal void Draw(SpriteBatch spriteBatch, ContentManager content, Camera2D camera)
        {
            _text = new Text(content);
            spriteBatch.Draw(content.Load<Texture2D>("Success"), new Vector2(camera.Position.X + 576, camera.Position.Y + 140), null, Color.White, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, 0f);
            //Ligne Bas 5 
            _text.DisplayText(("" + _jump + "/" + "1000"), new Vector2(camera.Position.X + 1245, camera.Position.Y + 845), spriteBatch, Color.White, 1.5f);
            _text.DisplayText(("" + _craft + "/" + "20"), new Vector2(camera.Position.X + 950, camera.Position.Y + 845), spriteBatch, Color.White, 1.5f);
            //Ligne 4
            _text.DisplayText(("" + _goldenApple + "/" + "100"), new Vector2(camera.Position.X + 1245, camera.Position.Y + 730), spriteBatch, Color.White, 1.5f);
            _text.DisplayText(("" + _quest + "/" + "5"), new Vector2(camera.Position.X + 950, camera.Position.Y + 730), spriteBatch, Color.White, 1.5f);
            //Ligne 3
            _text.DisplayText(("" + _pickaxeDiamond + "/" + "1"), new Vector2(camera.Position.X + 1245, camera.Position.Y + 620), spriteBatch, Color.White, 1.5f);
            _text.DisplayText(("" + _pickaxeIron + "/" + "1"), new Vector2(camera.Position.X + 950, camera.Position.Y + 620), spriteBatch, Color.White, 1.5f);
            //Ligne 2
            _text.DisplayText(("" + _breakblock1 + "/" + "200"), new Vector2(camera.Position.X + 950, camera.Position.Y + 520), spriteBatch, Color.White, 1.5f);
            _text.DisplayText(("" + _breakblock2 + "/" + "1000"), new Vector2(camera.Position.X + 1245, camera.Position.Y + 520), spriteBatch, Color.White, 1.5f);
            //Ligne 1
            _text.DisplayText(("" + _monster1 + "/" + "50"), new Vector2(camera.Position.X + 950, camera.Position.Y + 400), spriteBatch, Color.White, 1.5f);
            _text.DisplayText(("" + _monster2 + "/" + "500"), new Vector2(camera.Position.X + 1245, camera.Position.Y + 400), spriteBatch, Color.White, 1.5f);
        }
    }
}

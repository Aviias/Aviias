using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aviias.GUI
{
    [Serializable]
   public  class Animation
    {
        [field:NonSerialized]
        Texture2D _animation;
        [field: NonSerialized]
        Rectangle _sourceRect;
        string _texture;
        float elapsed;
        float _frameTime;
        int _numOfFrame;
        int _currentFrame;
        int _frameWidth;
        int _frameHeight;
                
        public Animation(ContentManager Content, string assert, float frameSpeed, int numOfFrame, Vector2 pos)
        {
            _frameTime = frameSpeed;
            _numOfFrame = numOfFrame;
            _animation = Content.Load<Texture2D>(assert);
            _frameWidth = (_animation.Width / numOfFrame);
            _frameHeight = _animation.Height;
            _texture = assert;
        }

        public void PlayAnim(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _sourceRect = new Rectangle(_currentFrame * _frameWidth, 0, _frameWidth, _frameHeight);

            if (elapsed >= _frameTime)
            {
                if(_currentFrame >= _numOfFrame - 1)
                {
                     _currentFrame = 0;
                }
                else
                {
                    _currentFrame++;
                }
                elapsed = 0;
            }
        }

        public void Reload(ContentManager content)
        {
            _animation = content.Load<Texture2D>(_texture);
            _sourceRect = new Rectangle(_currentFrame* _frameWidth, 0, _frameWidth, _frameHeight);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, ContentManager content)
        {
            if (_animation == null) Reload(content);
            spriteBatch.Draw(_animation, pos, _sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, int luminosity, ContentManager content)
        {
            if (_animation == null) Reload(content);
            spriteBatch.Draw(_animation, pos, _sourceRect, new Color(luminosity, luminosity, luminosity, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            
        }
    }
}

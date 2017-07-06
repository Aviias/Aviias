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
   public  class Animation
    {
        Texture2D _animation;
        Rectangle _sourceRect;
        Vector2 _pos;

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
            _pos = pos;
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

        public void Draw(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.Draw(_animation, pos, _sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, int luminosity)
        {
            spriteBatch.Draw(_animation, pos, _sourceRect, new Color(luminosity, luminosity, luminosity, 255), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            
        }
    }
}

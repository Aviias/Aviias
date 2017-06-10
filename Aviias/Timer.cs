using Microsoft.Xna.Framework;
using System;


namespace Aviias
{
    public class Timer
    {
        float _timeTo;
        float _timeTOReinit;
         
        public Timer(float time)
        {
            _timeTo = time;
            _timeTOReinit = time;
        }

        public void Decrem(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            _timeTo -= elapsed;
        }

        public bool IsDown()
        {
            return (_timeTo < 1);
        }

        public void ReInit()
        {
            _timeTo = _timeTOReinit;
        }

    }
}

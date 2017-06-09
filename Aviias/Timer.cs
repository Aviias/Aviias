using Microsoft.Xna.Framework;
using System;


namespace Aviias
{
    class Timer
    {
        float _timeTo;
        float _timeTO;
         
        public Timer(float time)
        {
            _timeTo = time;
            _timeTO = time;
        }

        public bool IsDown(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            _timeTo -= elapsed;
            return _timeTo < 1;
        }

        public void ReInit()
        {
            _timeTo = _timeTO;
        }

    }
}

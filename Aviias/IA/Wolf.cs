using Aviias.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviias
{
    class Wolf : Monster
    {
        public Wolf(ContentManager content, Texture2D texture, Vector2 pos, ushort[] proba)
            : base(100, 1.5f, 0.10, 10, 5, content, texture, pos, 150, proba)
        {
            _pos = pos;
        }

        override public string Type()
        {
            return "wolf";
        }

    }
}

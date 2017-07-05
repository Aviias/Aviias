using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aviias
{
    class Drake : Monster
    {
        public Drake(ContentManager content, Texture2D texture, Vector2 pos)
            : base(300, 1.6f, 0.10, 0, 2, content, texture, pos, 100)
        {

        }

        override public string Type()
        {
            return "drake";
        }

    }
}

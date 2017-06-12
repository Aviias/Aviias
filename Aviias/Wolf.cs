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
           
        public Wolf(ContentManager content,Texture2D texture, Vector2 pos)
            : base(100, 1.5f, 0.10, 150, 5, content, texture, pos)
        { 

        }

        
} 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aviias.IA
{
     class Gloutogobe : Monster
    {
        int _stepEvolve;

        public Gloutogobe(ContentManager content, Texture2D texture, Vector2 pos)
            : base(100, 1.5f, 0.10, 0, 5, content, texture, pos)
        {
            _stepEvolve = 1;
        }

        public bool GobMonster(List<Monster> monsters,int i)
        {
            if(this.MonsterPosition.X <= monsters[i].MonsterPosition.X - 15 && this.MonsterPosition.X >= monsters[i].MonsterPosition.X + monsters[i].Width + 15 && this.MonsterPosition.Y <= monsters[i].MonsterPosition.Y - 15 && this.MonsterPosition.Y <= monsters[i].MonsterPosition.Y + monsters[i].Height + 15 && _stepEvolve <= 3)
            {
                monsters.Remove(monsters[i]);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GloutoEvolve(ContentManager content, string texture)
        {
            if(_stepEvolve <= 3)
            {
                this.Health += 200;
                this.Resistance += 10;
                this.Damage += 15;
                this.moveSpeed += 0.2f;
                this.Texture = content.Load<Texture2D>(texture);
                _stepEvolve += 1;
            }
            
        }

        public int ClosestMonster(Vector2 pos, List<Monster> monsters)
        {
            Monster closest = monsters[0];
            int clos = 0;
            int i = 1;

            while (i < monsters.Count)
            {
                if(Vector2.Distance(pos, monsters[i].MonsterPosition) <= Vector2.Distance(pos, closest.MonsterPosition))
                {
                    closest = monsters[i];
                    clos = i;
                }
                i++;
            }
            return clos;
        }

        public void MoveOnClosestMonster(Vector2 pos)
        {
            if(_stepEvolve <=3)
            {
                float alpha = (float)Math.Atan2((pos.Y - this.MonsterPosition.Y), (pos.X - this.MonsterPosition.X));
                Vector2 direction = AngleToVector(alpha);
                Vector2 move = new Vector2(direction.X * this.moveSpeed, /*direction.Y * _speed*/0);
                this.MonsterPosition = new Vector2(this.MonsterPosition.X + move.X, this.MonsterPosition.Y /*+ move.Y*/);
            }
            
        }

        Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}

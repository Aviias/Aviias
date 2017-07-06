﻿using System;
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
        Timer GobMonsterTimer = new Timer(15f);
        
        public Gloutogobe(ContentManager content, Texture2D texture, Vector2 pos, ushort[] proba)
            : base(100, 1.5f, 0.10, 20, 5, content, texture, pos, 100,proba ,"glouto")
        {
            _stepEvolve = 1;
        }

        public int StepEvolve
        {
            get { return _stepEvolve; }
            set { _stepEvolve = value; }
        }

        public bool GobMonster(List<Monster> monsters,int i, GameTime gametime)
        {
            GobMonsterTimer.Decrem(gametime);
            if( Vector2.Distance(this.MonsterPosition, monsters[i].MonsterPosition) <= 400 && _stepEvolve <= 3 && GobMonsterTimer.IsDown())
            {
                monsters.Remove(monsters[i]);
                GobMonsterTimer.ReInit();
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
                _points[3] += 10;
            }
            
        }

        public string GetTexture(int x)
        {
            if(x == 2)
            {
                return "boeuface";
            }
            else if (x == 3)
            {
                return "larveface";
            }
            else
            {
                return "wolfface";
            }
        }

        public bool ClosestPlayer(Player player)
        {
            return Vector2.Distance(this.MonsterPosition, player.PlayerPosition) <= 500;
        }

        public int ClosestMonster(Vector2 pos, List<Monster> monsters)
        {
            Monster closest = monsters[0];
            int clos = 0;
            int i = 1;

            while (i < monsters.Count)
            {
                if(monsters[i] != null && Vector2.Distance(pos, monsters[i].MonsterPosition) <= Vector2.Distance(pos, closest.MonsterPosition))
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
                Vector2 move = new Vector2(direction.X * this.moveSpeed, direction.Y * this.moveSpeed);
                this.MonsterPosition = new Vector2(this.MonsterPosition.X + move.X, this.MonsterPosition.Y + move.Y);
            }
            
        }

        override public string Type()
        {
            return "glouto";
        }

        Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        public void Update(List<Monster> monsters, Player player, ContentManager content, GameTime gametime, Map map)
        {
            GobMonsterTimer.Decrem(gametime);
            if(_stepEvolve <= 3 && ClosestPlayer(player) == false)
            {
                int i = ClosestMonster(this.MonsterPosition, monsters);
                MoveOnClosestMonster(monsters[i].MonsterPosition);
                if (GobMonsterTimer.IsDown() && GobMonster(monsters,i,gametime))
                {
                    GloutoEvolve(content, GetTexture(_stepEvolve - 1));
                    GobMonsterTimer.ReInit();
                }
            }
            else
            {
                base.Update(player, gametime, map);
            }
        }
        
    }
    
}

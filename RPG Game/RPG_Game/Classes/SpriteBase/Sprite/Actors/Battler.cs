using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RPG_Game
{
    public class Battler : Sprite
    {
        public string name;

        public int maxHealth;
        public int health;

        public float PhAtk;
        public float MgAtk;

        public float PhDef;
        public float MgDef;

        public float speed;

        public float Acc;
        public float Eva;

        public float meter;
        public float pastMeter;

        public Sprite meterSprite = new Sprite();

        public Sprite shadow = new Sprite();

        public List<Ability> abilities = new List<Ability>();

        public Vector2 battleOrigin;
        public bool friendly;
    }
}

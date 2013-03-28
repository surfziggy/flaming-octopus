using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelLibrary
{
    public class GameObject
    {
        public Vector2 position;                                // Player coords relative to top left screen
        public virtual int Height { get; set; }  
        public virtual int Width { get; set; }
        public virtual int Health { get; set; }
        public virtual int Lives { get; set; }
        public virtual int Rotation { get; set; }

        public GameObject()
        {
            Health = 100;
            Lives = 3;
        }

        public virtual void Hit(int hitPower)
        {
            Health -= hitPower;
            if (Health < 0)
            {
                Lives--;
                Health = 100;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle GetBounds()
        {
            return (new Rectangle((int)position.X, (int)position.Y, Width, Height));
        }
    }
}

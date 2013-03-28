using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelLibrary
{
    public class GameObject
    {
        #region Properties
        public virtual int  Height { get; set; }  
        public virtual int  Width { get; set; }
        public virtual int  Health { get; set; }
        public virtual int  Lives { get; set; }
        public virtual int  Rotation { get; set; }
        public virtual bool Alive { get; set; }
        // ordinary virtual property with backing field 
        public Vector2 position;
        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        

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

                Lives = (int)Math.Floor((float)Lives);
                // We are dead for now
                Alive = false;
            }
        }

        public Rectangle GetBounds()
        {
            return (new Rectangle((int)position.X, (int)position.Y, Width, Height));
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPlat
{
    public class GameObject
    {
        public Vector2 position;                                // Player coords relative to top left screen
        float rotation = 0f;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }

        }
    }
}

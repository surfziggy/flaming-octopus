using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LevelLibrary
{
    public class Gravity
    {

        float vi = 0; // vi - initial velocity 
        float gravityStrength = 0.2f;
        public int objectHeight = 0;
        public int windowHeight = 0;
        public LevelLibrary.Directions direction;
        public void SetVelocity(float v, LevelLibrary.Directions dir)  
        {
            vi = v;
            direction = dir;
        }

        // Apply the rules of gravity
        public void Apply(ref Vector2 spritepos, GameTime gameTime, bool isOnGround)
        {   
            // Is the player jumping up
            if (direction == LevelLibrary.Directions.up)
            {
                // Apply the velocity
                spritepos.Y += vi;
                // Erode the velocity (it's negative at the mo')   
                vi += gravityStrength;
                // Once we reach zero(ish) velocity we will start to come back down
                if (vi >= 0f)
                {
                    // Now headed down
                    direction = LevelLibrary.Directions.down;
                    vi = 1;
                }
            }
            else if (direction == LevelLibrary.Directions.down) 
            {
                // Apply the velocity
                spritepos.Y += vi;
                // This time increase velocity as speed increases over time
                vi += gravityStrength;
            }
            // If we are not moving 
            else if (direction == LevelLibrary.Directions.none && isOnGround == false)
            {
                // Apply the velocity
                spritepos.Y += vi;
                // If so we will start to fall
                direction = LevelLibrary.Directions.down;
                // Increase at rate of fall
                vi = gravityStrength;
            } 
        }
    }
}

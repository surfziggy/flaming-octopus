//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// Class      : PlayerAnimation.cs
// Description: Logic to animate the player
//                  - stores the texture atlas
//                  - updates the frame
//                  - draws it
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelLibrary
{
    public enum Directions
    {
        none, up, down, left, right
    }
    public class SpriteAnimator
    {
        // The image representing the collection of images used for animation
        Texture2D spriteStripLeft;
        Texture2D spriteStripRight;

        // The scale used to display the sprite strip
        float scale;

        // The time we display a frame until the next one
        public int frameTime;
        int elapsedTime;    

        // The number of frames that the animation contains
        int frameCount;

        // The index of the current frame we are displaying
        int currentFrame;

        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();

        // Width of a given frame
        public int FrameWidth;

        // Height of a given frame
        public int FrameHeight;

        // The state of the Animation
        public bool Active;
        public bool Animating;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Width of a given frame
        public Vector2 Position;
        public Directions direction;

        public void Initialize(Texture2D textureLeft, Texture2D textureRight, Vector2 position,
        int frameWidth, int frameHeight, int frameCount,
        int frametime, Color color, float scale, bool looping)
        {
            // Keep a local copy of the values passed in
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStripLeft = textureLeft; 
            spriteStripRight = textureRight;
            direction = Directions.right;

            currentFrame = 0;

            // Set the Animation to active by default
            Active = true;
            Animating = true;
        }
        public void NextFrame()
        {
            Console.Write("Current frame %d", currentFrame);
            Console.WriteLine(); 
            currentFrame++;
            if (currentFrame == frameCount)
            {
                currentFrame = 0;
                // If we are not looping deactivate the animation
                if (Looping == false)
                    Active = false;
            }
        }
        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Animating == true)
            {
                // Update the elapsed time
                elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                // If the elapsed time is larger than the frame time
                // we need to switch frames
                if (elapsedTime > frameTime)
                {
                    // Move to the next frame
                    currentFrame++;

                    // If the currentFrame is equal to frameCount reset currentFrame to zero
                    if (currentFrame == frameCount)
                    {
                        currentFrame = 0;
                    }
                    // Reset the elapsed time to zero
                    elapsedTime = 0;
                }
            }
            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            //// Build a destination rectangle X centered, but aligned Y coord is the bottom
            //// centered Y would be (frameheight * scale)/2
            //destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale),
            //(int)Position.Y - (int)(FrameHeight * scale),
            //(int)(FrameWidth * scale),
            //(int)(FrameHeight * scale));
            destinationRect = new Rectangle((int)Position.X,
            (int)Position.Y,
            (int)(FrameWidth * scale),
            (int)(FrameHeight * scale));
        }
        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                if (direction == Directions.left)
                {
                    spriteBatch.Draw(spriteStripLeft, destinationRect, sourceRect, color);
                }
                else
                {
                    spriteBatch.Draw(spriteStripRight, destinationRect, sourceRect, color);
                }
            }
        }
    }
}

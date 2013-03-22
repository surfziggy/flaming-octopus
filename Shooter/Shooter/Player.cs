//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// Class      : player.cs
// Description: Logic for the player to:
//                  - Update position
//                  - Draw the player
//                  - load textures
//                  - check for collisions with platforms
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyPlat
{
    public class Player : GameObject
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public LevelLibrary.SpriteAnimator PlayerAnimation;     // Animation representing the player
        public LevelLibrary.Directions direction;               // Players direction
        public float currentSpeed;                              // Player speed now
        public bool active;                                     // State of the player
        public int health;                                      // Amount of hit points that player has
        public bool hoverAbility;                               // Can I hover
        public int jumpPower = -3;                              // Jump power
        public float maxSpeed = 3f;                             // How fast can I run?
        public float initialSpeed = 1f;                         // Run start speed
        bool jumping = false;                                   // Is character currently jumping?
        LevelLibrary.Gravity gravity;                           // Gravity logic
        int frameTime = 40;                                     // Time to show each frame of animation
        bool isOnGround = false;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Method to return the size of the player sprite (width)
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Method to return the size of the player sprite (height)
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Initialise method
        // animation - texture atlas of all the animation frames
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Initialize(LevelLibrary.SpriteAnimator animation, Vector2 startPosition, int xSize, int ySize)
        {
            PlayerAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = startPosition;
            direction = LevelLibrary.Directions.none;
            currentSpeed = 1f;
            gravity = new LevelLibrary.Gravity();
            gravity.windowHeight = ySize;
            gravity.objectHeight = PlayerAnimation.FrameHeight;

            // Set the player to be active
            active = true;

            // Set the player health
            health = 100;

            // Ability to jump
            jumpPower = -7;
            // Can I hover
            hoverAbility = false;

            PlayerAnimation.Active = true;
            PlayerAnimation.Position = Position;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handles ;
        // - collisions with platforms
        // - applying gravity to the player
        // - resetting the player position if needed.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdatePlayerGravityAndPlatforms(GameTime gameTime, LevelLibrary.LevelRenderer levelRenderer)
        {
            bool clashing;

            gravity.Apply(ref position, gameTime, isOnGround);
            
            // Update the level positions and enemies
            levelRenderer.Update(gameTime, ref position, this.Width, this.Height);

            // Check for clashes
            clashing = levelRenderer.HandleClash(ref position, Width, Height, ref isOnGround);

            // If we clash with the square below and we're falling STOP
            if (clashing)
            {
                if (gravity.direction == LevelLibrary.Directions.down)
                {
                    gravity.direction = LevelLibrary.Directions.none;
                }
                else if (gravity.direction == LevelLibrary.Directions.up)
                {
                    gravity.SetVelocity(1, LevelLibrary.Directions.down);
                }
            }

            // If we have landed back on earth, we can jump again.
            if (isOnGround || hoverAbility == true)
            {
                jumping = false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Update the players animation
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Animate()
        {
            PlayerAnimation.frameTime = (frameTime - ((int)currentSpeed * 2));
            // Set animation direction
            PlayerAnimation.direction = direction;
            // Apply current speed and direction
            if ((direction == LevelLibrary.Directions.left) && 
                (currentSpeed > 1))
            {
                position.X -= (int)currentSpeed;
                //PlayerAnimation.NextFrame();
                PlayerAnimation.Animating = true;
            }
            else if ((direction == LevelLibrary.Directions.right) && 
                (currentSpeed > 1))
            {
                position.X += (int)currentSpeed;
                //PlayerAnimation.NextFrame();
                PlayerAnimation.Animating = true;
            }
            else if(currentSpeed < 1)
            {
                PlayerAnimation.Animating = false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Update the position, animation frame etc
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Update(LevelLibrary.LevelRenderer levelRenderer,       
                                GameTime gameTime, 
                                bool jumpKey, bool leftKey, bool rightKey)
        {
            //PlayerAnimation.Active = false;
            HandleKeyPress(jumpKey, leftKey, rightKey);

            Animate();

            UpdatePlayerGravityAndPlatforms(gameTime, levelRenderer);
            // Keep the player within the limits of the level
            //ResetPositionOnScreen();

            PlayerAnimation.Position = position;
            PlayerAnimation.Update(gameTime);
        }

        // Draw everything related to the player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
        
        // Handle player key presses
        void HandleKeyPress(bool jumpKey, bool leftKey, bool rightKey)
        {
            // We're jumping so set our velocity to upwards 
            if (jumpKey == true && !jumping)
            {
                jumping = true;
                gravity.SetVelocity(jumpPower, LevelLibrary.Directions.up);
            }
            // Left key press
            if (leftKey == true)
            {
                // Not moving - start moving
                if (direction == LevelLibrary.Directions.none)
                {
                    currentSpeed = initialSpeed;
                    direction = LevelLibrary.Directions.left;
                }
                else if (direction == LevelLibrary.Directions.left)
                {
                    // Allow increase in speed up until max speed
                    if (currentSpeed < maxSpeed)
                    {
                        currentSpeed++;
                    }
                }
                else
                {
                    currentSpeed--;
                    if (currentSpeed <= 1)
                    {
                        direction = LevelLibrary.Directions.none;
                    }
                }
            }
            // Right key press
            else if (rightKey == true)
            {
                // Not moving - start moving
                if (direction == LevelLibrary.Directions.none)
                {
                    currentSpeed = initialSpeed;
                    direction = LevelLibrary.Directions.right;
                }
                else if (direction == LevelLibrary.Directions.right)
                {
                    // Allow increase in speed up until max speed
                    if (currentSpeed < maxSpeed)
                    {
                        currentSpeed++;
                    }
                }
                else
                {
                    currentSpeed--;
                    if (currentSpeed <= 1)
                    {
                        direction = LevelLibrary.Directions.none;
                    }
                }
            }
            else
            {
                // Player slows down if they don't press a key
                if (currentSpeed > 0)
                {
                    currentSpeed -= 0.1f;
                }
            }
        }
    }
}

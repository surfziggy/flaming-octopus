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
    public class Player : LevelLibrary.GameObject
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #region private members
        LevelLibrary.SpriteAnimator PlayerAnimation;            // Animation representing the player
        LevelLibrary.Directions direction;                      // Players direction
        float currentSpeed;                                     // Player speed now
        bool hoverAbility;                                      // Can I hover
        int jumpPower = -15;                                    // Jump power
        float maxSpeed = 6f;                                    // How fast can I run?
        float initialSpeed = 1f;                                // Run start speed
        bool jumping = false;                                   // Is character currently jumping?
        LevelLibrary.Gravity gravity;                           // Gravity logic
        int frameTime = 40;                                     // Time to show each frame of animation
        bool isOnGround = false;
        #endregion
        #region Properties
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Method to return the size of the player sprite (width)
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Method to return the size of the player sprite (height)
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }
        #endregion
        #region Public Methods
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Initialise method
        // animation - texture atlas of all the animation frames
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Initialize(LevelLibrary.SpriteAnimator animation, Vector2 startPosition, int xSize, int ySize)
        {
            PlayerAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            position = startPosition;
            direction = LevelLibrary.Directions.none;
            currentSpeed = 1f;
            gravity = new LevelLibrary.Gravity();
            gravity.windowHeight = ySize;
            gravity.objectHeight = PlayerAnimation.FrameHeight;

            // Set the player health
            Health = 100;
            Lives = 3;

            // Can I hover
            hoverAbility = false;

            PlayerAnimation.Active = true;
            PlayerAnimation.Position = Position;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Update the position, animation frame etc
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Update(LevelLibrary.LevelRenderer levelRenderer,
                                GameTime gameTime,
                                bool jumpKey, bool leftKey, bool rightKey)
        {
            UpdatePlayerSpeedAndDirection(jumpKey, leftKey, rightKey);

            UpdateAnimationMovementState();

            UpdatePlayerGravityAndPlatforms(gameTime, levelRenderer);

            UpdatePlayerAnimation(gameTime);
        }

        // Draw everything related to the player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
        #endregion
        #region Private Methods
        // Handle player key presses
        private void UpdatePlayerSpeedAndDirection(bool jumpKey, bool leftKey, bool rightKey)
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
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handles ;
        // - Set the animations position
        // - Tell the animation class to animate the sprite
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdatePlayerAnimation(GameTime gameTime)
        {
            PlayerAnimation.Position = position;
            PlayerAnimation.Update(gameTime);
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
            levelRenderer.Update(gameTime, ref position, this.Width, this.Height, this);

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
        private void UpdateAnimationMovementState()
        {
            PlayerAnimation.frameTime = (frameTime - ((int)currentSpeed * 2));
            // Set animation direction

            if (this.Alive == true)
            {
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
                else if (currentSpeed < 1)
                {
                    PlayerAnimation.Animating = false;
                }
            }
            else
            {
                Alive = true;
            }
        }
        #endregion
    }
}

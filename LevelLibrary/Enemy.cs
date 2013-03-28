using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace LevelLibrary
{
    enum enemyMode
    {
        Static = 0,
        LeftRight = 1,
        UpDown = 2
    };

    /// <summary>
    /// Enemy class - one of these is created for each of the bad guys
    /// </summary>
    class Enemy
    {
        private SpriteAnimator  enemyAnimation;         // Animation representing the enemy guy
        private Vector2         position;               // Enemy current position
        private Vector2 oldPosition;                    // Enemy previous position
        private Vector2 direction;                      // the current direction
        private Vector2 screenLimits;                   // Size of the screen
        private bool active;                            // Is the enemy active
        private int Health { get; set; }                // is the enemy alive and if so how much
        public enemyMode EnemyMode { get; set; }


        public void Initialise(SpriteAnimator animation, Vector2 startPosition, 
                                enemyMode mode, Vector2 dir, Vector2 scr)
        {
            enemyAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            position = startPosition;
            oldPosition = startPosition;

            // Set the enemy to be active
            active = true;

            // Set the player health
            Health = 100;

            enemyAnimation.Active = true;
            enemyAnimation.Position = position;
            EnemyMode = mode;
            direction = dir;

            screenLimits = scr;
        }

        public void Update(GameTime gameTime, LevelLibrary.LevelRenderer levelRenderer )
        {
            // Store our old position in case we need to reset 
            // because we're clashing with something
            oldPosition = position;
            
            // Different guys have different movement patterns 
            switch (EnemyMode)
            {
                case enemyMode.LeftRight:
                    ModeLeftRight(levelRenderer);
                    break;
                case enemyMode.UpDown:
                    ModeLeftRight(levelRenderer);
                    break;
            }

            // Update the sprite animators position
            enemyAnimation.Position = position;
            enemyAnimation.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (active)
            {
                enemyAnimation.Draw(spriteBatch);
            }
        }

        public Rectangle GetBounds()
        {
            return(new Rectangle((int)position.X,
                                 (int)position.Y, 
                                 enemyAnimation.FrameWidth, 
                                 enemyAnimation.FrameHeight));
        }

        private void ModeLeftRight(LevelLibrary.LevelRenderer levelRenderer)
        {
            //LevelLibrary.LevelRenderer.Sides clashingWith;
            bool clash = false;
            bool isOnGround = false;

            position += direction;

            clash = levelRenderer.HandleClash(ref position,
                                        enemyAnimation.FrameWidth, enemyAnimation.FrameHeight, ref isOnGround);

            // Clash on left ?
            // Clash on right ?
            if ((clash) || (position.X <= 0) || (position.X >= screenLimits.X))
            {
                //position = oldPosition;
                // Reverse the direction by multiplying by -1
                // e.g. 1 * -1 = -1
                // and -1 * -1 =  1
                // Nice huh?
                direction.X = direction.X * -1.0f;
                if (enemyAnimation.direction == Directions.right)
                {
                    enemyAnimation.direction = Directions.left;
                }
                else
                {
                    enemyAnimation.direction = Directions.right;
                }
            }
        }
        private void ModeUpDown(LevelLibrary.LevelRenderer levelRenderer)
        {
            //LevelLibrary.LevelRenderer.Sides clashingWith;
            bool clash = false;
            bool isOnGround = false;

            position += direction;

            clash = levelRenderer.HandleClash(ref position, 
                                        enemyAnimation.FrameWidth, enemyAnimation.FrameHeight, ref isOnGround);

            // Clash on left ?
            // Clash on right ?
            if ((clash) ||  (position.Y <= 0) || (position.Y >= screenLimits.Y))
            {                
                //position = oldPosition;
                // Reverse the direction by multiplying by -1
                // e.g. 1 * -1 = -1
                // and -1 * -1 =  1
                // Nice huh?
                direction.Y = direction.Y * -1.0f;
                if (enemyAnimation.direction == Directions.up)
                {
                    enemyAnimation.direction = Directions.down;
                }
                else
                {
                    enemyAnimation.direction = Directions.up;
                }
            }
        }
    }
}

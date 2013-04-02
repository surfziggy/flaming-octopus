using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace LevelLibrary
{
    enum enemyMode
    {
        Static = 0,
        LeftRight = 1,
        UpDown = 2,
        Chase = 3,
        Random = 4
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
        private float duration;                         // Time duration of the current movement 
        private Vector2 screenLimits;                   // Size of the screen
        private Vector2 playerPosition;                 // Players position
        private bool active;                            // Is the enemy active
        private int Health { get; set; }                // is the enemy alive and if so how much
        private Timer timer;                            // Timer to control movement direction
        private Random random;                          // Random number generator
        public enemyMode EnemyMode { get; set; }        // Enemy movement type property

        public void UpdatePlayerLocation(Vector2 pos)
        {
            playerPosition = pos;
        }

        public void Initialise(SpriteAnimator animation, Vector2 startPosition, 
                                enemyMode mode, Vector2 dir, Vector2 scr)
        {
            enemyAnimation = animation;

            position = startPosition;
            oldPosition = startPosition;

            // Set the enemy to be active
            active = true;

            // Set the player health
            Health = 100;

            enemyAnimation.Active = true;
            enemyAnimation.Position = position;
            EnemyMode = mode;
            
            direction = Vector2.Zero;
            switch(mode)
            {
                case enemyMode.UpDown:
                    direction.Y = 1f;
                    break;
                case enemyMode.LeftRight:
                    direction.X = 1f;
                    break;
                case enemyMode.Chase:
                    direction.X = 1f;
                    break;
                case enemyMode.Random:
                    timer = new Timer();
                    AssignNewDirection();
                    break;
            }

            screenLimits = scr;
        }

        public void Update(GameTime gameTime, LevelLibrary.LevelRenderer levelRenderer )
        {
            // Store our old position in case we need to reset 
            // because we're clashing with something
            oldPosition = position;
            
            // Different guys have different movement patterns 
            //direction.X = 1f;
            switch (EnemyMode)
            {
                case enemyMode.LeftRight:
                    ModeLeftRight(levelRenderer);
                    break;
                case enemyMode.UpDown:
                    ModeLeftRight(levelRenderer);
                    break;
                case enemyMode.Chase:
                    ModeChase(levelRenderer);
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
        private void ModeChase(LevelLibrary.LevelRenderer levelRenderer)
        {
            bool clash = false;
            bool isOnGround = false;

            if (playerPosition.X < position.X)
            {
                direction.X = -1f;
            }
            else
            {
                direction.X = 1f;
            }

            position += direction;

            clash = levelRenderer.HandleClash(ref position,
                                        enemyAnimation.FrameWidth, enemyAnimation.FrameHeight, ref isOnGround);

            // Clash on left ?
            // Clash on right ?
            if ((clash) || (position.X <= 0) || (position.X >= screenLimits.X))
            {
                direction.Y = 0f;
            }
        }
        private void AssignNewDirection()
        {
            random = new Random();
            direction.X = (float)random.Next(-5, 5);
            direction.Y = (float)random.Next(-5, 5);
            duration = (float)random.Next(1000,4000);
            timer.Elapsed += new ElapsedEventHandler(TimerExpired);
            timer.Interval = duration;
            timer.Enabled = true;
        }
        // Specify what you want to happen when the Elapsed event is raised.
        private void TimerExpired(object source, ElapsedEventArgs e)
        {
            AssignNewDirection();
        }
    }
}

//#define MODULE_DEBUG
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelLibrary
{
    public class LevelRenderer
    {
        public enum Sides
        {
            none   = 0, 
            top    = 1, 
            right  = 2,
            bottom = 4, 
            left   = 8
        }
        
        Level levelData;                            // The grid of data representing the level
        Texture2D spriteStrip;                      // The image representing the collection of images used for level backdrop
        float scale;                                // The scale used to display the sprite strip
        int tileCount;                              // The number of frames that the backdrop texture contains
        Color color;                                // The color of the frame we will be displaying
        Rectangle sourceRect = new Rectangle();     // The area of the image strip we want to display
        Rectangle destinationRect = new Rectangle();// The area where we want to display the image strip in the game
        public int tileWidth;                       // Width of a given grid square
        public int tileHeight;                      // Height of a given grid square
        public bool active;                         // The state of the level render
        public Vector2 position;                    // Position of the level on the screen
        Enemy[] enemies;
        private SpriteAnimator []enemyAnimation;
        private Texture2D enemyTexture1;
        private Texture2D enemyTexture2;
        private Vector2 screenLimits;
        private float previousBottom;

        // Method to initiali(s)e a level renderer, with level data including graphics, position etc.
        public void Initialize(Level level, 
                                Texture2D texture, 
                                int width, 
                                int height, 
                                int gridCount,
                                Color color, 
                                float scale)
        {
            // Keep a local copy of the values passed in
            levelData = level;
            this.color = color;
            this.tileWidth = width;
            this.tileHeight = height;
            this.tileCount = gridCount;
            this.scale = scale;
            spriteStrip = texture;
            // Set the backdrops to active by default
            active = true;
            position.X = 0;
            position.Y = 0;
            screenLimits.X = levelData.Columns * tileWidth;
            screenLimits.Y = levelData.Rows * tileHeight;


            buildAnimations();
            buildEnemies();
        }

        // Update the level
        public void Update(GameTime gameTime, ref Vector2 position, int Width, int Height, GameObject player)
        {
            UpdateEnemies(player.position, gameTime);

        }
        // Draw the level to the background
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int row = 0;
            int column = 0;
            int square = 0;

            for (row = 0; row < levelData.Rows; row++)
            {
                for (column = 0; column < levelData.Columns; column++)
                {
                    square = levelData.GetValue(row, column);

                    // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
                    sourceRect = new Rectangle(square * tileWidth, 0, tileWidth, tileHeight);
                    // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
                    destinationRect = new Rectangle((int)position.X +  (int)((tileWidth * scale) * column),
                    (int)position.Y + (int)((tileHeight * scale) * row),
                    (int)(tileWidth * scale),
                    (int)(tileHeight * scale));
                    spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
                }
            }

            // Now draw all the bad guys
            DrawEnemies(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager Content)
        {
            enemyTexture1 = Content.Load<Texture2D>("Enemies\\alienl");
            enemyTexture2 = Content.Load<Texture2D>("Enemies\\alienr");
        }

        public bool HasPlayerReachedExit(Vector2 position)
        {
            // Work out where the center of the guy is
            int column = (int)(position.X / tileWidth);
            int row = (int)(position.Y / tileHeight);

            return (levelData.isExit(row, column));
        }

        private void ResolveCollision(Vector2 depth, ref Vector2 position)
        {
            float absDepthX = Math.Abs(depth.X);
            float absDepthY = Math.Abs(depth.Y);

            if (absDepthY < absDepthX)
            {
                position = new Vector2(position.X, position.Y + depth.Y);
            }
            else
            {
                position = new Vector2(position.X + depth.X, position.Y);
            }
        }

        public Rectangle BoundingRectangle(Vector2 position, int Width, int Height)
        {
            // Bounding rectangle of the guy
            return(new Rectangle((int)position.X, (int)position.Y, Width, Height));
        }

        public bool HandleClash(ref Vector2 position, int Width, int Height, ref bool isOnGround)
        {
            bool clash = false;
            bool tileBelow = false;
            Rectangle playerRectangle;
            int row = 0;
            int column = 0;
            

            playerRectangle = BoundingRectangle(position, Width, Height);

            // Work out where the center of the guy is
            column = (int)(position.X / tileWidth);
            row = (int)(position.Y / tileHeight);

            int leftTile = (int)Math.Floor((float)playerRectangle.Left / Width);
            int rightTile = (int)Math.Ceiling(((float)playerRectangle.Right / Width)) - 1;
            int topTile = (int)Math.Floor((float)playerRectangle.Top / Height);
            int bottomTile = (int)Math.Ceiling(((float)playerRectangle.Bottom / Height)) - 1;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    if(levelData.IsSupporting(y, x))
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = levelData.GetRectangle(y,x);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerRectangle, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis, but Y takes preference
                            if (absDepthY <= absDepthX)
                            {
                                // Check if we are on the ground
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;
                                 
                                // Resolve the collision along the Y axis.
                                position = new Vector2((float)Math.Round(position.X, 0), (float)Math.Round((position.Y + depth.Y), 0));
                                //position = new Vector2(position.X, (position.Y + depth.Y));

                                // Perform further collisions with the new bounds.
                                playerRectangle = BoundingRectangle(position, Width, Height);
                                clash = true;
                            }
                            else 
                            {
                                // Resolve the collision along the X axis.
                                position = new Vector2(Math.Abs(position.X + depth.X), Math.Abs(position.Y));

                                // Perform further collisions with the new bounds.
                                playerRectangle = BoundingRectangle(position, Width, Height);
                                clash = true;
                            }
                        }
                    }
                }
            }

            // Check to see if we are in thin air.
            if (clash == false)
            {
                // Will need to check one tile under.
                playerRectangle = BoundingRectangle(position, Width, Height+1);

                int y = bottomTile;
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    if (levelData.IsSupporting(y, x))
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = levelData.GetRectangle(y, x);
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerRectangle, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            tileBelow = true;
                            break;
                        }
                    }
                }
                if (tileBelow != true)
                {
                    isOnGround = false;
                }
            }
            // Save the new bounds bottom.
            previousBottom = playerRectangle.Bottom;

            return (clash);
        }
        
        public bool HandleEnemyClash(GameObject player)
        {
            bool clash = false;
            foreach(Enemy enemy in enemies)
            {
                if(enemy.GetBounds().Intersects(player.GetBounds()))
                {
                    player.Hit(1);
                    clash = true;
                }
            }
            return (clash);
        }

        public int FindPlatformBelow(int row, int column)
        {
            int nextRow = row+1;
            while(nextRow < levelData.Rows)
            {
                if (levelData.IsSupporting(nextRow, column))
                {
                    break;
                }
                nextRow++;
            }
            return (nextRow);
        }

        public void DumpLevel()
        {
            for (int row = 0; row < levelData.Rows; row++)
            {
                for (int column = 0; column < levelData.Columns; column++)
                {
                    Console.Write(levelData.GetValue(row, column));
                }
                Console.WriteLine();
            }
        }

        #region private

        private void UpdateEnemies(Vector2 playerPosition, GameTime gameTime)
        {
            for (int i = 0; i < levelData.numberAliens; i++)
            {
                enemies[i].UpdatePlayerLocation(playerPosition);
                enemies[i].Update(gameTime, this);
            }
        }

        private void DrawEnemies(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < levelData.numberAliens; i++)
            {
                enemies[i].Draw(gameTime, spriteBatch);
            }
        }
        

        private void buildEnemies()
        {
            Vector2 startPosition;
            Vector2 direction;
            

            enemies = new Enemy[levelData.numberAliens];
            for (int i = 0; i < levelData.numberAliens; i++)
            {
                direction.X = 4.0f;
                direction.Y = 0.0f;
                startPosition.X = (float)(levelData.EnemyPositions[i, 0] * this.tileWidth);
                startPosition.Y = (float)(levelData.EnemyPositions[i, 1] * this.tileHeight);
                enemies[i] = new Enemy();
                enemies[i].Initialise(enemyAnimation[i], startPosition, (LevelLibrary.enemyMode)levelData.EnemyTypes[i], direction, screenLimits);
            }
        }

        private void buildAnimations()
        {
            enemyAnimation = new SpriteAnimator[levelData.numberAliens];
            for (int i = 0; i < levelData.numberAliens; i++)
            {
                SpriteAnimator enemySpriteAnimation = new SpriteAnimator();
                enemyAnimation[i] = enemySpriteAnimation;
                enemyAnimation[i].Initialize(enemyTexture1, enemyTexture2, Vector2.Zero, 50, 50, 1, 50, Color.White, 1f, true);
            }
        }

        #endregion
    }
}

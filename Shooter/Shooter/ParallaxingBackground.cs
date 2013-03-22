// ParallaxingBackground.cs
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyPlat
{
    class ParallaxingBackground
    {
        // The image representing the parallaxing background
        Texture2D texture;

        // An array of positions of the parallaxing background
        Vector2[] positions;

        // The speed which the background is moving
        int speed;
        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
        {
            int tilesNeeded = 1;
            // Load the background texture we will be using
            texture = content.Load<Texture2D>(texturePath);

            // Set the speed of the background
            this.speed = speed;

            tilesNeeded = 4;
            if (tilesNeeded == 1) tilesNeeded++;

            // If we divide the screen with the texture width then we can determine the number of tiles need.
            // We add 1 to it so that we won't have a gap in the tiling
            positions = new Vector2[tilesNeeded];

            // Set the initial positions of the parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                positions[i] = new Vector2((i * texture.Width), 0);
            }
        }
        public void Update(LevelLibrary.Directions direction)
        {
            // Update the positions of the background
            for (int i = 0; i < positions.Length; i++)
            {
                Console.Write("Position[" + i + "].X = " + positions[i].X);
                Console.WriteLine();
                // Update the position of the screen by adding the speed
                if (direction == LevelLibrary.Directions.left)
                {
                    positions[i].X -= speed;
                    // Check the texture is out of view then put that texture at the end of the screen
                    if (positions[i].X <= -texture.Width)
                    {
                        positions[i].X = texture.Width * (positions.Length - 1);
                    }
                }
                else if (direction == LevelLibrary.Directions.right)
                {
                    positions[i].X += speed;
                    // Check if the texture is out of view then position it to the start of the screen
                    if (positions[i].X >= texture.Width * (positions.Length - 1))
                    {
                        positions[i].X = -texture.Width;
                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture, positions[i], Color.White);
            }
        }
    }
}

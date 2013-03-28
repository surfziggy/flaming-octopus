using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyPlat
{
    class HUDisplay
    {
        Texture2D lifeBar;
        Texture2D livesIcon;
        SpriteFont font;
        int health;
        int lives;
        
        /// <summary>
        /// Initialise the Heads Up Display
        /// </summary>
        public void Initialize(SpriteFont hudFont )
        {
            health = 0;
            lives = 0;
            font = hudFont;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            lifeBar = Content.Load<Texture2D>("GenericTextures\\healthBar");
            livesIcon = Content.Load<Texture2D>("GenericTextures\\livesIcon");
        }
                /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent(ContentManager Content)
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
       public void Update(GameTime gameTime, LevelLibrary.GameObject player)
        {
            health = player.Health;
            lives = player.Lives;
        }

        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, SpriteBatch hudBatch)
        {
            // Game info spritebatch
            hudBatch.Begin();

            // Draw the life bar
            hudBatch.Draw(lifeBar, new Rectangle( 50, 40, health, 9),  Color.White);

            // Draw an icon for each life left, 9x9 at a spacing of 20px horizontally
            for (int life = 0; life < lives; life++ )
            {
                hudBatch.Draw(livesIcon, new Rectangle((50 + (life * 20)), 50, 9, 9), Color.White);
            }

            hudBatch.End();
        }
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// Class      : Game1.cs
// Description: Main game class
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 


    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Private Members
        GraphicsDeviceManager graphics;                 // Graphics device
        Camera camera;                                  // Viewpoint of the player
        SpriteBatch spriteBatch;                        // Game Sprites
        SpriteBatch hudBatch;                           // Heads Up Display Sprites
        SpriteFont font;                                // Game font
        public Player player;                           // Represents the player 
        Input input;                                    // Handle the controller / keyboard
        HUDisplay hud;                                  // Heads.Up.Display - score, lives etc
        bool fullScreenMode = false;                    // Full screen or not
        ParallaxingBackground mainbg;                   // Parallaxing Layer 1
        ParallaxingBackground bgLayer1;                 // Parallaxing Layer 1
        ParallaxingBackground bgLayer2;                 // Parallaxing Layer 1
        LevelLibrary.LevelRenderer levelRenderer;       // Level rendering engine
        ParticleEngine particleEngine;                  // Collision particle engine
        #endregion 
        #region Public Methods
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;

            Content.RootDirectory = "Content";
            if (fullScreenMode)
            {
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }
        }
        #endregion
        #region Protected Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // Initialize the player class
            player = new Player();
            input = new Input();
            mainbg   = new ParallaxingBackground();
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            levelRenderer = new LevelLibrary.LevelRenderer();

            // Camera that follows the player
            camera = new Camera(GraphicsDevice.Viewport, Vector2.Zero);
            camera.Follow(player, 0f);
            hud = new HUDisplay();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            LoadLevel(1);

            // TODO move this into the player
            LevelLibrary.SpriteAnimator playerAnimation = new LevelLibrary.SpriteAnimator();
            Texture2D playerTexture1 = Content.Load<Texture2D>("GenericTextures\\walkingleft");
            Texture2D playerTexture2 = Content.Load<Texture2D>("GenericTextures\\walkingright");

            playerAnimation.Initialize(playerTexture1, playerTexture2, Vector2.Zero, 50, 50, 9, 50, Color.White, 1f, true);

            // TODO set initial position
            Vector2 playerPosition = new Vector2(100, 
                                                 350);
            player.Initialize(playerAnimation, playerPosition, GraphicsDevice.Viewport.Width,
                                            GraphicsDevice.Viewport.Height);

            // Load the parallaxing background
            mainbg.Initialize(Content, "GenericTextures\\mainbackground", GraphicsDevice.Viewport.Width, 1); 
            bgLayer1.Initialize(Content, "GenericTextures\\bgLayer1", GraphicsDevice.Viewport.Width, 2);
            bgLayer2.Initialize(Content, "GenericTextures\\bgLayer2", GraphicsDevice.Viewport.Width, 3);

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("GenericTextures\\circle"));
            textures.Add(Content.Load<Texture2D>("GenericTextures\\star"));
            textures.Add(Content.Load<Texture2D>("GenericTextures\\diamond"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));

            // Heads Up Display
            hudBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("gameFont");
            hud.LoadContent(Content);
            hud.Initialize(font);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            LevelLibrary.Directions direction = LevelLibrary.Directions.none;
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            UpdatePlayer(gameTime);

            // Update the parallaxing background
            mainbg.Update(direction);
            bgLayer1.Update(direction);
            bgLayer2.Update(direction);

            // Check for enemy collisions
            if (levelRenderer.HandleEnemyClash(player))
            {
                // Shake the camera.
                camera.Shake(0.5f, 5f, 1f);
                //particleEngine.Add(10);
            }

            // Update any particle effects
           // particleEngine.EmitterLocation = player.position;
           // particleEngine.Update();

            // Update the HUD
            hud.Update(gameTime, player);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Start drawing
           spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            // Draw the moving background
            mainbg.Draw(spriteBatch);
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);

            // Draw the level
            levelRenderer.Draw(gameTime, spriteBatch);

            // Draw the Player
            player.Draw(spriteBatch);
            particleEngine.Draw(spriteBatch);

            // Stop drawing
            spriteBatch.End();

            // Draw the HUD
            hud.Draw(gameTime, hudBatch);

            base.Draw(gameTime);
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Update the players direction based on key presses or other input
        /// </summary>
        private LevelLibrary.Directions UpdatePlayer(GameTime gameTime)
        {
            LevelLibrary.Directions direction = LevelLibrary.Directions.none;
            bool jump = false;
            bool left = false;
            bool right = false;
            // Update logic here
            input.UpdateInput(gameTime, camera);
            if (input.jumpKeyPressed == true)
            {
                jump = true;
            }
            else if (input.currentDirection == LevelLibrary.Directions.left)
            {
                left = true;
                direction = LevelLibrary.Directions.right;
            }
            else if (input.currentDirection == LevelLibrary.Directions.right)
            {
                right = true;
                direction = LevelLibrary.Directions.left;
            }
            else if (input.exitKeyPressed == true)
            {
                this.Exit();
            }
            else if (input.fullScreenPressed == true)
            {
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
                this.fullScreenMode = true;
            }
            // Update the player position
            player.Update(levelRenderer, gameTime, jump, left, right);

            return (direction);
        }
        /// <summary>
        /// Load the specified level and initialise all the required objects for it
        /// </summary>
        /// 
        private void LoadLevel(int levelNumber)
        {
            String levelName;
            String levelTexturesName;

            // Load the map (.level) file and 
            levelName = "LevelMaps\\level" + levelNumber;
            levelTexturesName = "LevelTextures\\level" + levelNumber;
            LevelLibrary.Level levelMap = Content.Load<LevelLibrary.Level>(levelName); 
            Texture2D levelTexture = Content.Load<Texture2D>(levelTexturesName);

#if MODULE_DEBUG
            levelRenderer.DumpLevel();
#endif
            levelRenderer.LoadContent(Content);
            levelRenderer.Initialize(levelMap, levelTexture, 50, 50, 6, Color.White, 1f);
        }
        #endregion
    }
}

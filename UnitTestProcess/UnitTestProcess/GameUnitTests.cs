//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// Class      : 
// Description: 
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

namespace UnitTestProcess
{
    enum TestResult
    {
        Pass = 1,
        Fail = 2
    };
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameUnitTests : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LevelLibrary.LevelRenderer levelRenderer;

        public GameUnitTests()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            levelRenderer = new LevelLibrary.LevelRenderer();

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

            // TODO: use this.Content to load your game content here
            LoadLevel(1);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            RunUnitTests();
            base.Draw(gameTime);
        }

        private void LoadLevel(int levelNumber)
        {
            String levelName;
            String levelTexturesName;

            // Load the map (.level) file and 
            levelName = "LevelMaps\\test" + levelNumber;
            levelTexturesName = "LevelTextures\\level" + levelNumber;
            Texture2D levelTexture = Content.Load<Texture2D>(levelTexturesName);
            LevelLibrary.Level levelMap = Content.Load<LevelLibrary.Level>(levelName);
#if MODULE_DEBUG
            levelRenderer.DumpLevel();
#endif
            levelRenderer.LoadContent(Content);
            levelRenderer.Initialize(levelMap, levelTexture, 50, 50, 6, Color.White, 1f);
        }

        private void RunUnitTests()
        {
            TestResult result = TestResult.Fail;
            result = TestSuite1();
        }

        private TestResult TestSuite1()
        {
            // Results
            TestResult result = TestResult.Fail;
            TestResult suiteResult = TestResult.Pass;            

            Console.Write("Running Test Suite 1 COLLISIONS");
            Console.WriteLine();



            result = Test1_1();
            if (result == TestResult.Fail)
            {
                suiteResult = TestResult.Fail;
            }

            result = Test1_2();
            if (result == TestResult.Fail)
            {
                suiteResult = TestResult.Fail;
            }

            result = Test1_3();
            if (result == TestResult.Fail)
            {
                suiteResult = TestResult.Fail;
            }

            result = Test1_4();
            if (result == TestResult.Fail)
            {
                suiteResult = TestResult.Fail;
            }

            result = Test1_5();
            if (result == TestResult.Fail)
            {
                suiteResult = TestResult.Fail;
            }

            result = Test1_6();
            if (result == TestResult.Fail)
            {
                suiteResult = TestResult.Fail;
            }

            return (suiteResult);
        }

        // Middle
        // xxxxx
        // x   x
        // x p x
        // x   x 
        // xxxxx
        // Test no expected collision
        private TestResult Test1_1()
        {
            bool ground = false;
            // Result
            TestResult result = TestResult.Fail;      
            // Test scratch variables
            bool clash = false;
            Vector2 position;
            // No collision expected
            Console.Write("Test 1:1 no collision expected: ");
            position.X = 100f;
            position.Y = 100f;

            clash = levelRenderer.HandleClash(ref position, 50, 50, ref ground);
            // If no clash and position is still the same - PASS
            if (!clash && 
                (position.X == 100f && position.Y == 100f) &&
                (ground == false))
            {
                result = TestResult.Pass;
                Console.Write(" PASS");
            }
            else
            {
                result = TestResult.Fail;
                Console.Write(" FAIL");
            }
            Console.WriteLine();
            return (result);
        }

        // Top left
        // pxxxx
        // x   x
        // x   x
        // x   x 
        // xxxxx
        private TestResult Test1_2()
        {
            bool ground = false;
            // Result
            TestResult result = TestResult.Fail;
            // Test scratch variables
            bool clash = false;
            Vector2 position;
            // No collision expected
            Console.Write("Test 1:2 collision expected: ");
            position.X = 48f;
            position.Y = 49;

            clash = levelRenderer.HandleClash(ref position, 50, 50, ref ground);
            // If no clash and position is still the same - PASS
            if (clash == true && 
                (position.X == 50f && position.Y == 50f) &&
                (ground == false))
            {
                result = TestResult.Pass;
                Console.Write(" PASS");
            }
            else
            {
                result = TestResult.Fail;
                Console.Write(" FAIL");
            }
            Console.WriteLine();
            return (result);
        }

        // Top right
        // xxxxp
        // x   x
        // x   x
        // x   x 
        // xxxxx
        private TestResult Test1_3()
        {
            bool ground = false;
            // Result
            TestResult result = TestResult.Fail;
            // Test scratch variables
            bool clash = false;
            Vector2 position;
            // No collision expected
            Console.Write("Test 1:3 collision expected: ");
            position.X = 151f;
            position.Y = 49;

            clash = levelRenderer.HandleClash(ref position, 50, 50, ref ground);
            // If no clash and position is still the same - PASS
            if (clash == true && 
                (position.X == 150f && position.Y == 50f) &&
                (ground == false))
            {
                result = TestResult.Pass;
                Console.Write(" PASS");
            }
            else
            {
                result = TestResult.Fail;
                Console.Write(" FAIL");
            }
            Console.WriteLine();
            return (result);
        }

        // bottom
        // xxxxx
        // x   x
        // x   x
        // x   x 
        // xxpxx
        private TestResult Test1_4()
        {
            bool ground = false;
            // Result
            TestResult result = TestResult.Fail;
            // Test scratch variables
            bool clash = false;
            Vector2 position;
            // No collision expected
            Console.Write("Test 1:4 collision expected: ");
            position.X = 100f;
            position.Y = 155f;

            clash = levelRenderer.HandleClash(ref position, 50, 50, ref ground);
            // If no clash and position is still the same - PASS
            if (clash == true && 
                (position.X == 100f && position.Y == 150f) &&
                (ground == true))
            {
                result = TestResult.Pass;
                Console.Write(" PASS");
            }
            else
            {
                result = TestResult.Fail;
                Console.Write(" FAIL");
            }
            Console.WriteLine();
            return (result);
        }

        // bottom left
        // xxxxx
        // x   x
        // x   x
        // x   x 
        // pxxxx        
        private TestResult Test1_5()
        {
            bool ground = false;
            // Result
            TestResult result = TestResult.Fail;
            // Test scratch variables
            bool clash = false;
            Vector2 position;
            // No collision expected
            Console.Write("Test 1:5 collision expected: ");
            position.X = 49f;
            position.Y = 155f;

            clash = levelRenderer.HandleClash(ref position, 50, 50, ref ground);
            // If no clash and position is still the same - PASS
            if (clash == true && 
                (position.X == 50f && position.Y == 150f) &&
                (ground == true))
            {
                result = TestResult.Pass;
                Console.Write(" PASS");
            }
            else
            {
                result = TestResult.Fail;
                Console.Write(" FAIL");
            }
            Console.WriteLine();
            return (result);
        }

        // bottom right
        // xxxxx
        // x   x
        // x   x
        // x   x 
        // xxxxp
        private TestResult Test1_6()
        {
            bool ground = false;
            // Result
            TestResult result = TestResult.Fail;
            // Test scratch variables
            bool clash = false;
            Vector2 position;
            // No collision expected
            Console.Write("Test collision expected: ");
            position.X = 151f;
            position.Y = 155f;

            clash = levelRenderer.HandleClash(ref position, 50, 50, ref ground);
            // If no clash and position is still the same - PASS
            if (clash == true && 
                (position.X == 150f && position.Y == 150f) &&
                (ground == true))
            {
                result = TestResult.Pass;
                Console.Write(" PASS");
            }
            else
            {
                result = TestResult.Fail;
                Console.Write(" FAIL");
            }
            Console.WriteLine();
            return (result);
        }
    }
}

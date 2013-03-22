//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// Class      : input.cs
// Description: Handle input from keyboard/ controller
//                  - Abstract keypresses / buttons from the game function
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyPlat
{
    class Input
    {
        /// <summary>
        /// Public variables    
        /// </summary>
        public LevelLibrary.Directions   currentDirection;
        public bool         jumpKeyPressed     = false;
        public bool         exitKeyPressed     = false;
        public bool         fullScreenPressed  = false;
        /// <summary>
        /// Private data
        /// </summary>
        private KeyboardState oldState;
        private Keys leftKeyMapping;
        private Keys rightKeyMapping;
        private Keys jumpKeyMapping;
        private Keys exitKeyMapping;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Initialise method 
        // Get the current state of the keyboard; any keys pressed
        // Set the direction to static
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Input()
        {
            leftKeyMapping  = Keys.N;
            rightKeyMapping = Keys.M;
            jumpKeyMapping = Keys.Space;
            exitKeyMapping = Keys.Escape;
            
            oldState = Keyboard.GetState();
            currentDirection = LevelLibrary.Directions.none;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Read the keypresses
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdateInput(GameTime gameTime, Camera camera)
        {
            currentDirection = LevelLibrary.Directions.none;
            jumpKeyPressed = false;
            KeyboardState newState = Keyboard.GetState();

            camera.Update(gameTime, newState, oldState);

            // Is the Q key down?
            if (newState.IsKeyDown(jumpKeyMapping))
            {
                jumpKeyPressed = true;
            }
            else if (newState.IsKeyDown(leftKeyMapping))
            {
                currentDirection = LevelLibrary.Directions.left;
            }
            else if (newState.IsKeyDown(rightKeyMapping))
            {
                currentDirection = LevelLibrary.Directions.right;
            }
            if (newState.IsKeyDown(exitKeyMapping))
            {
                exitKeyPressed = true;
            }

            // Update saved state.
            oldState = newState;
        }
    }
}


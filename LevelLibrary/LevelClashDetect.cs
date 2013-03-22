using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelLibrary
{
    class LevelClashDetect
    {
#if SAVED_CODE
        // Logic to check what's at a particular coordinate in space
        public Sides CheckForPlatformClash(Vector2 position, int Width, int Height, ref float platformBelowsYCoord)
        {
            int row = 0;
            int column = 0;
            int data = 0;
            Sides sides = Sides.none;

            // TOP TOP TOP TOP 
            // Check the top midpoint to see what's in the square above
            column = ((int)((position.X + (Width / 2)) / gridSquareWidth));
            row = ((int)position.Y / gridSquareHeight);

            // Is it solid?
            if (levelData.IsSupporting(row, column))
            {
                sides |= Sides.top;
#if MODULE_DEBUG
                Console.Write("Top clash row"+  row+ "col "+ column + "data is"+data);
                Console.WriteLine();
#endif
            }

            // BOTTOM BUM BUM
            // Check the bottom midpoint to see what's in the square below
            column = ((int)((position.X + (Width / 2)) / gridSquareWidth));
            row = ((int)(position.Y + (Height)) / gridSquareHeight);
            data = levelData.GetValue(row, column);

            // Is it solid?
            if (levelData.IsSupporting(data))
            {
#if MODULE_DEBUG
                Console.Write("Bottom clash row" + row + "col " + column + "data is" + data);
                Console.WriteLine(); 
#endif
                sides |= Sides.bottom;

                // Set the supporting platform to the square below (remember to subtract the height)
                // As we're talking about the top of the sprite
            }

            // LEFTY LEFT
            // Check the left midpoint to see what's in the to the side
            column = ((int)((position.X) / gridSquareWidth));
            row = ((int)(position.Y + (Height / 2)) / gridSquareHeight);
            data = levelData.GetValue(row, column);

            // Is it solid?
            if (levelData.IsSupporting(data))
            {
                sides |= Sides.left;
#if MODULE_DEBUG
                Console.Write("LEFT clash row" + row + "col " + column + "data is" + data);
                Console.WriteLine();
#endif
            }

            // RIGHTY IGHTY
            // Check the right midpoint to see what's in the to the side
            column = ((int)((position.X + Width) / gridSquareWidth));
            row = ((int)(position.Y + (Height / 2)) / gridSquareHeight);
            data = levelData.GetValue(row, column);

            // Is it solid?
            if (levelData.IsSupporting(data))
            {
                sides |= Sides.right;
#if MODULE_DEBUG
                Console.Write("RIGHT clash row" + row + "col " + column + "data is" + data);
                Console.WriteLine();
#endif
            }


            // Calculate where the platform we will hit if we fall 
            // The y coordinate is the top of the player sprite
            // So, the guy actually stops 50px above the platform
            // but the sprite is draw down to the platform.
            {
                int i = 0, numRows;

                numRows = levelData.Rows;
                i = row + 1;
                for (i = row + 1; i < numRows; i++)
                {
                    if (levelData.IsSupporting(levelData.GetValue(i, column)))
                    {
#if MODULE_DEBUG
                       // Console.Write("Found a platform row " + i);
                        //Console.WriteLine();
#endif
                        break;
                    }
                }
                platformBelowsYCoord = (float)(((i - 1) * Height));

            }

            return (sides);
        }
#endif
    }
}

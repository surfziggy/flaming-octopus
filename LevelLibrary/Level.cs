//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// Class      : Level.cs
// Description: Stores the data for a single level in the game
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelLibrary
{
    public class Level
    {
        public int[,]EnemyPositions;
        public int numberAliens;
        public int[] EnemyTypes;
        public int[,] Values { get; set; }
        private Tile[,] tiles;
        private int tileHeight;
        private int tileWidth;
        private Rectangle[,] blockList;


        public Level(int[,] values)
        {
            int row, column;
            Values = values;

            tileHeight = 50;
            tileWidth = 50;

            // Build a list of rectangles for all of the tiles in the level including empty tiles.
            buildBlockList();

            // Create an array (with two axis) of all the tiles.
            tiles = new Tile[values.GetLength(0), values.GetLength(1)];
            for (row = 0; row < values.GetLength(0); row++)
            {
                for (column = 0; column < values.GetLength(1); column++)
                {
                    tiles[row, column] = new Tile(values[row, column]);
                }
            }
        }

        public bool IsSupporting(int row, int column)
        {
            bool supporting = true;

            if((row>=0 && row < Rows) && (column>=0 && column < Columns))
            {
                if( (tiles[row, column].Collision == TileCollision.Passable) ||
                    (tiles[row, column].Collision == TileCollision.LevelExit))
                {
                    supporting = false;
                }       
            }
            return (supporting);
        }

        public bool isExit(int row, int column)
        {
            bool isExitReached = false;

            if ((row >= 0 && row < Rows) && (column >= 0 && column < Columns))
            {
                if (tiles[row, column].Collision == TileCollision.LevelExit)
                {
                    isExitReached = true;
                }
            }
            return (isExitReached);
        }

        public int GetValue(int row, int column)
        {
            int val = 0;

            if ((row >= 0) && (column >= 0))
            {
                if ((row < Values.GetLength(0)) &&
                   (column < Values.GetLength(1)))
                {
                    val = Values[row, column];
                }
                else
                {
#if MODULE_DEBUG
                    Console.Write("Row/Column out of bounds");
                    Console.WriteLine();
#endif
                    return (0);
                }
            }
            return (val);
        }

        public int Rows
        {
            get
            {
                return Values.GetLength(0);
            }
        }

        public int Columns
        {
            get
            {
                return Values.GetLength(1);
            }
        }

        private void buildBlockList()
        {
            int row, column;
            int x = 0;
            int y = 0;
            // Create an array of rectangles to store the list of blocks within
            blockList = new Rectangle[this.Rows+1, this.Columns+1];
            x = 0;
            y = 0;

            for (row = 0; row <= this.Rows; row++)
            {
                x = 0;
                for (column = 0; column <= this.Columns; column++)
                {
                    blockList[row, column] = new Rectangle(x, y, this.tileWidth, this.tileHeight);
                    x += tileWidth;
                }
                y += tileHeight;
            }
        }

        public Rectangle GetRectangle(int row, int column)
        {
            if ((row >= 0 && row < this.Rows) &&
                (column >= 0 && column < this.Columns))
            {
                return (blockList[row, column]);
            }
            else
            {
                return (blockList[0,0]);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TRead = LevelLibrary.Level;

namespace LevelLibrary
{
    public class LevelReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            Level level;
            int rows = input.ReadInt32();
            int columns = input.ReadInt32();

            int[,] levelData = new int[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    levelData[row, column] = input.ReadInt32();
                }
            }

            level = new Level(levelData);

            level.numberAliens = input.ReadInt32();
            level.EnemyTypes = new int[level.numberAliens];
            level.EnemyPositions = new int[level.numberAliens, 2];
            for (int i = 0; i < level.numberAliens; i++)
            {
                int x = 0, y = 0;
                int val = input.ReadInt32();
                level.EnemyTypes[i] = val;

                x = input.ReadInt32();
                y = input.ReadInt32();

                level.EnemyPositions[i,0] = (int)x;
                level.EnemyPositions[i,1] = (int)y;
            }

            return (level);
        }
    }
}

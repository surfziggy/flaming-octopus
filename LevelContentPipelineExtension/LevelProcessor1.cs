using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// These are the processor input and output types.
using TInput = System.String;
using TOutput = LevelLibrary.Level;

namespace LevelContentPipelineExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Level Processor")]
    public class LevelProcessor1 : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            string[] lines = input.Split(new char[] { '\n' });
            int rows = Convert.ToInt32(lines[0]);
            int columns = Convert.ToInt32(lines[1]);
            int lineno = 0, i = 0;
            LevelLibrary.Level level;

            int[,] levelData = new int[rows, columns];
            for (int row = 0; row < rows; row++)
            {
                string[] values = lines[row + 2].Split(new char[] { ' ' });
                for (int column = 0; column < columns; column++)
                {
                    levelData[row, column] = Convert.ToInt32(values[column]);
                }
            }

            level = new LevelLibrary.Level(levelData);

            // Aliens

            lineno = rows+2;

            level.numberAliens = Convert.ToInt32(lines[lineno]);
            lineno++;
            level.EnemyTypes = new int[level.numberAliens];
            level.EnemyPositions = new int[level.numberAliens, 2];
            for (i = 0; i < level.numberAliens; i++)
            {
                string[] values = lines[lineno + i].Split(new char[] { ' ' });
                int p = (int) Convert.ToInt32(values[0]);
                level.EnemyTypes[i] = p;
                level.EnemyPositions[i,0] = (int)Convert.ToInt32(values[1]);
                level.EnemyPositions[i,1] = (int)Convert.ToInt32(values[2]);
            }
            return (level);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

// the type you want to write out.
using TWrite = LevelLibrary.Level;

namespace LevelContentPipelineExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class LevelWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            output.Write(value.Rows);
            output.Write(value.Columns);
            for (int row = 0; row < value.Rows; row++)
            {
                for (int column = 0; column < value.Columns; column++)
                {
                    output.Write(value.GetValue(row, column));
                }
            }
            output.Write(value.numberAliens);

            for (int i = 0; i < value.numberAliens; i++)
            {
                output.Write(value.EnemyTypes[i]);
                output.Write((int)value.EnemyPositions[i,0]);
                output.Write((int)value.EnemyPositions[i,1]);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "LevelLibrary.LevelReader, LevelLibrary";
        }
    }
}

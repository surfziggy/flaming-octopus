//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 
// Class      : 
// Description: 
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace UnitTestProcess
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameUnitTests game = new GameUnitTests())
            {
                game.Run();
            }
        }
    }
#endif
}


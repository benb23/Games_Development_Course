using System;

namespace A19_Ex02_Ben_305401317_Dana_311358543
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new SpaceInvaders())
            { 
                game.Run();
            }
        }
    }
#endif
}

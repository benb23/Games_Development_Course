using System;

namespace A19_Ex01_Ben_305401317_Dana_311358543
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new SpaceInvaders())
            { 
                game.Run();
            }
        }//test
    }
#endif
}

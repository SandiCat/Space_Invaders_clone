using System;

namespace Space_Invaders_clone
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SpaceInvaders game = new SpaceInvaders())
            {
                game.Run();
            }
        }
    }
}


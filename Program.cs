using Raylib_cs;
using RPG_JKL.Managers;

namespace RPG_JKL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            
            gameManager.Init();

            while (!Raylib.WindowShouldClose())
            {
                gameManager.Update();

                gameManager.Draw();
            }
            Raylib.CloseWindow();
        }
    }
}

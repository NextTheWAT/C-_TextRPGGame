using System;
using System.Threading;

namespace TextRPG
{
    public class BattleSystem
    {
        public static int dungeonClearCount = 0;

        private enum DungeonDifficulty
        {
            StartScene = 0,
            Easy = 1,
            Normal = 2,
            Hard = 3
        }

        public static void ShowDungeonMenu()
        {
            Console.Clear();
            GameSystem.DungeonScreen(); // 큰 UI 출력

            Console.WriteLine("0. 🔙 돌아가기");
            Console.Write("\n>> ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    GameSystem.ShowDungeon1Scene();
                    break;
                case "2":
                    GameSystem.ShowDungeon2Scene();
                    break;
                case "3":
                    GameSystem.ShowDungeon3Scene();
                    break;
                case "0":
                    Program.StartGame();
                    break;
                default:
                    GameSystem.FaileInput();
                    ShowDungeonMenu();
                    break;
            }
        }



    }
}

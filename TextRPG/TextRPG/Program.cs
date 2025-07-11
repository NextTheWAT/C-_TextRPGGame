using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public static class Program
    {
        public static Player player = new Player();
        public static Inventory inventory = new Inventory();
        public static Shop shop = new Shop();
        public static BattleSystem battleSystem = new BattleSystem();



        enum Scene 
        {
            Start,
            PlayerInfo,
            Shop,
            Inventory,
            Dungeon,
            Data
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            shop.InitializeItems();
            StartGame();
        }


        public static void StartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                🏰 스파르타 마을에 오신 걸 환영합니다!                 ║");
            Console.WriteLine("╠═══════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 이곳에서 던전에 들어가기 전 활동을 할 수 있습니다.                    ║");
            Console.WriteLine("║ 아래 메뉴 중 하나를 선택해주세요.                                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  [메뉴]");
            Console.WriteLine("  0. 🔁 시작 화면");
            Console.WriteLine("  1. 🧍 플레이어 상태");
            Console.WriteLine("  2. 🏪 상점");
            Console.WriteLine("  3. 🎒 인벤토리");
            Console.WriteLine("  4. ⚔️ 던전 입장"); // ✅ 추가
            Console.WriteLine("  5. 💾 저장 / 불러오기");
            Console.ResetColor();

            Console.Write("\n>> ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int Sel))
            {
                Scene scene = (Scene)Sel;
                //Console.Clear();
                switch (scene)
                {
                    case Scene.Start:
                        StartGame();
                        break;
                    case Scene.PlayerInfo:
                        player.PlayerInfo();
                        StartGame();
                        break;
                    case Scene.Shop:
                        Console.Clear();
                        shop.ShowShopMenu();
                        break;
                    case Scene.Inventory:
                        inventory.ShowInventory();
                        break;
                    case Scene.Dungeon:
                        BattleSystem.ShowDungeonMenu();
                        break;
                    case Scene.Data:
                        SaveData.DataUI();
                        break;
                    default:
                        GameSystem.FaileInput();
                        StartGame();
                        break;
                }
            }
            else
            {
                GameSystem.StringPrintRed("잘못된 입력입니다. 숫자만 입력해주세요.");
                StartGame();
            }
        }


    }
}

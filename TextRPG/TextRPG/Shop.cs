using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace TextRPG
{
    public class Shop
    {
        //public List<Item> Items { get; private set; } = new List<Item>();

        public enum ShopMenu
        {
            ShopMenu = 1,
            BuyItem = 2,
            SellItem = 3,
            Exit = 4
        }

        public void ShowShopMenu()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════╗");
            Console.WriteLine("║       🏪 상점 메뉴         ║");
            Console.WriteLine("╚════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("1. 📦 아이템 보기");
            Console.WriteLine("2. 💰 아이템 구매");
            Console.WriteLine("3. 🪙 아이템 판매");
            Console.WriteLine("4. 🔙 시작 화면으로");
            Console.Write("\n메뉴 선택: ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out int Sel))
            {
                ShopMenu menu = (ShopMenu)Sel;

                switch (menu)
                {
                    case ShopMenu.ShopMenu:
                        Console.Clear();
                        ShowShopItems();
                        //ShowShopMenu();
                        break;
                    case ShopMenu.BuyItem:
                        //Console.Clear();
                        BuyItem();
                        //ShowShopMenu();
                        break;
                    case ShopMenu.SellItem:
                        Console.Clear();
                        SellItem();
                        //ShowShopMenu();
                        break;
                    case ShopMenu.Exit:
                        //Console.Clear();
                        Program.StartGame();
                        break;
                    default:
                        GameSystem.FaileInput();
                        //ShowShopMenu();
                        break;
                }
                //ShowShopMenu();
            }
            Console.WriteLine("\n아무 키나 누르면 상점 메뉴로 돌아갑니다...");
            Console.ReadKey();
            ShowShopMenu();
        }

        public void ShowShopItems()
        {
            //Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                        🛍️ 상점 아이템 목록                                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine($"{AdjustWidth("이름", 18)} {AdjustWidth("종류", 8)} {AdjustWidth("스탯", 8)} {AdjustWidth("설명", 60)} {AdjustWidth("가격", 10)}");
            Console.WriteLine(new string('-', 120));

            foreach (var item in Item.Items)
            {
                string type = item.Type == ItemType.Weapon ? "공격력" : "방어력";
                string stat = $"+{item.StatValue}";
                string priceStr = item.Price == 0 ? "구매완료" : $"{item.Price} G";

                Console.ForegroundColor = item.Price != 0 ? ConsoleColor.Green : ConsoleColor.DarkGray; // 가격이 0이 아니면 초록색, 0이면 회색 삼항 연산자

                if (item.Price != 0) Console.ForegroundColor = ConsoleColor.Green; else Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine($"{AdjustWidth(item.Name, 18)} {AdjustWidth(type, 8)} {AdjustWidth(stat, 8)} {AdjustWidth(item.Info, 60)} {AdjustWidth(priceStr, 10)}");
            }
            Console.ResetColor();

            Console.WriteLine(new string('-', 120));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"💰 소지 Gold  : {Program.player.Gold} G");
            Console.ResetColor();
            Console.WriteLine("\n아무 키나 누르면 상점 메뉴로 돌아갑니다...");
            Console.ReadKey();
            ShowShopMenu();
        }

        public void BuyItem()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                        🛍️ 상점 아이템 구매                                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine($"{AdjustWidth("번호", 4)} {AdjustWidth("이름", 18)} {AdjustWidth("종류", 8)} {AdjustWidth("스탯", 8)} {AdjustWidth("설명", 60)} {AdjustWidth("가격", 10)}");
            Console.WriteLine(new string('-', 120));

            for (int i = 0; i < Item.Items.Count; i++)
            {
                var item = Item.Items[i];
                string indexStr = $"{i + 1}.";
                string type = item.Type == ItemType.Weapon ? "공격력" : "방어력";
                string stat = $"+{item.StatValue}";
                string priceStr = item.Price == 0 ? "구매완료" : $"{item.Price} G";

                Console.ForegroundColor = item.Price != 0 ? ConsoleColor.Green : ConsoleColor.DarkGray; // 가격이 0이 아니면 초록색, 0이면 회색 삼항 연산자

                Console.WriteLine($"{AdjustWidth(indexStr, 4)} {AdjustWidth(item.Name, 18)} {AdjustWidth(type, 8)} {AdjustWidth(stat, 8)} {AdjustWidth(item.Info, 60)} {AdjustWidth(priceStr, 10)}");
            }
            Console.ResetColor(); // 원래 색상으로 되돌림
            Console.WriteLine(new string('-', 120));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"💰 소지 Gold  : {Program.player.Gold} G");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("0. 뒤로가기");
            Console.Write("구매할 아이템 번호를 입력하세요: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= Item.Items.Count)
            {
                var selectedItem = Item.Items[choice - 1];
                if (selectedItem.Price == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    Console.ResetColor();
                    Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                    Console.ReadKey();    
                    Console.Clear();
                    BuyItem();
                    return;
                }
                else if (Program.player.Gold >= selectedItem.Price)
                {
                    Program.player.Gold -= selectedItem.Price;
                    selectedItem.Price = 0; // 구매 완료 처리

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n✅ {selectedItem.Name}을(를) 구매했습니다!");
                    Console.WriteLine($"💰 남은 Gold : {Program.player.Gold} G");
                    Console.ResetColor();

                    Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                    Console.ReadKey();

                    Console.Clear();
                    BuyItem();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ Gold가 부족합니다.");
                    Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                    Console.ReadKey();

                    Console.Clear();
                    BuyItem();
                }
                Console.ResetColor();
            }
            else if (choice != 0)
            {
                GameSystem.FaileInput();
            }
            else if (choice == 0)
            {
                Console.Clear();
                ShowShopMenu();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public void SellItem()
        {
            var inventoryItems = Item.Items.Where(i => i.Price == 0).ToList(); // ✅ 수정된 부분

            if (inventoryItems.Count == 0)
            {
                GameSystem.StringPrintRed("인벤토리에 판매할 아이템이 없습니다.");
                Console.WriteLine("\n아무 키나 누르면 상점 메뉴로 돌아갑니다...");
                Console.ReadKey();
                Console.Clear();
                ShowShopMenu();
                return;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                           🛍️ 아이템 판매                                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine($"{AdjustWidth("번호", 4)} {AdjustWidth("이름", 18)} {AdjustWidth("종류", 8)} {AdjustWidth("스탯", 8)} {AdjustWidth("설명", 60)} {AdjustWidth("가격", 12)}");
            Console.WriteLine(new string('-', 120));

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                var item = inventoryItems[i];
                string indexStr = $"{i + 1}.";
                string type = item.Type == ItemType.Weapon ? "공격력" : "방어력";
                string stat = $"+{item.StatValue}";
                string priceStr = $"{(int)(item.OriginalPrice * 0.85)} G (판매가)";

                Console.WriteLine($"{AdjustWidth(indexStr, 4)} {AdjustWidth(item.Name, 18)} {AdjustWidth(type, 8)} {AdjustWidth(stat, 8)} {AdjustWidth(item.Info, 60)} {AdjustWidth(priceStr, 12)}");
            }

            Console.WriteLine(new string('-', 120));
            Console.WriteLine("0. 뒤로가기");
            Console.Write("판매할 아이템 번호를 입력하세요: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= inventoryItems.Count)
            {
                var selectedItem = inventoryItems[choice - 1];

                // 장착 중이면 해제 후 능력치 감소
                if (selectedItem.IsEquipped)
                {
                    selectedItem.IsEquipped = false;

                    if (selectedItem.Type == ItemType.Weapon)
                        Program.player.Attack -= selectedItem.StatValue;
                    else if (selectedItem.Type == ItemType.Armor)
                        Program.player.Defense -= selectedItem.StatValue;
                }

                // 판매가 계산 (85%로)
                int sellPrice = (int)(selectedItem.OriginalPrice * 0.85);
                Program.player.Gold += sellPrice;

                // 상태 초기화
                selectedItem.Price = selectedItem.OriginalPrice;

                // 인벤토리에서 제거
                inventoryItems.Remove(selectedItem);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n🧷 {selectedItem.Name}을(를) 판매했습니다!");
                Console.WriteLine($"💰 현재 Gold : {Program.player.Gold} G");
                Console.ResetColor();

                Console.WriteLine("\n아무 키나 누르면 판매로 돌아갑니다...");
                Console.ReadKey();
                Console.Clear();
                SellItem();
            }
            else if (choice == 0)
            {
                Console.Clear();
                ShowShopMenu();
            }
            else
            {
                GameSystem.FaileInput();
                Console.WriteLine("\n아무 키나 누르면 다시 판매 화면으로 돌아갑니다...");
                Console.ReadKey();
                Console.Clear();
                SellItem(); // 재진입
            }

            Console.WriteLine();
        }


        public static string AdjustWidth(string input, int totalWidth)
        {
            int width = input.Sum(c => c > 127 ? 2 : 1); // 한글은 2칸, 영문은 1칸
            int padding = totalWidth - width;
            return input + new string(' ', Math.Max(padding, 0));
        }

        public void InitializeItems()
        {
            Item.Items.Add(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, ItemType.Armor, 5));
            Item.Items.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, ItemType.Armor, 9));
            Item.Items.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, ItemType.Armor, 15));
            Item.Items.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 600, ItemType.Weapon, 2));
            Item.Items.Add(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1500, ItemType.Weapon, 5));
            Item.Items.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 5000, ItemType.Weapon, 7));
        }








    }
}
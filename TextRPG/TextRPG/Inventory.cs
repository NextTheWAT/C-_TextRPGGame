using System;
using System.Collections.Generic;
using System.Linq;

namespace TextRPG
{
    public class Inventory
    {
        public void ShowInventory()
        {
            Console.Clear();
            GameSystem.PlayerInfo_Color();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                          🎒 인벤토리                                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("보유 중인 아이템을 확인하고 장착할 수 있습니다.\n");

            var ownedItems = Item.Items.Where(i => i.Price == 0).ToList();

            if (ownedItems.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("[아이템 없음]");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[아이템 목록]");
                Console.ResetColor();
                Console.WriteLine(new string('-', 80));

                foreach (var item in ownedItems)
                {
                    string equippedMark = item.IsEquipped ? "[E] " : "    ";
                    string statType = item.Type == ItemType.Weapon ? "🗡️ 공격력" : "🛡️ 방어력";
                    Console.ForegroundColor = item.IsEquipped ? ConsoleColor.Green : ConsoleColor.Gray;
                    Console.WriteLine($"{equippedMark}{item.Name} | {statType} +{item.StatValue} | {item.Info}");
                }

                Console.ResetColor();
                Console.WriteLine(new string('-', 80));
            }

            Console.WriteLine("\n[메뉴]");
            Console.WriteLine("1. ⚙️ 장착 관리");
            Console.WriteLine("0. 🔙 나가기");
            Console.Write(">> ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    ManageEquip(Program.player);
                    break;
                case "0":
                    Console.Clear();
                    Console.WriteLine("인벤토리를 종료합니다.\n");
                    Program.StartGame();
                    break;
                default:
                    GameSystem.FaileInput();
                    ShowInventory();
                    break;
            }
        }

        public void ManageEquip(Player player)
        {
            while (true)
            {
                var ownedItems = Item.Items.Where(i => i.Price == 0).ToList();

                Console.Clear();
                GameSystem.PlayerInfo_Color();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                                        ⚙️ 장착 관리                                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════╝");
                Console.ResetColor();

                for (int i = 0; i < ownedItems.Count; i++)
                {
                    var item = ownedItems[i];
                    string equippedMark = item.IsEquipped ? "[E] " : "    ";
                    string statType = item.Type == ItemType.Weapon ? "🗡️ 공격력" : "🛡️ 방어력";
                    Console.ForegroundColor = item.IsEquipped ? ConsoleColor.Green : ConsoleColor.Gray;
                    Console.WriteLine($"{i + 1}. {equippedMark}{item.Name} | {statType} +{item.StatValue} | {item.Info}");
                }

                Console.WriteLine("\n0. 🔙 나가기");
                Console.Write(">> ");
                string selectInput = Console.ReadLine();

                if (selectInput == "0") break;

                if (int.TryParse(selectInput, out int selected) && selected >= 1 && selected <= ownedItems.Count)
                {
                    Item selectedItem = ownedItems[selected - 1];

                    if (!selectedItem.IsEquipped)
                    {
                        if (selectedItem.Type == ItemType.Weapon)
                        {
                            var equippedWeapons = ownedItems.Where(i => i.IsEquipped && i.Type == ItemType.Weapon).ToList();
                            if (equippedWeapons.Count >= 2)
                            {
                                var oldest = equippedWeapons.First();
                                oldest.IsEquipped = false;
                                player.Attack -= oldest.StatValue;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"\n🧷 {oldest.Name} 을(를) 자동으로 해제했습니다.");
                                Console.ResetColor();
                            }

                            selectedItem.IsEquipped = true;
                            player.Attack += selectedItem.StatValue;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n✅ {selectedItem.Name} 을(를) 장착했습니다.");
                        }
                        else if (selectedItem.Type == ItemType.Armor)
                        {
                            var equippedArmor = ownedItems.FirstOrDefault(i => i.IsEquipped && i.Type == ItemType.Armor);
                            if (equippedArmor != null)
                            {
                                equippedArmor.IsEquipped = false;
                                player.Defense -= equippedArmor.StatValue;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"\n🧷 {equippedArmor.Name} 을(를) 자동으로 해제했습니다.");
                                Console.ResetColor();
                            }

                            selectedItem.IsEquipped = true;
                            player.Defense += selectedItem.StatValue;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n✅ {selectedItem.Name} 을(를) 장착했습니다.");
                        }
                        Console.ResetColor();
                    }
                    else
                    {
                        selectedItem.IsEquipped = false;
                        if (selectedItem.Type == ItemType.Weapon) player.Attack -= selectedItem.StatValue;
                        else if (selectedItem.Type == ItemType.Armor) player.Defense -= selectedItem.StatValue;

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n🧷 {selectedItem.Name} 을(를) 해제했습니다.");
                    }
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ 잘못된 입력입니다.");
                }

                Console.ResetColor();
                Console.WriteLine("\n아무 키나 누르면 계속합니다...");
                Console.ReadKey();
            }

            ShowInventory();
        }
    }
}

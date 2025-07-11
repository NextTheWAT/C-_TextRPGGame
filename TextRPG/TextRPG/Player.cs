using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Player
    {
        public int Level { get; set; } = 1;
        public int Exp { get; set; } = 0;  // ✅ 경험치 필드 추가
        public int ExpToNextLevel => Level * 100; // ✅ 다음 레벨까지 필요한 경험치
        public string Name { get; set; } = "Player";
        public string Job { get; set; } = "전사";
        public int BaseAttack { get; set; } = 10;
        public int BaseDefense { get; set; } = 10;

        public int Attack { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int HP { get; set; } = 100;
        public int Gold { get; set; } = 1000; // 플레이어의 골드

        // ✅ 장착 아이템
        public Item EquippedWeapon { get; set; }
        public Item EquippedArmor { get; set; }

        public void PlayerInfo()
        {
            while (true)
            {
                Console.Clear();
                GameSystem.PlayerInfo_Color();

                Console.WriteLine($"\n🩺 체력을 회복하려면 500 G가 필요합니다.");
                Console.WriteLine("1. 체력 회복");
                Console.WriteLine("0. 돌아가기");

                Console.Write("\n>> ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    if (HP >= 100)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n✅ 이미 체력이 가득 찼습니다!");
                        Console.ResetColor();
                    }
                    else if (Gold < 500)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n❌ 골드가 부족합니다!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Gold -= 500;
                        HP = 100;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n💉 체력을 모두 회복했습니다! (HP: 100)");
                        Console.ResetColor();
                    }

                    Console.WriteLine("\n(아무 키나 누르면 계속합니다)");
                    Console.ReadKey();
                }
                else if (input == "0")
                {
                    break;
                }
                else
                {
                    GameSystem.FaileInput();
                    Console.WriteLine("\n(아무 키나 누르면 계속합니다)");
                    Console.ReadKey();
                }
            }
        }

        public void GainExp(int amount)
        {
            Exp += amount;

            while (Exp >= ExpToNextLevel)
            {
                Level++;
                Exp = 0;
                BaseAttack += 5;
                BaseDefense += 3;

                Attack += 5;   // ✅ 실 공격력 증가
                Defense += 3;  // ✅ 실 방어력 증가

                GameSystem.ShowLevelUpAnimation(this); // 현재 플레이어 넘겨줌

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\n🎉 레벨업! Lv.{Level} → 공격력 +5, 방어력 +3"); //
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"📚 경험치 +{amount} (현재: {Exp}/{ExpToNextLevel})");
                Console.ResetColor();
                Console.ResetColor();

                         // ✅ 레벨업 후 플레이어 정보 출력
                GameSystem.PlayerInfo_Color();
            }
        }

    }
}

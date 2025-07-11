using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TextRPG;

[Serializable]
public class SaveData
{
    public string Name { get; set; }
    public string Job { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int HP { get; set; }
    public int Gold { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int BaseAttack { get; set; }
    public int BaseDefense { get; set; }


    public List<ItemData> OwnedItems { get; set; } = new List<ItemData>();
    public string EquippedWeaponName { get; set; }
    public string EquippedArmorName { get; set; }

    public static void DataUI()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("💾 저장 / 불러오기 메뉴");
        Console.ResetColor();

        for (int i = 1; i <= 5; i++)
        {
            string path = $"save{i}.json";
            if (File.Exists(path))
            {
                var data = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(path));
                Console.WriteLine($"[{i}] ✅ {data.Name} (Lv.{data.Level} {data.Job})");
            }
            else
            {
                Console.WriteLine($"[{i}] ❌ 저장 없음");
            }
        }

        Console.WriteLine("\n예시: s1 → 1번 슬롯에 저장 / l3 → 3번 슬롯에서 불러오기");
        Console.WriteLine("0. ⬅ 돌아가기");
        Console.Write("\n>> ");

        string input = Console.ReadLine();

        if (input == "0")
        {
            Program.StartGame();
            return;
        }

        if (input.Length == 2 && int.TryParse(input[1].ToString(), out int slot) && slot >= 1 && slot <= 5)
        {
            if (input.StartsWith("s"))
            {
                SavePlayer(Program.player, slot);
            }
            else if (input.StartsWith("l"))
            {
                var loaded = LoadPlayer(slot);
                if (loaded != null)
                {
                    Program.player = loaded;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("✅ 불러오기 완료!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("❌ 알 수 없는 명령어입니다.");
            }
        }
        else
        {
            Console.WriteLine("❌ 잘못된 입력입니다.");
        }

        Console.WriteLine("\n아무 키나 누르면 계속합니다.");
        Console.ReadKey();
        DataUI();
    }

    public static void SavePlayer(Player player, int slot)
    {
        var data = new SaveData
        {
            Name = player.Name,
            Job = player.Job,
            Level = player.Level,
            Exp = player.Exp,
            HP = player.HP,
            Gold = player.Gold,

            Attack = player.Attack,
            Defense = player.Attack,

            BaseAttack = player.BaseAttack,
            BaseDefense = player.BaseDefense,

            OwnedItems = Item.Items
                .Where(i => i.Price == 0)
                .Select(i => i.ToData())
                .ToList(),

            EquippedWeaponName = player.EquippedWeapon?.Name,
            EquippedArmorName = player.EquippedArmor?.Name
        };

        string json = JsonSerializer.Serialize(data);
        File.WriteAllText($"save{slot}.json", json);
        Console.WriteLine($"✅ 저장 슬롯 {slot}번에 저장 완료!");
    }


    public static Player LoadPlayer(int slot)
    {
        string path = $"save{slot}.json";
        if (!File.Exists(path))
        {
            Console.WriteLine($"❌ 저장 슬롯 {slot}번이 존재하지 않습니다.");
            return null;
        }

        string json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<SaveData>(json);

        // 1. 플레이어 생성
        var player = new Player
        {
            Name = data.Name,
            Job = data.Job,
            Level = data.Level,
            Exp = data.Exp,
            HP = data.HP,
            Gold = data.Gold,
            BaseAttack = data.BaseAttack,
            BaseDefense = data.BaseDefense,
            Attack = data.BaseAttack,
            Defense = data.BaseDefense
        };

        // 2. 모든 아이템 초기화 (미구매 상태로)
        foreach (var item in Item.Items)
        {
            item.Price = item.OriginalPrice;
            item.IsEquipped = false;
        }

        // 3. 구매한 아이템 복구
        foreach (var itemData in data.OwnedItems)
        {
            var matched = Item.Items.FirstOrDefault(i =>
                i.Name == itemData.Name && i.Type == itemData.Type);

            if (matched != null)
            {
                matched.Price = 0;
                matched.IsEquipped = itemData.IsEquipped;

                // 4. 장비가 착용된 상태라면 실제 장착 + 스탯 반영
                if (itemData.IsEquipped)
                {
                    if (matched.Type == ItemType.Weapon)
                    {
                        player.EquippedWeapon = matched;
                        player.Attack += matched.StatValue;
                    }
                    else if (matched.Type == ItemType.Armor)
                    {
                        player.EquippedArmor = matched;
                        player.Defense += matched.StatValue;
                    }
                }
            }
        }

        return player;
    }


}

using System;
using System.Collections.Generic;

namespace TextRPG
{
    public enum ItemType
    {
        Weapon,
        Armor
    }

    public class Item
    {
        public static List<Item> Items { get; private set; } = new List<Item>();

        public string Name { get; }
        public string Info { get; }
        public int OriginalPrice { get; }
        public int Price { get; set; }     // 현재 상태 (0이면 구매완료)
        public ItemType Type { get; }
        public int StatValue { get; }
        public bool IsEquipped { get; set; } = false;

        // 기본 생성자
        public Item(string name, string info, int price, ItemType type, int statValue)
        {
            Name = name;
            Info = info;
            OriginalPrice = price;
            Price = price;
            Type = type;
            StatValue = statValue;
        }

        // 저장용 ItemData로 변환
        public ItemData ToData()
        {
            return new ItemData
            {
                Name = this.Name,
                Type = this.Type,
                IsEquipped = this.IsEquipped
            };
        }
    }
}

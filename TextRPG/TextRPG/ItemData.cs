using System;
using TextRPG;

[Serializable]
public class ItemData
{
    public string Name { get; set; }
    public ItemType Type { get; set; }           // ✅ 타입 정보 추가
    public bool IsEquipped { get; set; }

    public ItemData() { }

    public ItemData(Item item)
    {
        Name = item.Name;
        Type = item.Type;                        // ✅ 타입 저장
        IsEquipped = item.IsEquipped;
    }
}

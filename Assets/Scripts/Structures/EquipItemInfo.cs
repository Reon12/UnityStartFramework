using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EquipItemInfo
{
    // 장비 아이템 코드
    public string itemCode;

    // 장비 아이템 이름
    public string itemName;

    // 장비 아이템 설명
    public string itemDescription;

    // 장비 아이템 부위 타입
    public EquipmentType equipmentType;

    // 장비 아이템 타입
    public ItemType itemType;

    // 아이템 이미지 경로
    public string itemImagePath;

    // 아이템 가격
    public int priceToSilver;
    public int priceToGold;
    public int priceToDiamond;

    public bool IsEmpty => string.IsNullOrEmpty(itemCode);

    public EquipItemInfo(string itemCode, string itemName, string itemDescription,
        EquipmentType equipmentType, ItemType itemType, string itemImagePath,
        int priceToSilver, int priceToGold, int priceToDiamond)
    {
        this.itemCode = itemCode;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemImagePath = itemImagePath;
        this.equipmentType = equipmentType;
        this.itemType = itemType;
        this.priceToSilver = priceToSilver;
        this.priceToGold = priceToGold;
        this.priceToDiamond = priceToDiamond;
    }

}
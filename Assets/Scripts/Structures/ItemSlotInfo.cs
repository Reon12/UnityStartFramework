using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemSlotInfo
{
    public string itemCode;

    public int itemCount;

    public int maxSlotCount;

    public ItemSlotInfo(string itemcode, int itemCount, int maxSlotCount)
    {
        this.itemCode = itemcode;
        this.itemCount = itemCount;
        this.maxSlotCount = maxSlotCount;
    }

    // 같은 아이템인지 확인
    public static bool operator ==(ItemSlotInfo thisItemSlotInfo, ItemSlotInfo itemSlotInfo) =>
        thisItemSlotInfo.itemCode == itemSlotInfo.itemCode;


    // 다른 아이템인지 확인
    public static bool operator !=(ItemSlotInfo thisItemSlotInfo, ItemSlotInfo itemSlotInfo) =>
        thisItemSlotInfo.itemCode != itemSlotInfo.itemCode;

    public bool isEmpty() => string.IsNullOrEmpty(itemCode);

    public void Clear()
    {
        itemCode = null;
        itemCount = maxSlotCount = 0;
    }
}
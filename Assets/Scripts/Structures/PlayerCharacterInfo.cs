﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerCharacterInfo
{
    // 인벤토리 슬롯 개수
    public int InventorySlotCount;

    // 소지중인 아이템 정보
    public List<ItemSlotInfo> inventoryItemInfos;

    public ItemInfo itemInfos;

    // 소지금 (은화)
    public int Silver;

    // 소지금 (금화)
    public int Gold;

    // 소지금 (다이아몬드)
    public int Diamond;

    public void Initialize()
    {
        InventorySlotCount = 25;

        Silver = 5000;

        Gold = 500;

        Diamond = 200;
        inventoryItemInfos = new List<ItemSlotInfo>();
        for (int i = 0; i < InventorySlotCount; ++i)
            inventoryItemInfos.Add(new ItemSlotInfo());

        inventoryItemInfos[5] = new ItemSlotInfo("90003", 5, 0);
        inventoryItemInfos[7] = new ItemSlotInfo("90002", 1, 0);
        inventoryItemInfos[0] = new ItemSlotInfo("90001", 3, 0);
        inventoryItemInfos[1] = new ItemSlotInfo("90001", 4, 0);
        inventoryItemInfos[2] = new ItemSlotInfo("90001", 5, 0);
        inventoryItemInfos[3] = new ItemSlotInfo("90001", 6, 0);
        inventoryItemInfos[4] = new ItemSlotInfo("90001", 7, 0);
        inventoryItemInfos[9] = new ItemSlotInfo("90009", 7, 0);

    }
}
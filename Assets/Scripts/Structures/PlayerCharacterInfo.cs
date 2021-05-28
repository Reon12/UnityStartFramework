using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerCharacterInfo
{
    // 인벤토리 슬롯 개수
    public int InventorySlotCount;

    // 소지금 (은화)
    public int Silver;

    // 소지금 (금화)
    public int Gold;

    // 소지금 (다이아몬드)
    public int Diamond;

    public void Initialize()
    {
        InventorySlotCount = 24;

        Silver = 5000;

        Gold = 500;

        Diamond = 200;

    }
}
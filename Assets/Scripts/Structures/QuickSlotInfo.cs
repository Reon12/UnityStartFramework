using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct QuickSlotInfo
{
    // 퀵슬롯에 연결된 슬롯을 나타냅니다.
    public SlotType linkedSlotType;

    // 인벤토리 슬롯일 경우 인벤토리 슬롯인덱스를 나타냅니다.
    public int linkedInventorySlotIndex;

    // 퀵슬롯에 들어간 아이템 코드
    public string itemCode;

    // 퀵슬롯에 들어갈 아이템 최대 갯수
    public int maxSlotCount;

    // 개수
    public int count;

    public float coolTime;

    public QuickSlotInfo(SlotType linkedSlotType, int linkedInventorySlotIndex, string itemCode, int count, int maxSlotCount, float coolTime)
    {
        this.linkedSlotType = linkedSlotType;
        this.linkedInventorySlotIndex = linkedInventorySlotIndex;
        this.itemCode = itemCode;
        this.count = count;
        this.maxSlotCount = maxSlotCount;
        this.coolTime = coolTime;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct QuickSlotInfo
{
    // 퀵슬롯에 연결된 슬롯을 나타냅니다.
    public SlotType linkedSlotType;

    // 인벤토리 슬롯일 경우 인벤토리 슬롯인덱스를 나타냅니다ㅏ.
    public int linkedInventorySlotIndex;


    public string inCode;

    // 개수
    public int count;

    public QuickSlotInfo(SlotType linkedSlotType, int linkedInventorySlotIndex, string inCode, int count)
    {
        this.linkedSlotType = linkedSlotType ;
        this.linkedInventorySlotIndex = linkedInventorySlotIndex;
        this.inCode = inCode;
        this.count = count;
        
    }
}
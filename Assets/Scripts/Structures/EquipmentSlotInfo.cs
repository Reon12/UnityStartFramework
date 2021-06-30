using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EquipmentSlotInfo
{
    // 장비 아이템 코드
    public string itemCode;

    // 장비 타입
    public EquipmentType equipmentType;


    public EquipmentSlotInfo(string itemCode, EquipmentType equipmentType)
    {
        this.itemCode = itemCode;
        this.equipmentType = equipmentType;
    }

}

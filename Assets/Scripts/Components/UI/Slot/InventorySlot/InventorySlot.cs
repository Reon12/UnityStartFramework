using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : ItemSlot
{
    // 인벤토리 아이템 슬롯 인덱스
    private int _InventoryItemSlotIndex;

    public int inventoryItemSlotIndex => _InventoryItemSlotIndex;

    protected override void InitializeSlot(SlotType slotType, string inCode)
    {
        base.InitializeSlot(slotType, inCode);

        // 슬롯 타입 설정
        m_SlotType = SlotType.InventoryItemSlot;

        // 드래그 드랍 사용 여부
        m_UseDragDrop = true;

        // 드래그 시작시 실행할 내용 정의
        onSlotDragStarted += (dragDropOperation, dragVisual) =>
        {
            // 슬롯이 비어있지 않다면 드래그 한 아이템 어둡게 표시
            if (!_ItemInfo.IsEmpty)
            {
                Color slotImageColor = new Color(0.3f, 0.3f, 0.6f);
                slotImage.color = slotImageColor;
            }

        };
        
    }
}

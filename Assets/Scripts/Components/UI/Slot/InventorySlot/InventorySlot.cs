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

            // 드래그 비쥬얼 이미지를 슬롯 이미지로 설정
            dragVisual.SetDragImageFromSprite(slotImage.sprite);

            // 드래그 취소
            dragDropOperation.onDragCancelled += () =>
            {
                Debug.Log($"[드래그 취소] 드래그 취소 슬롯 인덱스 : {inventoryItemSlotIndex}");
                slotImage.color = new Color(1.0f, 1.0f, 1.0f);
            };

            // 드래그 성공
            dragDropOperation.onDragCompleted += () =>
            {
                // 모든 겹친 UI 에 추가된 컴포넌트 확인
                foreach (var overlappedComponent in dragDropOperation.overlappedComponents)
                {
                    BaseSlot overlappedSlot = overlappedComponent as BaseSlot;

                    if (overlappedSlot == null) continue;

                    // 슬롯 타입이 인벤토리 슬롯이라면
                    if (overlappedSlot.slotType == SlotType.InventoryItemSlot)
                    {

                    }
                }

            };
        };
        
    }

    public void InitializeInventoryItemSlot(SlotType slotType, string itemCode, int itemSlotIndex)
    {
        base.InitializeSlot(slotType, itemCode);

        _InventoryItemSlotIndex = itemSlotIndex;

        UpdateItemCountText();
    }

    public void UpdateItemCountText()
    {
        var playerController = PlayerManager.Instance.playerController as GamePlayerController;
        ItemSlotInfo itemSlotInfo = playerController.playerCharacterInfo.inventoryItemInfos[_InventoryItemSlotIndex];

        SetSlotItemCount(itemSlotInfo.itemCount);
    }

    public void UpdateInventoryItemSlot()
    {
        UpdateItemImage();
        UpdateItemCountText();
    }

}

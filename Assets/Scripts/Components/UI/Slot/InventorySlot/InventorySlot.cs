﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : ItemSlot
{
    // 인벤토리 아이템 슬롯 인덱스
    private int _InventoryItemSlotIndex;

    public int inventoryItemSlotIndex => _InventoryItemSlotIndex;

    protected override void Awake()
    {
        base.Awake();
        
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
            if (_ItemInfo.IsEmpty) return;

            // 드래그 취소
            dragDropOperation.onDragCancelled += () =>
            {
                // 메시지 박스 생성
                MessageBoxWnd msgBox = (m_ScreenInstance as ScreenInstanceBase).CreateMessageBox(
                    "경고!", true);

                // 메시지 박스 ok버튼 클릭시 실행될 내용
                msgBox.onOkButtonClicked += (screenInstance, msgBoxWnd) =>
                {
                    GamePlayerController gamePlayerController = PlayerManager.Instance.playerController as GamePlayerController;
                    gamePlayerController.playerInventory.RemoveItem(inventoryItemSlotIndex);
                    msgBoxWnd.CloseThisWnd();
                };
                // 메시지 박스 Cancel버튼 클릭시 실행될 내용
                msgBox.onCancelButtonClicked += (screenInstance, msgBoxWnd) =>
                {
                    msgBoxWnd.CloseThisWnd();
                };

                slotImage.color = new Color(1.0f, 1.0f, 1.0f);
            };

            // 드래그 성공
            dragDropOperation.onDragFinished += () =>
            {
                // 모든 겹친 UI 에 추가된 컴포넌트 확인
                foreach (var overlappedComponent in dragDropOperation.overlappedComponents)
                {
                    BaseSlot overlappedSlot = overlappedComponent as BaseSlot;
                    if (overlappedSlot == null) continue;

                    InventorySlot inventorySlot = overlappedComponent as InventorySlot;

                    GamePlayerController playerController = (PlayerManager.Instance.playerController) as GamePlayerController;
                    ref PlayerCharacterInfo playerCharacterInfo = ref playerController.playerCharacterInfo;

                    // 슬롯 타입이 인벤토리 슬롯이라면
                    if (overlappedSlot.slotType == SlotType.InventoryItemSlot)
                    {
                        // 아이템 슬롯이 비어있다면 스왑 x
                        if (_ItemInfo.IsEmpty) continue;

                        slotImage.color = new Color(1.0f, 1.0f, 1.0f);

                        bool isSameItem =
                            playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex] == playerCharacterInfo.inventoryItemInfos[inventoryItemSlotIndex];
                        if (isSameItem)
                            playerController.playerInventory.MergeItem(this, inventorySlot);
                        else
                            playerController.playerInventory.SwapItem(this, inventorySlot);
                    }
                    // 퀵슬롯 일 경우
                    else if (overlappedSlot.slotType == SlotType.QuickSlot)
                    {
                        QuickSlot quickSlot = overlappedComponent as QuickSlot;

                        //playerController.playerInventory.RemoveItem(inventoryItemSlotIndex);

                        slotImage.color = new Color(1.0f, 1.0f, 1.0f);
                    }
                    else if (overlappedSlot.slotType == SlotType.EquipItemSlot)
                    {
                        EquipmentSlot equipmentSlot = overlappedComponent as EquipmentSlot;

                        slotImage.color = new Color(1.0f, 1.0f, 1.0f);

                        // 아이템 타입이 장비 아이템이 아니라면 실행 x
                        if (itemInfo.itemType != ItemType.Equipment || itemInfo.equipmentType != equipmentSlot.equipmentType) return;
                        equipmentSlot.equipmentSlotImage.sprite = Sprite.Create(m_BgIcon, new Rect(0.0f, 0.0f, m_BgIcon.width, m_BgIcon.height), Vector2.one);
                        equipmentSlot.UpdateEquipmentSlot();
                    }

                }

            };
        };
    }


    public void InitializeInventoryItemSlot(SlotType slotType, string itemCode, int itemSlotIndex)
    {
        base.InitializeSlot(slotType, itemCode, EquipmentType.None);

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

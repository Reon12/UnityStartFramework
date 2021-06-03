using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickSlot : BaseSlot
{
    [SerializeField] private TextMeshProUGUI _TMP_KeyCode;

    [SerializeField] private KeyCode _HotKey;

    private QuickSlotInfo _QuickSlotInfo;

    private ItemInfo itemInfo;

    public KeyCode hotKey { set => _HotKey = value; }

    public ref QuickSlotInfo quickSlotInfo => ref _QuickSlotInfo;

    public void InitializeQuickSlot(KeyCode hotKey, string hotkeyText)
    {
        _HotKey = hotKey;
        _TMP_KeyCode.text = hotkeyText;
    }

    protected override void Awake()
    {
        base.Awake();

        // 슬롯 타입 설정
        m_SlotType = SlotType.QuickSlot;

        // 드래그 사용
        m_UseDragDrop = true;

        // 드래그 시작시 실행될 내용
        onSlotDragStarted += (dragDropOp, dragVisual) =>
        {
            dragVisual.SetDragImageFromSprite(slotImage.sprite);
            slotImage.color = new Color(0.3f, 0.3f, 0.3f);


            dragDropOp.onDragCancelled += () =>
            slotImage.color = new Color(1.0f, 1.0f, 1.0f);
        };

        // 드래그 드랍 시 실행될 내용
        onSlotDragFinished += (dragDropOp) =>
        {
            slotImage.color = new Color(1.0f, 1.0f, 1.0f);
            if (dragDropOp.originatedComponent == this)
            {
                if (string.IsNullOrEmpty(_QuickSlotInfo.inCode)) return;
                foreach(var overlappedComponent in dragDropOp.overlappedComponents)
                {
                    BaseSlot otherSlot = overlappedComponent as BaseSlot;

                    if (otherSlot == null) continue;
                    if (otherSlot.slotType == SlotType.QuickSlot)
                    {
                        QuickSlot otherQuickSlot = dragDropOp.overlappedComponents[0] as QuickSlot;

                        SwapQuickSlot(this, otherQuickSlot);
                    }
                }
            }
            else
            {
                BaseSlot linkedSlot = dragDropOp.originatedComponent as BaseSlot;
                _QuickSlotInfo.linkedSlotType = linkedSlot.slotType;    

                if (_QuickSlotInfo.linkedSlotType == SlotType.InventoryItemSlot)
                {
                    InventorySlot inventorySlot = linkedSlot as InventorySlot;

                    _QuickSlotInfo.inCode = inventorySlot.itemInfo.itemCode;
                    _QuickSlotInfo.linkedInventorySlotIndex = inventorySlot.inventoryItemSlotIndex;
                }

                UpdateQuickSlot(linkedSlot);
            }
        };
        

    }

    // 퀵슬롯 스왑
    private void SwapQuickSlot(QuickSlot first, QuickSlot second)
    {
        Sprite firstQuickSlotSprite = first.slotImage.sprite;
        Sprite secondQuickSlotSprite = second.slotImage.sprite;

        QuickSlotInfo temp = first._QuickSlotInfo;
        first._QuickSlotInfo = second._QuickSlotInfo;
        second._QuickSlotInfo = temp;

        first.slotImage.sprite = secondQuickSlotSprite;
        second.slotImage.sprite = firstQuickSlotSprite;

        first.SetSlotItemCount(first._QuickSlotInfo.count);
        second.SetSlotItemCount(second._QuickSlotInfo.count);
    }

    // 퀵슬롯 정보 업데이트
    private void UpdateQuickSlot(BaseSlot linkedSlot)
    {
        GamePlayerController gamePlayerController = PlayerManager.Instance.playerController as GamePlayerController;

        PlayerInventory playerInventory = gamePlayerController.playerInventory;

        bool fileNotFound;

        switch (_QuickSlotInfo.linkedSlotType)
        {
            case SlotType.InventoryItemSlot:
                {
                    ItemSlotInfo itemSlotInfo = gamePlayerController.playerCharacterInfo.inventoryItemInfos[_QuickSlotInfo.linkedInventorySlotIndex];
                    ItemInfo itemInfo = ResourceManager.Instance.LoadJson<ItemInfo>(
                        "ItemInfos",
                        itemSlotInfo.itemCode + ".json", out fileNotFound);

                    _QuickSlotInfo.count = itemSlotInfo.itemCount;

                    Texture2D itemimage = ResourceManager.Instance.LoadResource<Texture2D>("", itemInfo.itemImagePath, false);

                    Rect rect = new Rect(0.0f, 0.0f, itemimage.width, itemimage.height);

                    Sprite itemSprite = Sprite.Create(itemimage, rect, Vector2.one);

                    slotImage.sprite = itemSprite;

                    SetSlotItemCount(_QuickSlotInfo.count);
                }
                break;
            case SlotType.EquipItemSlot:
                break;
            case SlotType.ShopItemSlot:
                break;
            case SlotType.QuickSlot:
                break;
            default:
                break;
        }
    }
}

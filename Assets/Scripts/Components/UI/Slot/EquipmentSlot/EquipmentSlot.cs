using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : ItemSlot
{
    private static Dictionary<EquipmentType, Texture2D> SlotHintTextures;



    protected override void Awake()
    {
        base.Awake();

        m_SlotType = SlotType.EquipItemSlot;

        m_UseDragDrop = true;


        onSlotDragStarted += (dragDropOperation, dragVisual) =>
        {

            dragVisual.SetDragImageFromSprite(slotImage.sprite);
            slotImage.color = new Color(0.3f, 0.3f, 0.3f);

            dragDropOperation.onDragFinished += () =>
            {
                foreach (var overlappedComponent in dragDropOperation.overlappedComponents)
                {
                    BaseSlot overlappedSlot = overlappedComponent as BaseSlot;
                    if (overlappedSlot == null) continue;

                    InventorySlot inventorySlot = overlappedComponent as InventorySlot;

                    GamePlayerController playerController = (PlayerManager.Instance.playerController) as GamePlayerController;
                    ref PlayerCharacterInfo playerCharacterInfo = ref playerController.playerCharacterInfo;

                    if (overlappedSlot.slotType == SlotType.InventoryItemSlot)
                    {
                        
                    }

                    slotImage.color = new Color(1.0f, 1.0f, 1.0f);
                }

            };
        };
    }

    public void InitializeEquipmentSlot(SlotType slotType, string itemCode, EquipmentType equipmentType)
    {
        base.InitializeSlot(slotType, itemCode, equipmentType);

        if (SlotHintTextures == null)
        {
            SlotHintTextures = new Dictionary<EquipmentType, Texture2D>();

            SlotHintTextures.Add(EquipmentType.Helmet, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Helmet.ToString(), "RPG_MMO_GUI/Texture/Inventory/Helmet"));
            SlotHintTextures.Add(EquipmentType.Armor, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Armor.ToString(), "RPG_MMO_GUI/Texture/Inventory/Armor"));
            SlotHintTextures.Add(EquipmentType.Leg, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Leg.ToString(), "RPG_MMO_GUI/Texture/Inventory/Leg"));
            SlotHintTextures.Add(EquipmentType.Glove, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Glove.ToString(), "RPG_MMO_GUI/Texture/Inventory/Glove"));
            SlotHintTextures.Add(EquipmentType.Shoes, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Shoes.ToString(), "RPG_MMO_GUI/Texture/Inventory/Shoes"));
            SlotHintTextures.Add(EquipmentType.Weapon, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Weapon.ToString(), "RPG_MMO_GUI/Texture/Inventory/Weapon"));
            SlotHintTextures.Add(EquipmentType.Ring1, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Ring1.ToString(), "RPG_MMO_GUI/Texture/Inventory/Ring1"));
            SlotHintTextures.Add(EquipmentType.Ring2, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Ring2.ToString(), "RPG_MMO_GUI/Texture/Inventory/Ring2"));
            SlotHintTextures.Add(EquipmentType.Jew, ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Jew.ToString(), "RPG_MMO_GUI/Texture/Inventory/Jew"));

        }


        equipmentSlotImage.sprite = Sprite.Create(SlotHintTextures[equipmentType], new Rect(0.0f, 0.0f, SlotHintTextures[equipmentType].width, SlotHintTextures[equipmentType].height), Vector2.one);

    }

    // 장비슬롯 업데이트
    public void UpdateEquipmentSlot()
    {

    }
}

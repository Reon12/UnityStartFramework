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
        };
    }

    public void InitializeEquipmentSlot(SlotType slotType, string itemCode, EquipmentType equipmentType)
    {
        base.InitializeSlot(slotType, itemCode, equipmentType);

        if (SlotHintTextures == null)
        {
            SlotHintTextures = new Dictionary<EquipmentType, Texture2D>();
        }

        // 각 슬롯의 EquipmentType 을 저장할 리스트
        List<EquipmentType> equipmentTypeToString = new List<EquipmentType>();

        var slothintValue = ResourceManager.Instance.LoadResource<Texture2D>(equipmentType.ToString(), $"RPG_MMO_GUI/Texture/Inventory/{equipmentType}");

        equipmentTypeToString.Add(equipmentType);


        SlotHintTextures.Add(equipmentType, ResourceManager.Instance.LoadResource<Texture2D>(equipmentType.ToString(), $"RPG_MMO_GUI/Texture/Inventory/{equipmentType}"));

        Debug.Log(equipmentType);

        equipmentSlotImage.sprite = Sprite.Create(SlotHintTextures[equipmentType], new Rect(0.0f, 0.0f, SlotHintTextures[equipmentType].width, SlotHintTextures[equipmentType].height), Vector2.one);



        //SlotHintTextures.Add(equipmentType, ResourceManager.Instance.LoadResource<Texture2D>(equipmentTypeTostring, $"RPG_MMO_GUI/Texture/Inventory/{equipmentTypeTostring}"));

        //SlotHintTextures.Add(EquipmentType.Helmet,  ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Helmet.ToString(), "RPG_MMO_GUI/Texture/Inventory/Helmet"));
        //SlotHintTextures.Add(EquipmentType.Armor,   ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Armor.ToString(), "RPG_MMO_GUI/Texture/Inventory/Armor"));
        //SlotHintTextures.Add(EquipmentType.Leg,     ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Leg.ToString(), "RPG_MMO_GUI/Texture/Inventory/Leg"));
        //SlotHintTextures.Add(EquipmentType.Glove,   ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Glove.ToString(), "RPG_MMO_GUI/Texture/Inventory/Glove"));
        //SlotHintTextures.Add(EquipmentType.Shoes,   ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Shoes.ToString(), "RPG_MMO_GUI/Texture/Inventory/Shoes"));
        //SlotHintTextures.Add(EquipmentType.Weapon,  ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Weapon.ToString(), "RPG_MMO_GUI/Texture/Inventory/Weapon"));
        //SlotHintTextures.Add(EquipmentType.Ring1,   ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Ring1.ToString(), "RPG_MMO_GUI/Texture/Inventory/Ring1"));
        //SlotHintTextures.Add(EquipmentType.Ring2,   ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Ring2.ToString(), "RPG_MMO_GUI/Texture/Inventory/Ring2"));
        //SlotHintTextures.Add(EquipmentType.Jew,     ResourceManager.Instance.LoadResource<Texture2D>(EquipmentType.Jew.ToString(), "RPG_MMO_GUI/Texture/Inventory/Jew"));

    }
}

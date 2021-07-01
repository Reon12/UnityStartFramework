using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : BaseSlot
{
    // 아이템 정보를 나타냅니다.
    protected ItemInfo _ItemInfo;

    public ItemInfo itemInfo => _ItemInfo;

  

    protected override void InitializeSlot(SlotType slotType, string inCode, EquipmentType equipmentType)
    {
        base.InitializeSlot(slotType, inCode, equipmentType);

        SetItemInfo(inCode);
        UpdateItemImage();
    }

    
    public void SetItemInfo(string itemCode)
    {
        // 아이템 정보가 비어있다면
        if (string.IsNullOrEmpty(itemCode))
        {
            // 아이템 정보 등록
            _ItemInfo = new ItemInfo();
            return;
            
        }
        else
        {
            // 아이템 정보를 불러옵니다.
            bool fileNotFound;
            ItemInfo itemInfo =
                ResourceManager.Instance.LoadJson<ItemInfo>("ItemInfos", $"{itemCode}.json", out fileNotFound);
            // 파일을 읽어 오지 못했다면
            if (fileNotFound)
            {
                _ItemInfo = new ItemInfo();
            }
            else
            {
                _ItemInfo = itemInfo;
            }

        }
    }

    // 아이템 이미지 갱신
    public void UpdateItemImage()
    {
        Texture2D itemImage;
        // 아이템 정보가 비어있다면 투명한 이미지로 설정합니다.
        if (_ItemInfo.IsEmpty)
            itemImage = m_T_Null;

        // 아이템 정보가 비어있지 않다면
        else
        {
            //  아이템 이미지 경로가 비어있다면 투명한 이미지 사용
            if (string.IsNullOrEmpty(_ItemInfo.itemImagePath))
                itemImage = m_T_Null;

            // 경로가 비어있지않다면 이미지를 로드합니다.
            else itemImage = ResourceManager.Instance.LoadResource<Texture2D>(
                "", _ItemInfo.itemImagePath, false);

            itemImage = itemImage ?? m_T_Null;


        }
        // 아이템 이미지 적용
        Rect rect = new Rect(0.0f, 0.0f, itemImage.width, itemImage.height);
        Vector2 pivot = Vector2.one * 0.5f;
        slotImage.sprite = Sprite.Create(itemImage, rect, pivot);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemInfo
{
    // 아이템 코드
    public string itemCode;

    // 아이템 이름
    public string itemName;

    // 아이템 설명
    public string itemDescription;

    // 아이템 타입
    public ItemType itemType;

    // 아이템 이미지 경로
    public string itemImagePath;

    // 슬롯 내에 들어갈수있는 아이템 최대 개수
    public int maxSlotItemCount;

    // 아이템 사용 쿨타임
    public float itemCoolTime;

    // 아이템 쿨타임 사용 여부
    public bool useCoolTime;

    // 아이템 판매 가격
    public int priceToSilver;
    public int priceToGold;
    public int priceToDiamond;

    public bool IsEmpty => string.IsNullOrEmpty(itemCode);

    public ItemInfo(string itemCode, string itemName, string itemDescription,
        ItemType itemType, string itemImagePath, int maxSlotItemCount, float itemCoolTime, int priceToSilver, int priceToGold, int priceToDiamond, bool useCoolTime)
    {
        this.itemCode = itemCode;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemType = itemType;
        this.itemImagePath = itemImagePath;
        this.maxSlotItemCount = maxSlotItemCount;
        this.priceToSilver = priceToSilver;
        this.priceToGold = priceToGold;
        this.priceToDiamond = priceToDiamond;
        this.itemCoolTime = itemCoolTime;
        this.useCoolTime = useCoolTime;
    }
}
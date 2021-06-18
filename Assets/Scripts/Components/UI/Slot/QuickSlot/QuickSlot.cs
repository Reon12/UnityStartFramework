﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : BaseSlot
{
    [SerializeField] private TextMeshProUGUI _TMP_KeyCode;

    [SerializeField] private KeyCode _HotKey;

    [SerializeField] private Image _SlotBackgroundImage;

    [SerializeField] private TextMeshProUGUI _Text_CoolTime;

    private QuickSlotInfo _QuickSlotInfo;

    public KeyCode hotKey { set => _HotKey = value; }

    public ref QuickSlotInfo quickSlotInfo => ref _QuickSlotInfo;

    // 시작 쿨타임
    public float currentCoolTime;

    // 쿨타임 여부
    public bool isCoolTime;

    public void InitializeQuickSlot(KeyCode hotKey, string hotkeyText)
    {
        _HotKey = hotKey;
        _TMP_KeyCode.text = hotkeyText;
    }

    protected override void Awake()
    {
        base.Awake();

        // 슬롯 쿨타임 이미지 / 텍스트 초기화
        {
            _SlotBackgroundImage.fillAmount = 0.0f;

            _Text_CoolTime.text = "";

            isCoolTime = false;
        }

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
            {
                if (_QuickSlotInfo.itemCode != null && !isCoolTime)
                {
                    MessageBoxWnd msgBox = (m_ScreenInstance as ScreenInstanceBase).CreateMessageBox(
                        "경고", true);
                    msgBox.onOkButtonClicked += (screenInstance, msgBoxWnd) =>
                    {
                        msgBoxWnd.CloseThisWnd();
                        ClearQuickSlot();
                    };
                    msgBox.onCancelButtonClicked += (screenInstance, msgBoxWnd) =>
                    {
                        msgBoxWnd.CloseThisWnd();
                    };
                }


                slotImage.color = new Color(1.0f, 1.0f, 1.0f);
            };
        };



        // 드래그 드랍 시 실행될 내용
        onSlotDragFinished += (dragDropOp) =>
        {
            slotImage.color = new Color(1.0f, 1.0f, 1.0f);
            // 퀵슬롯을 드래그 했을때
            if (dragDropOp.originatedComponent == this)
            {
                if (string.IsNullOrEmpty(_QuickSlotInfo.itemCode)) return;
                foreach (var overlappedComponent in dragDropOp.overlappedComponents)
                {
                    BaseSlot otherSlot = overlappedComponent as BaseSlot;

                    if (otherSlot == null) continue;
                    if (otherSlot.slotType == SlotType.QuickSlot)
                    {
                        QuickSlot otherQuickSlot = dragDropOp.overlappedComponents[0] as QuickSlot;

                        if (!isCoolTime)
                        {
                            // 두 슬롯의 아이템 코드가 동일하면 합치고 아니면 스왑시킵니다.
                            if (_QuickSlotInfo.itemCode == otherQuickSlot._QuickSlotInfo.itemCode)
                                MergeQuickSlot(this, otherQuickSlot);
                            else SwapQuickSlot(this, otherQuickSlot);
                        }
                    }
                    else if (otherSlot.slotType == SlotType.InventoryItemSlot)
                    {
                        
                        ref PlayerCharacterInfo playerCharacterInfo = ref (PlayerManager.Instance.playerController as GamePlayerController).playerCharacterInfo;
                        InventorySlot inventorySlot = overlappedComponent as InventorySlot;

                       
                        ItemSlotInfo itemSlotInfo = playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex];
                        itemSlotInfo.itemCount = _QuickSlotInfo.count;
                        itemSlotInfo.itemCode = _QuickSlotInfo.itemCode;
                        
                        if (inventorySlot.itemInfo.itemCode == null)
                        {

                            playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex] = itemSlotInfo;
                            inventorySlot.InitializeInventoryItemSlot(otherSlot.slotType, itemSlotInfo.itemCode, inventorySlot.inventoryItemSlotIndex);
                            ClearQuickSlot();
                        }
                        else
                        {
                            // 여기서 스왑해주기
                            playerCharacterInfo.inventoryItemInfos[inventorySlot.inventoryItemSlotIndex] = itemSlotInfo;
                            inventorySlot.InitializeInventoryItemSlot(otherSlot.slotType, _QuickSlotInfo.itemCode, inventorySlot.inventoryItemSlotIndex);
                        }

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
                    GamePlayerController playerController = (PlayerManager.Instance.playerController) as GamePlayerController;

                    if (inventorySlot.itemInfo.itemType != ItemType.Consumption) return;
                    else
                    {
                        _QuickSlotInfo.itemCode = inventorySlot.itemInfo.itemCode;
                        _QuickSlotInfo.linkedInventorySlotIndex = inventorySlot.inventoryItemSlotIndex;
                        _QuickSlotInfo.maxSlotCount = inventorySlot.itemInfo.maxSlotItemCount;
                    }
                }
                UpdateQuickSlot(linkedSlot);
            }
        };


    }

    public void Update()
    {
        InputKey();
    }

    // 키 입력 확인
    private void InputKey()
    {
        if (_QuickSlotInfo.itemCode == null) return;

        if (isCoolTime) return;

        // 퀵슬롯 키가 눌렸을 경우 1 ~ 5
        if (Input.GetKeyDown(_HotKey))
        {
            // 아이템 개수 1 감소
            SetSlotItemCount(--_QuickSlotInfo.count);
            if (_QuickSlotInfo.count != 0)
                StartCoroutine(CoolTime());
            else
            {
                ClearQuickSlot();
                ClearCoolTime();
            }

        }
    }

    // 쿨타임 코루틴 정의
    IEnumerator CoolTime()
    {
        currentCoolTime = _QuickSlotInfo.coolTime;

        while (currentCoolTime >= 0.0f)
        {
            currentCoolTime -= Time.deltaTime;

            _SlotBackgroundImage.fillAmount = currentCoolTime / _QuickSlotInfo.coolTime;

            string resultCoolTime = currentCoolTime.ToString("N1");
            if (resultCoolTime == "10.0")
                resultCoolTime = "10";

            SetCoolTimeText(resultCoolTime);

            isCoolTime = true;
            yield return new WaitForFixedUpdate();
        }
        if (currentCoolTime <= 0)
            ClearCoolTime();
    }

    // 스킬 쿨타임 텍스트 설정
    private void SetCoolTimeText(string text)
    {
        _Text_CoolTime.text = text;
    }

    // 쿨타임 텍스트 지우기 / 쿨타임 사용 비활성화
    private void ClearCoolTime()
    {
        _Text_CoolTime.text = "";
        isCoolTime = false;
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

    // 퀵슬롯 아이템 합치기
    private void MergeQuickSlot(QuickSlot ori, QuickSlot target)
    {
        // 합치려는 슬롯중 하나라도 최대아이템개수라면 아이템 스왑
        if (ori._QuickSlotInfo.count == ori._QuickSlotInfo.maxSlotCount || target._QuickSlotInfo.count == target._QuickSlotInfo.maxSlotCount)
            SwapQuickSlot(ori, target);
        else
        {
            int temp = target._QuickSlotInfo.maxSlotCount - target._QuickSlotInfo.count;
            if (temp > ori._QuickSlotInfo.count)
                temp = ori._QuickSlotInfo.count;

            ori._QuickSlotInfo.count -= temp;
            target._QuickSlotInfo.count += temp;
            ori.SetSlotItemCount(ori._QuickSlotInfo.count);
            target.SetSlotItemCount(target._QuickSlotInfo.count);

            if (ori._QuickSlotInfo.count == 0)
                ClearQuickSlot();
        }

    }

    // 퀵슬롯 비우기
    public void ClearQuickSlot()
    {
        ref QuickSlotInfo quickSlotInfo = ref _QuickSlotInfo;

        Vector2 position = new Vector2(0.0f, 0.0f);
        Vector2 size = new Vector2(42.0f, 42.0f);
        Rect rect = new Rect(position, size);
        slotImage.sprite = Sprite.Create(m_T_Null, rect, Vector2.zero);
        _QuickSlotInfo.itemCode = null;
        _QuickSlotInfo.linkedInventorySlotIndex = 0;
        _QuickSlotInfo.count = 0;
        SetSlotItemCount(_QuickSlotInfo.count);
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
                    // 퀵슬롯 정보 저장
                    ItemSlotInfo itemSlotInfo = gamePlayerController.playerCharacterInfo.inventoryItemInfos[_QuickSlotInfo.linkedInventorySlotIndex];
                    ItemInfo itemInfo = ResourceManager.Instance.LoadJson<ItemInfo>(
                        "ItemInfos",
                        itemSlotInfo.itemCode + ".json", out fileNotFound);

                    _QuickSlotInfo.count = itemSlotInfo.itemCount;

                    _QuickSlotInfo.coolTime = itemInfo.itemCoolTime;


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
                {
                }
                break;
            default:
                break;
        }
    }


}

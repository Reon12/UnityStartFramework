using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventoryWnd : ClosableWnd
{
    [SerializeField] private RectTransform _Panel_ItemSlots;
    [SerializeField] private TextMeshProUGUI _TMP_Silver;
    [SerializeField] private TextMeshProUGUI _TMP_Gold;
    [SerializeField] private TextMeshProUGUI _TMP_Diamond;

    private InventorySlot _Panel_InventorySlotPrefab;

    // 추가된 인벤토리 아이템 슬롯들을 저장할 리스트
    private List<InventorySlot> _ItemSlots = new List<InventorySlot>();

    public List<InventorySlot> itemSlot => _ItemSlots;
    protected override void Awake()
    {
        base.Awake();

        _Panel_InventorySlotPrefab = ResourceManager.Instance.LoadResource<GameObject>("Panel_InventoryItemSlot",
            "Prefabs/UI/Slot/Panel_InventoryItemSlot", true).GetComponent<InventorySlot>();
    }

    // 인벤토리 초기화
    private void InitializeInventoryWnd()
    {
        PlayerController playerController = (PlayerManager.Instance.playerController as PlayerController);
        ref PlayerCharacterInfo playerCharacterInfo =
            ref playerController.playerCharacterInfo;

        for (int i = 0; i < playerCharacterInfo.InventorySlotCount; ++i)
        {
            var newitemslot = CreateSlot();

            _ItemSlots.Add(newitemslot);

            newitemslot.
        }
    }

    // 인벤토리 슬롯 생성
    private InventorySlot CreateSlot() =>
        Instantiate(_Panel_InventorySlotPrefab, _Panel_ItemSlots);
}

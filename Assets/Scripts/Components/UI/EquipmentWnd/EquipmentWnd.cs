using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentWnd : ClosableWnd
{
    [SerializeField] RectTransform _Panel_LeftItemSlot;
    [SerializeField] RectTransform _Panel_RightItemSlot;
    [SerializeField] TextMeshProUGUI _TMP_Atk;
    [SerializeField] TextMeshProUGUI _TMP_Def;
    [SerializeField] TextMeshProUGUI _TMP_Hp;
    [SerializeField] TextMeshProUGUI _TMP_Mp;
    [SerializeField] TextMeshProUGUI _TMP_Critical;

    private EquipmentSlot _Panel_EquipmentSlotPrefab;

    private List<EquipmentSlot> _LeftItemSlots = new List<EquipmentSlot>();
    private List<EquipmentSlot> _RightItemSlots = new List<EquipmentSlot>();

    public List<EquipmentSlot> leftItemSlots => _LeftItemSlots;
    public List<EquipmentSlot> rightItemSlots => _RightItemSlots;

    protected override void Awake()
    {
        base.Awake();

        _Panel_EquipmentSlotPrefab = ResourceManager.Instance.LoadResource<GameObject>("Panel_EquipmentSlot",
            "Prefabs/UI/Slot/Panel_EquipmentSlot").GetComponent<EquipmentSlot>();

        InitializeEquipmentWnd();

        InitializeStatus();
    }


    private void InitializeEquipmentWnd()
    {
        PlayerController playerController = PlayerManager.Instance.playerController as PlayerController;
        ref PlayerCharacterInfo playerCharacterInfo = ref playerController.playerCharacterInfo;

        for (int i = 0; i< playerCharacterInfo.LeftEquipmentSlotCount; ++i)
        {
            var newLeftItemSlots = CreateLeftSlot();

            _LeftItemSlots.Add(newLeftItemSlots);

            newLeftItemSlots.InitializeEquipmentSlot(SlotType.EquipItemSlot, playerCharacterInfo.LeftequipmentItemInfos[i].itemCode, EquipmentType.Helmet + i);
        }
        for (int i = 0; i < playerCharacterInfo.RightEquipmentSlotCount; ++i)
        {
            var newRightItemSlots = CreateRightSlot();

            _RightItemSlots.Add(newRightItemSlots);

            newRightItemSlots.InitializeEquipmentSlot(SlotType.EquipItemSlot, playerCharacterInfo.RightequipmentItemInfos[i].itemCode, EquipmentType.Weapon + i);

        }
    }

    public void InitializeStatus()
    {
        GamePlayerController gamePlayerController = PlayerManager.Instance.playerController as GamePlayerController;

        _TMP_Atk.text = gamePlayerController.playerCharacterInfo.atk.ToString();
        _TMP_Def.text = gamePlayerController.playerCharacterInfo.def.ToString();
        _TMP_Hp.text = gamePlayerController.playerCharacterInfo.hp.ToString();
        _TMP_Mp.text = gamePlayerController.playerCharacterInfo.mp.ToString();
        _TMP_Critical.text = gamePlayerController.playerCharacterInfo.critical.ToString();
    }

    private EquipmentSlot CreateLeftSlot() =>
        Instantiate(_Panel_EquipmentSlotPrefab, _Panel_LeftItemSlot);

    private EquipmentSlot CreateRightSlot() =>
        Instantiate(_Panel_EquipmentSlotPrefab, _Panel_RightItemSlot);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

public class GamePlayerController : PlayerController
{
    private QuickSlotPanel _HUD_QuickSlotPanelPrefab;

    private QuickSlotPanel _QuickSlotPanel;

    private PlayerInventory _PlayerInventory;

    public PlayerInventory playerInventory => _PlayerInventory;

    private PlayerEquipment _PlayerEquipment;

    public PlayerEquipment playerEquipment => _PlayerEquipment;

    protected override void Awake()
    {
        base.Awake();

        if (!_HUD_QuickSlotPanelPrefab)
        {
            _HUD_QuickSlotPanelPrefab = ResourceManager.Instance.LoadResource<GameObject>(
                "HUD_QuickSlotPanel", "Prefabs/UI/HUD/HUD_QuickSlotPanel").GetComponent<QuickSlotPanel>();
        }
        _PlayerInventory = GetComponent<PlayerInventory>();

        _PlayerEquipment = GetComponent<PlayerEquipment>();

    }

    private void Start()
    {
        _QuickSlotPanel = screenInstance.CreateChildHUD(
            _HUD_QuickSlotPanelPrefab, InputMode.GameOnly, false, true, 500.0f, 105.0f);

        
    }
    private void Update()
    {
        if (InputManager.GetAction("OpenInventory", ActionEvent.Down))
        {
            _PlayerInventory.ToggleInventory();
        }
        if (InputManager.GetAction("OpenEquipment", ActionEvent.Down))
        {
            _PlayerEquipment.ToggleEquipmentWnd();
        }
    }
}

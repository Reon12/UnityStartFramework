using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _HUD_QuickSlotPanel;

    private int _QuickSlotCount = 5;


    QuickSlot _Panel_QuickSlotPrefab;

    public RectTransform rectTransform => transform as RectTransform;

    private void Awake()
    {
        if (!_Panel_QuickSlotPrefab)
            _Panel_QuickSlotPrefab = ResourceManager.Instance.LoadResource<GameObject>(
                "Panel_QuickSlot", "Prefabs/UI/Slot/Panel_QuickSlot").GetComponent<QuickSlot>();

        CreateQuickSlot();
    }

    private void CreateQuickSlot()
    {
        for (int i = 0; i < _QuickSlotCount; ++i)
        {
            QuickSlot newQuickSlot = Instantiate(_Panel_QuickSlotPrefab, _HUD_QuickSlotPanel);
            newQuickSlot.InitializeQuickSlot((KeyCode)((int)KeyCode.Alpha1 + i), (i + 1).ToString());

        }

    }
}

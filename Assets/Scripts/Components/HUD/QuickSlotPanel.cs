using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotPanel : MonoBehaviour
{
    private int _QuickSlotCount = 5;

    QuickSlot _Panel_QuickSlotPrefab;

    public RectTransform rectTransform => transform as RectTransform;

    private void Awake()
    {
        if (!_Panel_QuickSlotPrefab)
            _Panel_QuickSlotPrefab = ResourceManager.Instance.LoadResource<GameObject>(
                "Panel_QuickSlot", "Prefabs/UI/Slot/Panel_QuickSlot").GetComponent<QuickSlot>();
    }
}

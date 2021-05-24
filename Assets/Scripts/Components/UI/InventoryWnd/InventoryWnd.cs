using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryWnd : ClosableWnd
{
    [SerializeField] private RectTransform _Panel_ItemSlots;


    protected override void Awake()
    {
        base.Awake();

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    protected override void Awake()
    {
        base.Awake();

        m_SlotType = SlotType.EquipItemSlot;

        m_UseDragDrop = true;

        onSlotDragStarted += (dragDropOperation, dragVisual) =>
        {

        };
    }

    public void InitializeEquipmentSlot(SlotType slotType, string itemCode)
    {
        base.InitializeSlot(slotType, itemCode);

    }
}

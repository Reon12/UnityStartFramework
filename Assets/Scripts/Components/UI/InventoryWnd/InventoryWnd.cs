using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventoryWnd : ClosableWnd
{

    protected override void Awake()
    {
        base.Awake();
        SetTitleText("인벤토리");
    }

    
    // 인벤토리 창 생성

}

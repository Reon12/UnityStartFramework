using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    private InventoryWnd _Wnd_PlayerInventoryPrefab;

    public InventoryWnd playerInventoryWnd { get; private set; }

    public void Awake()
    {
        _Wnd_PlayerInventoryPrefab = ResourceManager.Instance.LoadResource<GameObject>("Wnd_PlayerInventoryPrefab",
            "Prefabs/UI/InventoryWnd/InventoryWnd").GetComponent<InventoryWnd>();
    }

    public void ToggleInventory()
    {
        if (playerInventoryWnd == null) OpenInventoryWnd();
        else CloseInventoryWnd();
    }

    public void OpenInventoryWnd(bool usePrevPosition = true)
    {
        if (playerInventoryWnd != null) return;

        // 인벤토리 창 생성
        playerInventoryWnd = PlayerManager.Instance.playerController.
            screenInstance.CreateWnd(_Wnd_PlayerInventoryPrefab, usePrevPosition) as InventoryWnd;

        playerInventoryWnd.onWndClosedEvent += () => playerInventoryWnd = null;
    }

    public void CloseInventoryWnd()
    {
        if (playerInventoryWnd == null) return;

        PlayerManager.Instance.playerController.
            screenInstance.CloseWnd(false, playerInventoryWnd);
    }
}

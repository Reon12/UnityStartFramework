using UnityEngine;

public sealed class PlayerEquipment : MonoBehaviour
{
    private EquipmentWnd _Wnd_EquipmentWndPrefab;

    public EquipmentWnd playerequipmentWnd { get; private set; }

    private void Awake()
    {
        _Wnd_EquipmentWndPrefab = ResourceManager.Instance.LoadResource<GameObject>("Wnd_EquipmentPrefab",
            "Prefabs/UI/EquipmentWnd/EquipmentWnd").GetComponent<EquipmentWnd>();
    }

    public void ToggleEquipmentWnd()
    {
        if (playerequipmentWnd == null)
            OpenEquipmentWnd();
        else CloseEquipmentWnd();

    }

    public void OpenEquipmentWnd(bool usePrevPosition = true)
    {
        if (playerequipmentWnd != null) return;
        playerequipmentWnd = PlayerManager.Instance.playerController.screenInstance.
            CreateWnd(_Wnd_EquipmentWndPrefab, usePrevPosition) as EquipmentWnd;

        playerequipmentWnd.onWndClosedEvent += () => playerequipmentWnd = null;
    }

    public void CloseEquipmentWnd()
    {
        if (playerequipmentWnd == null) return;

        PlayerManager.Instance.playerController.screenInstance.CloseWnd(false, playerequipmentWnd);
    }
}

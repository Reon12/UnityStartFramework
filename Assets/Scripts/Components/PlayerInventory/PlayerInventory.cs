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

    // 아이템 스왑
    public void SwapItem(InventorySlot first, InventorySlot second)
    {
        ref PlayerCharacterInfo playerCharacterInfo =
            ref (PlayerManager.Instance.playerController as GamePlayerController).playerCharacterInfo;

        // 소지 아이템 정보 변경
        var tempItemInfo = playerCharacterInfo.inventoryItemInfos[first.inventoryItemSlotIndex];
        playerCharacterInfo.inventoryItemInfos[first.inventoryItemSlotIndex] = playerCharacterInfo.inventoryItemInfos[second.inventoryItemSlotIndex];
        playerCharacterInfo.inventoryItemInfos[second.inventoryItemSlotIndex] = tempItemInfo;

        // 슬롯 아이템 정보 변경
        first.SetItemInfo(playerCharacterInfo.inventoryItemInfos[first.inventoryItemSlotIndex].itemCode);
        second.SetItemInfo(playerCharacterInfo.inventoryItemInfos[second.inventoryItemSlotIndex].itemCode);

        // 슬롯 갱신
        first.UpdateInventoryItemSlot();
        second.UpdateInventoryItemSlot();
    }

    // 아이템 합치기
    public void MergeItem(InventorySlot ori, InventorySlot target)
    {
        GamePlayerController gamePlayerController = PlayerManager.Instance.playerController as GamePlayerController;
        ref PlayerCharacterInfo playerCharacterInfo = ref gamePlayerController.playerCharacterInfo;

        // 드래그가 시작되는 아이템 슬롯 정보
        ItemSlotInfo oriItemSlotInfo = playerCharacterInfo.inventoryItemInfos[ori.inventoryItemSlotIndex];
        // 드래그 드랍이 되었을때 위치한 아이템 슬롯 정보
        ItemSlotInfo targetItemSlotInfo = playerCharacterInfo.inventoryItemInfos[target.inventoryItemSlotIndex];

        int maxSlotCount = ori.itemInfo.maxSlotItemCount;
        // 둘중 하나라도 최대 개수라면 스왑
        if (oriItemSlotInfo.itemCount == maxSlotCount  || targetItemSlotInfo.itemCount == maxSlotCount) SwapItem(ori, target);
        else
        {
            int addable = maxSlotCount - targetItemSlotInfo.itemCount;
            // 옮기려는 아이템의 개수가 보유 개수보다 크다면
            if (addable > oriItemSlotInfo.itemCount)
                // 보유 개수를 옮기려는 아이템으로 설정합니다.
                addable = oriItemSlotInfo.itemCount;

            // 아이템을 옮깁니다.
            oriItemSlotInfo.itemCount -= addable;
            targetItemSlotInfo.itemCount += addable;

            // 아이템을 옮긴후 옮겨진 슬롯의 개수가 0 이라면
            if (oriItemSlotInfo.itemCount == 0)
            {
                // 슬롯 정보를 비웁니다.
                oriItemSlotInfo.Clear();
                ori.SetItemInfo(oriItemSlotInfo.itemCode);
            }

            playerCharacterInfo.inventoryItemInfos[ori.inventoryItemSlotIndex] = oriItemSlotInfo;
            playerCharacterInfo.inventoryItemInfos[target.inventoryItemSlotIndex] = targetItemSlotInfo;

            ori.UpdateInventoryItemSlot();
            target.UpdateInventoryItemSlot();
        }
    }

    
}

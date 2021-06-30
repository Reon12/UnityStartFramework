using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoEditor : MonoBehaviour
{
    [SerializeField] private List<ItemInfo> _ItemInfos = new List<ItemInfo>();
    [SerializeField] private List<ItemInfo> _EquipItemInfos = new List<ItemInfo>();

    private void OnDestroy()
    {
        foreach (var iteminfo in _ItemInfos)
        {
            ResourceManager.Instance.SaveJson<ItemInfo>(iteminfo, "ItemInfos", $"{iteminfo.itemCode}.json");
        }
        foreach (var equipItemInfo in _EquipItemInfos)
        {
            ResourceManager.Instance.SaveJson<ItemInfo>(equipItemInfo, "ItemInfos", $"{equipItemInfo.itemCode}.json");
        }
    }

}

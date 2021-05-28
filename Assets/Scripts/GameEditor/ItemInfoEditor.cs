using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoEditor : MonoBehaviour
{
    [SerializeField] private List<ItemInfo> _ItemInfos = new List<ItemInfo>();


    private void OnDestroy()
    {
        foreach (var iteminfo in _ItemInfos)
        {
            ResourceManager.Instance.SaveJson<ItemInfo>(iteminfo, "ItemInfos", $"{iteminfo.itemCode}.json");
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EquipItemEditor : MonoBehaviour
{
    [SerializeField] private List<EquipItemInfo> _EquipitemInfos = new List<EquipItemInfo>();

    private void OnDestroy()
    {
        foreach (var equipItemInfo in _EquipitemInfos)
        {
            ResourceManager.Instance.SaveJson<EquipItemInfo>(equipItemInfo, "equipItemInfos", $"{equipItemInfo.itemCode}.json");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerController : PlayerController
{
    private PlayerInventory _PlayerInventory;

    public PlayerInventory playerInventory => _PlayerInventory;

    protected override void Awake()
    {
        base.Awake();

        _PlayerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (InputManager.GetAction("OpenInventory", UnityStartUpFramework.Enums.ActionEvent.Down))
        {
            _PlayerInventory.ToggleInventory();
        }
    }
}

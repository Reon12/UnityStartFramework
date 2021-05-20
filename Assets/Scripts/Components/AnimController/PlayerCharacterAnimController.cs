using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

public class PlayerCharacterAnimController : AnimController
{
    private PlayerCharacterBase _PlayerableCharacter;

    private void Awake()
    {
        _PlayerableCharacter = GetComponent<PlayerCharacterBase>();
    }

    private void Update()
    {
        if (!controlledAnimator) return;

        SetParam("_VelocityLength", _PlayerableCharacter.movement.moveXZVelocity.magnitude);
        SetParam("_IsInAir", !_PlayerableCharacter.movement.isGrounded);
        SetParam("_UseWeapon", _PlayerableCharacter.movement.useWeapon);
    }
    
}

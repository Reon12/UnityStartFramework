using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCharacterMovement))]

public class PlayerCharacterBase : PlayerableCharacterBase
{
    [SerializeField] private SpringArm _SpringArm;
    public CharacterController characterController { get; private set; }
    public PlayerCharacterMovement movement { get; private set; }

    public PlayerSkillController skillController { get; private set; }
    public SpringArm springArm => _SpringArm;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        movement = GetComponent<PlayerCharacterMovement>();

        idCollider = characterController;
    }
    protected override void Update()
    {
        base.Update();
        playerController.AddYawAngle(InputManager.GetAxis("Mouse X"));
        playerController.AddPitchAngle(-InputManager.GetAxis("Mouse Y"));
        springArm.ZoomCamera(-InputManager.GetAxis("Mouse ScrollWheel"));

    }
}

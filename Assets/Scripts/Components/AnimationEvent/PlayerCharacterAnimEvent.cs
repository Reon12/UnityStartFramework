using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerCharacterAnimEvent : MonoBehaviour
{
    private PlayerCharacterBase _PlayerCharacter;

    private void Start()
    {
        _PlayerCharacter = PlayerManager.Instance.playerController.playerableCharacter as PlayerCharacterBase;
    }

    private void AnimEvent_SkillFinished() =>
        _PlayerCharacter.skillController.FinishedSkill();

    private void AnimEvent_SetSkillRequestable() =>
        _PlayerCharacter.skillController.isRequestable = true;

    private void AnimEvent_BlockSkillRequestable() =>
        _PlayerCharacter.skillController.isRequestable = false;

    private void AnimEvent_SkillMoveStarted() =>
        _PlayerCharacter.skillController.blockMovement = false;

    private void AnimEvent_SkillMoveFinished() =>
        _PlayerCharacter.skillController.blockMovement = true;
}

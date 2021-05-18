
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // 참조할 플레이어 객체
    PlayerableCharacterBase _PlayerableCharacter;

    // 처음 사용한 스킬 
    SkillInfo? _CurrentSkillInfo;

    // 이전에 사용한 스킬
    SkillInfo? _PrevSkillInfo;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerableCharacterBase _PlayerableCharacter;

    // 현재 실행중인 스킬 정보를 나타냅니다.
    private SkillInfo? _CurrentSkillInfo;

    // 바로 이전에 실행시킨 스킬 정보를 나타냅니다.
    private SkillInfo? _PrevSkillInfo;

    // 실행시킬 스킬 정보를 담아둘 큐
    private Queue<SkillInfo> _SkillQueue = new Queue<SkillInfo>();

    // 스킬 큐 최대 요소 개수
    private int _MaxQueueCount = 1;

    private void Awake()
    {
        _PlayerableCharacter = GetComponent<PlayerableCharacterBase>();
    }

    private void SkillProcedure()
    {
        // 현재 실행중인 스킬이 존재한다면 return
        if (_CurrentSkillInfo != null) return;

        // Queue 에 요소가 없다면 실행하지 않습니다.
        if (_SkillQueue.Count == 0) return;

        // 다음에 실행시킬 스킬 정보를 얻습니다.
        SkillInfo skillInfo = _SkillQueue.Dequeue();

        
    }

    // 스킬 상태 정보를 갱신합니다.

    // 스킬을 시전합니다.

    // 스킬 실행을 요청합니다.

    // 스킬이 끝났음을 알립니다.
}

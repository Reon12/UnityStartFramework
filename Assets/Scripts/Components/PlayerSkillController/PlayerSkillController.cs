using System.Collections;
using System.Collections.Generic;
using UnityStartUpFramework.Enums;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{

    private PlayerCharacterBase _PlayerableCharacter;

    // 현재 실행중인 스킬 정보를 나타냅니다.
    private SkillInfo? _CurrentSkillInfo;

    // 바로 이전에 실행시킨 스킬 정보를 나타냅니다.
    private SkillInfo? _PrevSkillInfo;

    // 실행시킬 스킬 정보를 담아둘 큐
    private Queue<SkillInfo> _SkillQueue = new Queue<SkillInfo>();

    // 스킬 큐 최대 요소 개수
    private int _MaxQueueCount = 2;

    private Dictionary<string, SkillProgressInfo> _UsedSkillInfo = new Dictionary<string, SkillProgressInfo>();

    public bool blockMovement { get; set; } = false;

    public bool isRequestable { get; set; } = true;
    private void Awake()
    {
        _PlayerableCharacter = GetComponent<PlayerCharacterBase>();
    }

    private void Update()
    {
        SkillProcedure();
    }
    private void SkillProcedure()
    {
        // 현재 실행중인 스킬이 존재한다면 return
        if (_CurrentSkillInfo != null) return;

        // Queue 에 요소가 없다면 실행하지 않습니다.
        if (_SkillQueue.Count == 0) return;

        // 다음에 실행시킬 스킬 정보를 얻습니다.
        SkillInfo skillInfo = _SkillQueue.Dequeue();

        UpdateSkillInfo(skillInfo);

        CastSkill(skillInfo);

    }

    // 스킬 상태 정보를 갱신합니다.
    private void UpdateSkillInfo(SkillInfo newskillinfo)
    {
        if (_PrevSkillInfo != null)
        {
            // 사용하던 스킬이 전에 사용한 스킬과 코드가 동일하지 않다면
            if (_PrevSkillInfo.Value.skillCode != newskillinfo.skillCode)
            {
                SkillProgressInfo skillProgressInfo = _UsedSkillInfo[_PrevSkillInfo.Value.skillCode];

                skillProgressInfo.skillCombo = -1;

                _UsedSkillInfo[_PrevSkillInfo.Value.skillCode] = skillProgressInfo;
            }
        }

        if (_UsedSkillInfo.ContainsKey(newskillinfo.skillCode))
        {

            // 콤보를 사용하는 스킬이라면
            if (newskillinfo.maxComboCount != 0)
            {
                SkillProgressInfo progressInfo = _UsedSkillInfo[newskillinfo.skillCode];

                // 콤보 카운트 증가
                progressInfo.AddCombo();

                if (progressInfo.skillCombo == newskillinfo.maxComboCount)
                    // 스킬 콤보 초기화
                    progressInfo.ResetCombo();

                _UsedSkillInfo[newskillinfo.skillCode] = progressInfo;
            }
        }
        // 스킬을 처음 사용한다면
        else
        {
            SkillProgressInfo newSkillProgressInfo = new SkillProgressInfo(newskillinfo.skillCode,  0);
            _UsedSkillInfo.Add(newskillinfo.skillCode, newSkillProgressInfo);
        }
    }
    // 스킬을 시전합니다.
    private void CastSkill(SkillInfo skillInfo)
    {
        // 현재 실행중인 스킬 정보를 저장합니다.
        _CurrentSkillInfo = skillInfo;

        // 시전시킬 스킬이 이동을 제한하는지 확인합니다.
        blockMovement = !_CurrentSkillInfo.Value.moveableInSkillCastTime;


        // 스킬 실행 후 스킬 요청이 이루어질수 없게 합니다.
        isRequestable = false;


        // 전에 사용한 스킬이 존재한다면
        if (_PrevSkillInfo != null)
        {
            // 같은 스킬을 연계 공격으로 사용한다면
            if ((_PrevSkillInfo.Value.skillCode == _CurrentSkillInfo.Value.skillCode) &&
                _CurrentSkillInfo.Value.maxComboCount != 0)
            {
                // 애니메이션 클립 이름을 얻습니다.
                int currentComboCount = _UsedSkillInfo[_CurrentSkillInfo.Value.skillCode].skillCombo;
                string animClip = _CurrentSkillInfo.Value.LinkableSkillanimationName[currentComboCount];

                // 애니메이션을 재생합니다.
                _PlayerableCharacter.animController.controlledAnimator?.Play(animClip);

                return;
            }
        }
        _PlayerableCharacter.animController.controlledAnimator?.Play(_CurrentSkillInfo.Value.LinkableSkillanimationName[0]);
    }
    // 스킬 실행을 요청합니다.
    public void RequestSkill(string skillCode)
    {
        // 스킬을 요청할 수 없는 상태라면 실행하지 않습니다.
        if (!isRequestable) return;

        // 스킬이 _MaxQueue 개 이상 등록되었다면 추가하지 않습니다.
        if (_SkillQueue.Count >= _MaxQueueCount) return;

        bool fileNotFound;

        // 요청한 스킬 정보를 얻습니다.
        SkillInfo requestSkillInfo =
            ResourceManager.Instance.LoadJson<SkillInfo>("SkillInfos", $"{skillCode}.json", out fileNotFound);

        if (fileNotFound)
        {
            Debug.LogError($"SkillCode = {skillCode} is Not Found!");
            return;
        }

        if (!requestSkillInfo.castableInAir && !_PlayerableCharacter.movement.isGrounded) return;


        _SkillQueue.Enqueue(requestSkillInfo);


    }
    // 스킬이 끝났음을 알립니다.
    public void FinishedSkill()
    {
        // 처음 사용한 스킬을 저장합니다.
        _PrevSkillInfo = _CurrentSkillInfo;
        _CurrentSkillInfo = null;

        // 스킬 요청 가능상태로 변경합니다.
        isRequestable = true;

        // 이동 제한을 해제합니다.
        blockMovement = false;


        _PlayerableCharacter.animController.controlledAnimator?.CrossFade("BT_MoveGround", 0.25f);
        // 사용할 스킬이 존재하지 않다면
        if (_SkillQueue.Count == 0)
        {

            // 사용했던 스킬 콤보를 연계 시작 가능한 상태로 설정합니다.
            SkillProgressInfo skillProgressInfo = _UsedSkillInfo[_PrevSkillInfo.Value.skillCode];
            skillProgressInfo.skillCombo = -1;
            _UsedSkillInfo[_PrevSkillInfo.Value.skillCode] = skillProgressInfo;
        }
    }

    // 스킬 범위 인덱스를 다음 인덱스로 설정합니다.
    public void NextSkillRangeIndex()
    {
        // 현재 실행중인 스킬이 존재하지 않는다면 실행하지 않습니다.
        if (_CurrentSkillInfo == null) return;

        // 스킬 범위 인덱스 설정
        SkillProgressInfo skillProgressInfo = _UsedSkillInfo[_CurrentSkillInfo.Value.skillCode];
        ++skillProgressInfo.currentSkillIndex;

        // 만약 인덱스가 배열 범위를 초과한다면 마지막 요소 인덱스로 설정합니다.
        if (skillProgressInfo.currentSkillIndex == _CurrentSkillInfo.Value.skillRangeInfos.Length)
            --skillProgressInfo.currentSkillIndex;

        _UsedSkillInfo[_CurrentSkillInfo.Value.skillCode] = skillProgressInfo;
    }
}

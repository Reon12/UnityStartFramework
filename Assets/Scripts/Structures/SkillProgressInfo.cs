using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProgressInfo : MonoBehaviour
{
    public string progressSkillCode;

    public int skillCombo;

    public int currentSkillIndex;
    public SkillProgressInfo(string progressSkillCode, int skillCombo = 0, int currentSkillIndex = 0)
    {
        this.progressSkillCode = progressSkillCode;
        this.skillCombo = skillCombo;
        this.currentSkillIndex = currentSkillIndex;
    }
        

    public void AddCombo() => ++skillCombo;

    public void ResetCombo() => skillCombo = 0;
}

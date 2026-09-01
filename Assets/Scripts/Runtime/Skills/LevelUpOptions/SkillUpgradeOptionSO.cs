using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillUpgradeOptionSO", menuName = "코어 서바이벌/레벨업 선택지/스킬 강화")]
public class SkillUpgradeOptionSO : LevelUpOptionSO
{
    #region 인스펙터
    [Header("부분")]
    [SerializeField] private SkillID _skillID;
    #endregion

    public override void Apply(SkillManager skillManager, Player player)
    {
        if (skillManager == null)
        {
            Debug.LogWarning("스킬 매니저 null (SkillUpgradeOptionSO)");

            return;
        }

        if (!skillManager.HasSkill(_skillID))
        {
            Debug.LogWarning("스킬 없음 (SkillUpgradeOptionSO)");
            return;
        }

        skillManager.UpgradeSkill(_skillID);
    }
    public override bool IsAvailable(SkillManager skillManager)
    {
        if (_skillID == SkillID.None)
        {
            Debug.LogWarning($"스킬 ID 미설정 / {OptionName} 에셋 확인 요망");

            return false;
        }

        if (skillManager == null)
        {
            Debug.LogWarning("스킬매니저 null / 인스펙터 확인");

            return false;
        }
        
        return skillManager.CanUpgradeSkill(_skillID);
    }



}

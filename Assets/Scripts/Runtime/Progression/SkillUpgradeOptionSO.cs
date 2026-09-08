using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillUpgradeOptionSO", menuName = "코어 서바이벌/레벨업 선택지/스킬 강화")]
public class SkillUpgradeOptionSO : LevelUpOptionSO
{
    #region 인스펙터
    [Header("부분")]
    [SerializeField] private SkillDataSO _skillData;
    [SerializeField] private EUpgradeType _upgradeType;
    #endregion

    public override Sprite Icon => _skillData != null ? _skillData.Icon : null;

    public override void Apply(SkillManager skillManager, Player player)
    {
        if (_skillData == null)
        {
            Debug.LogWarning($"스킬 데이터 미설정 / {OptionName} 에셋 확인 요망");

            return;
        }

        if (skillManager == null)
        {
            Debug.LogWarning("스킬 매니저 null (SkillUpgradeOptionSO)");

            return;
        }

        if (!skillManager.HasSkill(_skillData.SkillID))
        {
            Debug.LogWarning("스킬 없음 (SkillUpgradeOptionSO)");

            return;
        }

        skillManager.UpgradeSkill(_skillData.SkillID, _upgradeType);
    }
    public override bool IsAvailable(SkillManager skillManager)
    {
        if (_skillData == null)
        {
            Debug.LogWarning($"스킬 데이터 미설정 / {OptionName} 에셋 확인 요망");

            return false;
        }

        if (skillManager == null)
        {
            Debug.LogWarning("스킬매니저 null / 인스펙터 확인");

            return false;
        }
        
        return skillManager.CanUpgradeSkill(_skillData.SkillID);
    }



}

using UnityEngine;

[CreateAssetMenu(fileName = "LearnSkillOptionSO", menuName = "코어 서바이벌/레벨업 선택지/스킬 습득")]
public class LearnSkillOptionSO : LevelUpOptionSO
{
    #region 인스펙터
    [Header("부분")]
	[SerializeField] private SkillDataSO _skillData;
    #endregion

    public override Sprite Icon => _skillData != null ? _skillData.Icon : null;

    public override void Apply(SkillManager skillManager, Player player)
    {
        if (skillManager == null)
        {
            Debug.LogWarning("스킬매니저 null / 인스펙터 확인");

            return;
        }

        if(_skillData == null)
        {
            Debug.LogWarning($"스킬 데이터 미설정 / {OptionName} 에셋 확인 요망");

            return;
        }

        skillManager.LearnSkill(_skillData.SkillID);
    }

    public override bool IsAvailable(SkillManager skillManager)
    {
        if(_skillData == null)
        {
            Debug.LogWarning($"스킬 데이터 미설정 / {OptionName} 에셋 확인 요망");

            return false;
        }

        if (skillManager == null)
        {
            Debug.LogWarning("스킬매니저 null / 인스펙터 확인");

            return false;
        }

        return !skillManager.HasSkill(_skillData.SkillID);
    }

}

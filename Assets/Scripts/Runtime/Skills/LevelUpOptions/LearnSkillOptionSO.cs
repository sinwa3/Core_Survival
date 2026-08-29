using UnityEngine;

[CreateAssetMenu(fileName = "LearnSkillOptionSO", menuName = "코어 서바이벌/레벨업 선택지/스킬 습득")]
public class LearnSkillOptionSO : LevelUpOptionSO
{
    #region 인스펙터
    [Header("부분")]
	[SerializeField] private SkillID _skillID;
    #endregion

    public override void Apply(SkillManager skillManager, Player player)
    {
        if (skillManager == null)
        {
            Debug.LogWarning("스킬매니저 null / 인스펙터 확인");

            return;
        }

        skillManager.LearnSkill(_skillID);
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

        if (skillManager.HasSkill(_skillID))
        {
            return false;
        }

        return true;
    }

}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillStatUI : MonoBehaviour
{
    #region 인스펙터
    [Header("아이콘")]
    [SerializeField] private Image _icon;

    [Header("텍스트")]
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _cooldownText;
    [SerializeField] private TMP_Text _upCountText;
    #endregion

    public void Setup(SkillDataSO data, SkillsBase skill)
    {
        if (data == null || skill == null)
        {
            Debug.LogWarning("스킬 데이터 혹은 스킬 베이스 null (SkillStat) / 일시 정지 메뉴 Setup 불가");

            return;
        }

        gameObject.SetActive(true);

        _icon.sprite = data.Icon;

        _damageText.text = $"데미지 {skill.DamageMultiplier * 100:0}%";
        _cooldownText.text = $"쿨타임 {skill.SkillCooldown:0.00}초";
        _upCountText.text = $"강화 횟수 {skill.CooldownLevel + skill.DamageLevel} / {skill.MaxUpgradeLevel}";

    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

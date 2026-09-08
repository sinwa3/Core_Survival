using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    #region 인스펙터
    [Header("슬롯")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _cooldownIcon;
    #endregion

    #region 내부 변수
    private SkillsBase _skill;
    #endregion

    void Update()
    {
        if (_skill == null)
        {
            return;
        }

        if (_cooldownIcon == null)
        {
            return;
        }

        _cooldownIcon.fillAmount = _skill.CooldownRemainRatio;
    }

    public void Setup(SkillDataSO data, SkillsBase skill)
    {
        if (data == null || skill == null)
        {
            Debug.LogWarning("스킬 데이터 혹은 스킬 null (스킬 슬롯) / 확인 요망");

            return; 
        }

        _skill = skill;

        gameObject.SetActive(true);

        _icon.sprite = data.Icon;
    }

    public void Hide()
    {
        _skill = null;
        gameObject.SetActive(false);
    }

}

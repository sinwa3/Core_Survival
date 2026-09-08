using UnityEngine;

public class SkillHUD : MonoBehaviour
{
    #region 인스펙터
    [Header("스킬")]
    [SerializeField] private SkillSlot[] _slots;
    [SerializeField] private SkillManager _skillManager;
    #endregion

    private void OnEnable()
    {
        if (_skillManager != null)
        {
            _skillManager.OnSkillLearned += SkillLearned;
        }
    }

    private void OnDisable()
    {
        if (_skillManager != null)
        {
            _skillManager.OnSkillLearned -= SkillLearned;
        }
    }

    void Start()
    {
        RefreshSlots();
    }

    private void SkillLearned(SkillID skillID)
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        int index = 0;

        if (_skillManager == null)
        {
            Debug.LogWarning("스킬 매니저 null (SkillHUD) / 인스펙터 확인");

            return;
        }

        foreach (var skill in _skillManager.Skills)
        {
            SkillDataSO skillData = _skillManager.GetSkillData(skill.Key);

            if (_slots.Length <= index)
            {
                Debug.LogWarning("슬롯 부족");

                break;
            }

            _slots[index].Setup(skillData, skill.Value);
            index++;
        }

        for (int i = index; i < _slots.Length; i++)
        {
            _slots[i].Hide();
        }
    }
}

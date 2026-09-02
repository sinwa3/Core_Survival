using UnityEngine;

public abstract class LevelUpOptionSO : ScriptableObject
{

    #region 인스펙터
    [Header("공통")]
    [SerializeField] private string _optionName;
    [TextArea] [SerializeField] private string _description;
    #endregion

    public string OptionName => _optionName;
    public string Description => _description;
    public abstract Sprite Icon
    {
        get;
    }


    public abstract void Apply(SkillManager skillManager, Player player);

    public virtual bool IsAvailable(SkillManager skillManager) => true;
}

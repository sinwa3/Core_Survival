using UnityEngine;

public abstract class LevelUpOptionSO : ScriptableObject
{

    #region 인스펙터
    [Header("공통")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _optionName;
    [TextArea] [SerializeField] private string _description;
    #endregion

    public string OptionName => _optionName;
    public string Description => _description;
    public Sprite Icon => _icon;


    public abstract void Apply(SkillManager skillManager, Player player);

    public virtual bool IsAvailable(SkillManager skillManager) => true;
}

using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "코어 서바이벌/스킬 데이터")]
public class SkillDataSO : ScriptableObject
{
    #region 인스펙터
    [SerializeField] private SkillID _skillID;
    [SerializeField] private Sprite _icon;
    [SerializeField] private SkillEffectBase _effectPrefab;
    [SerializeField] private float _cooldown;
    #endregion

    public SkillID SkillID => _skillID;
    public Sprite Icon => _icon;
    public float Cooldown => _cooldown;
    public SkillEffectBase EffectPrefab => _effectPrefab;

}

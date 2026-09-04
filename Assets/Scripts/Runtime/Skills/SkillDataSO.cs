using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "코어 서바이벌/스킬 데이터")]
public class SkillDataSO : ScriptableObject
{
    #region 인스펙터
    [SerializeField] private SkillID _skillID;
    [SerializeField] private Sprite _icon;
    [SerializeField] private SkillEffectBase _effectPrefab;
    [SerializeField] private int _effectCountPerCast = 1;

    [Header("데미지")]
    [SerializeField] private float _baseDamage;
    [SerializeField] private int _maxUpgradeLevel = 5;
    [SerializeField] private float _damagePerLevel = 0.2f;

    [Header("쿨타임")]
    [SerializeField] private float _cooldown;
    [SerializeField] private float _cooldownPerLevel = 0.2f;
    [SerializeField] private float _minCooldown = 0.0f;
    #endregion

    public SkillID SkillID => _skillID;
    public Sprite Icon => _icon;
    public float Cooldown => _cooldown;
    public SkillEffectBase EffectPrefab => _effectPrefab;
    public float BaseDamage => _baseDamage;
    public int MaxUpgradeLevel => _maxUpgradeLevel;
    public float DamagePerLevel => _damagePerLevel;
    public int EffectCountPerCast => _effectCountPerCast;
    public float CooldownPerLevel => _cooldownPerLevel;
    public float MinCooldown => _minCooldown;
}


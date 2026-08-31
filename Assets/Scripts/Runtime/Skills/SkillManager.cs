using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    #region 인스펙터
    [Header("스킬 데이터")]
    [SerializeField] private List<SkillDataSO> _skillDataList;

    [Header("적 스캔")]
    [SerializeField] private EnemyScanner _scanner;

    [Header("스킬 풀")]
    [SerializeField] private SkillEffectPooling _effectPool;
    #endregion

    #region 내부 변수
    private Dictionary<SkillID, SkillsBase> _skillDict = new Dictionary<SkillID, SkillsBase>();
    private Dictionary<SkillID, SkillDataSO> _skillDataDict = new Dictionary<SkillID, SkillDataSO>();
    #endregion

    public IReadOnlyDictionary<SkillID, SkillsBase> Skills => _skillDict;

    public event Action<SkillID> OnSkillLearned;

    private void Awake()
    {
        if (_scanner == null)
        {
            _scanner = GetComponent<EnemyScanner>();
        }

        if (_effectPool == null)
        {
            Debug.LogWarning("이펙트 풀 null / 인스펙터 확인");
            enabled = false;

            return;
        }

        if (_skillDataList == null || _skillDataList.Count == 0)
        {
            Debug.LogWarning("스킬 데이터 없음 / 인스펙터 확인");
            enabled = false;

            return;
        }

        for (int i = 0; i < _skillDataList.Count; i++)
        {
            SkillDataSO skillData = _skillDataList[i];

            if (skillData == null)
            {
                Debug.LogWarning($"스킬 데이터 {i}번 null / 인스펙터 확인");

                continue;
            }

            if (_skillDataDict.ContainsKey(skillData.SkillID))
            {
                Debug.LogWarning("딕셔너리에 등록된 스킬");

                continue;
            }

            _skillDataDict.Add(skillData.SkillID, skillData);
        }
    }

    void Start()
    {
        LearnSkill(SkillID.Cleave);
    }

    void Update()
    {
        Transform nearEnemy = _scanner.GetNearest();

        foreach (var skill in _skillDict.Values)
        {
            skill.TickCooltime(transform, nearEnemy);
        }
    }

    private bool LearnSkill(SkillsBase skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("존재하지 않는 스킬");

            return false;
        }

        if (_skillDict.ContainsKey(skill.SkillID))
        {
            Debug.LogWarning($"스킬 등록 불가 / 이미 보유중인 스킬 ({skill.SkillID})");

            return false;
        }

        _skillDict.Add(skill.SkillID, skill);
        OnSkillLearned?.Invoke(skill.SkillID);

        return true;
    }

    public bool LearnSkill(SkillID skillID)
    {
        SkillsBase skill = CreateSkill(skillID);

        return LearnSkill(skill);
    }

    private SkillsBase CreateSkill(SkillID skillID)
    {
        if (!_skillDataDict.TryGetValue(skillID, out SkillDataSO skillData))
        {
            Debug.LogWarning("등록되지 않은 스킬");

            return null;
        }

        switch (skillID)
        {
            case SkillID.Cleave:
                return new CleaveSkill(skillData.Cooldown, skillData.EffectPrefab, _effectPool);
            case SkillID.DamageAura:
                return new DamageAuraSkill(skillData.Cooldown, skillData.EffectPrefab, _effectPool);
            case SkillID.LaserBeam:
                return new LaserBeamSkill(skillData.Cooldown, skillData.EffectPrefab, _effectPool);
            default:
                Debug.LogWarning($"스킬 타입 미설정 / 확인 요망");
                return null;
        }
    }

    public bool HasSkill(SkillID skillID)
    {
        return _skillDict.ContainsKey(skillID);
    }

    public SkillDataSO GetSkillData(SkillID skillID)
    {
        if (!_skillDataDict.TryGetValue(skillID, out SkillDataSO skillData))
        {
            Debug.LogWarning("SkillID에 맞는 스킬 데이터 없음 null 반환");

            return null;
        }

        return skillData;
    }


}

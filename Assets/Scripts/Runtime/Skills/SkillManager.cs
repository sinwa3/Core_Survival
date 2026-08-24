using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    #region 인스펙터
    [Header("임시 스킬 프리팹")]
    [SerializeField] private SkillEffectBase _skillPrefab;

    [Header("적 스캔")]
    [SerializeField] private EnemyScanner _scanner;

    [Header("스킬 풀")]
    [SerializeField] private SkillEffectPooling _effectPool;
    #endregion

    #region 내부 변수
    private Dictionary<SkillID, SkillsBase> _skillDict = new Dictionary<SkillID, SkillsBase>();
    #endregion


    private void Awake()
    {
        if (_scanner == null)
        {
            _scanner = GetComponent<EnemyScanner>();
        }

        if (_skillPrefab == null)
        {
            Debug.LogWarning("스킬 프리팹 null / 인스펙터 확인");
            enabled = false;
            
            return;
        }

        if (_effectPool == null)
        {
            Debug.LogWarning("이펙트 프리팹 null / 인스펙터 확인");
            enabled = false;

            return;
        }
    }

    void Start()
    {
        LearnSkill(new CleaveSkill(1.5f, _skillPrefab, _effectPool));
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

        return true;
    }


}

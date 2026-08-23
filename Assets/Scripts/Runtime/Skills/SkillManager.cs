using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    #region 인스펙터
    [Header("임시 스킬 프리팹")]
    [SerializeField] private GameObject _skillPrefab;

    [Header("스킬 리스트")]
    [SerializeField] private List<SkillsBase> _skillList = new List<SkillsBase>();

    [Header("적 스캔")]
    [SerializeField] private EnemyScanner _scanner;
    #endregion

    #region 내부 변수

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
    }

    void Start()
    {
        LearnSkill(new CleaveSkill(1.5f, _skillPrefab));
    }

    void Update()
    {
        Transform nearEnemy = _scanner.GetNearest();

        for (int i = 0; i < _skillList.Count; i++)
        {
            _skillList[i].TickCooltime(transform, nearEnemy);
        }
    }

    private void LearnSkill(SkillsBase skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("존재하지 않는 스킬");

            return;
        }

        if (_skillList.Contains(skill))
        {
            Debug.LogWarning("스킬 등록 불가 / 리스트 중복");

            return;
        }

        _skillList.Add(skill);
    }


}

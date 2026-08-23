using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingSkills : MonoBehaviour
{
    #region 인스펙터
    [Header("사용할 스킬")]
    [SerializeField] private GameObject _skill;

    [Header("적 스캔")]
    [SerializeField] private EnemyScanner _scanner;

    [Header("스킬 옵션")]
    [SerializeField] private float _skillCooldown = 1.5f;
    #endregion

    #region 내부 변수
    private float _skillTimer = 0.0f;
    #endregion

    private void Awake()
    {
        if (_skill == null)
        {
            Debug.LogWarning("사용할 스킬 null / 인스펙터 확인");

            return;
        }

        if (_scanner == null)
        {
            _scanner = GetComponent<EnemyScanner>();
        }
    }

    void Update()
    {
        if (_skillTimer < _skillCooldown)
        {
            _skillTimer += Time.deltaTime;

            return;
        }

        Transform enemyTr = _scanner.GetNearest();

        if (enemyTr == null)
        {
            return;
        }

        UseSkill(enemyTr);
        _skillTimer = 0.0f;
    }

    private void UseSkill(Transform enemyTr)
    {
        Vector3 dir = (enemyTr.position - transform.position).normalized;

        Quaternion skillRot = Quaternion.LookRotation(dir, Vector3.up);
        GameObject skill = Instantiate(_skill);

        skill.transform.position = transform.position;
        skill.transform.rotation = skillRot;
    }
}

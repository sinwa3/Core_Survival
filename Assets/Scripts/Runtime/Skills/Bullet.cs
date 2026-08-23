using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region 인스펙터
    [Header("타겟")]
    [SerializeField] private Transform _targetTr;

    [Header("속도")]
    [SerializeField] private float _shotSpeed = 10.0f;

    [Header("태그")]
    [SerializeField] private string _enemyTag = "Enemy";
    #endregion

    void Update()
    {
        Vector3 dir = (_targetTr.position - transform.position).normalized;

        transform.position += dir * _shotSpeed * Time.deltaTime;
    }

    public void GetTarget(Transform tr)
    {
        _targetTr = tr;
    }

    private void OnTriggerEnter(Collider other)
    {
        tempEnemy tempEnemy = other.GetComponent<tempEnemy>();

        if (tempEnemy != null)
        {
            tempEnemy.TakeDamage(10.0f);
            Destroy(gameObject);
        }
    }
}

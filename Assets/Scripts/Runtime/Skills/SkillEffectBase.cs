using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffectBase : MonoBehaviour
{
    public abstract SkillID SkillID
    {
        get;
    }

    public bool printLog
    {
        get; protected set;
    } = true;

    #region 내부 변수
    private SkillEffectPooling _ownerPool;
    private AudioSource _audioSource;
    private float _skillDamage;
    #endregion

    protected float SkillDamage => _skillDamage;

    public bool IsPooled
    {
        get; private set;
    }


    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetOwnerPool(SkillEffectPooling pool)
    {
        _ownerPool = pool;
    }

    public virtual void OnSpawn()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }

        IsPooled = false;
    }

    public virtual void OnDespawn()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }

        IsPooled = true;
    }

    public void SetSkillDamage(float damage)
    {
        _skillDamage = damage;
    }

    protected virtual IEnumerator Co_Life(float skillDuration)
    {
        yield return new WaitForSeconds(skillDuration);

        ReturnToPool();
    }

    protected void ReturnToPool()
    {
        _ownerPool.ReturnSkillEffectPool(this);
    }
}

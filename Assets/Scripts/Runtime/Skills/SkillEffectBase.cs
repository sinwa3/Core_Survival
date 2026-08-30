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
    } = false;

    #region 내부 변수
    private SkillEffectPooling _ownerPool;
    private AudioSource _audioSource;
    #endregion

    private void Awake()
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
    }

    public virtual void OnDespawn()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }

    protected virtual IEnumerator Co_Life(float skillDuration)
    {
        yield return new WaitForSeconds(skillDuration);

        _ownerPool.ReturnSkillEffectPool(this);
    }
}

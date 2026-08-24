using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffectBase : MonoBehaviour
{
    public abstract SkillID SkillID
    {
        get;
    }

    #region 내부 변수
    private SkillEffectPooling _ownerPool;
    #endregion

    public void SetOwnerPool(SkillEffectPooling pool)
    {
        _ownerPool = pool;
    }

    public virtual void OnSpawn()
    {

    }

    public virtual void OnDespawn()
    {

    }

    protected virtual IEnumerator Co_Life(float skillDuration)
    {
        yield return new WaitForSeconds(skillDuration);

        _ownerPool.ReturnSkillEffectPool(this);
    }
}

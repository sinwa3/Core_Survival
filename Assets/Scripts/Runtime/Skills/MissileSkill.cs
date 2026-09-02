using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.Missile;

    public MissileSkill(SkillDataSO dataSO, SkillEffectPooling pool) : base(dataSO, pool)
    {

    }

    protected override void UseSkill(Player player, Transform target)
    {
        Vector3 dir = target.position - player.transform.position;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);

        SkillEffectBase skillEffect = skillEffectPool.GetEffect(SkillID, SkillPrefab, player.transform.position, rot);

        if (skillEffect == null)
        {
            Debug.LogWarning("이펙트 null (MissileSkill) / 스킬 사용 실패");

            return;
        }

        skillEffect.SetSkillDamage(CalcDamage(player));
    }
}

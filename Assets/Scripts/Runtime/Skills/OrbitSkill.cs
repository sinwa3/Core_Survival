using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.Orbit;

    public override bool NeedTarget => false;

    public OrbitSkill(SkillDataSO dataSO, SkillEffectPooling pool) : base(dataSO, pool)
    {

    }

    protected override void UseSkill(Player player, Transform target)
    {
        int count = EffectCountPerCast;

        if (count <= 0)
        {
            Debug.LogWarning("이펙트 수량이 0 이하 (OrbitSkill) / 스킬 사용 실패");

            return;
        }

        float angleStep = 360f / count;
        float damage = CalcDamage(player);

        for (int i = 0; i < count; i++)
        {
            SkillEffectBase skillEffect = skillEffectPool.GetEffect(SkillID, SkillPrefab, player.transform.position, Quaternion.identity);

            if (skillEffect == null)
            {
                Debug.LogWarning("이펙트 null (OrbitSkill) / 스킬 사용 실패");

                return;
            }

            skillEffect.SetSkillDamage(damage);

            OrbitEffect orbitEffect = skillEffect as OrbitEffect;

            if (orbitEffect == null)
            {
                Debug.LogWarning("이펙트 캐스팅 실패 (OrbitSkill) / 스킬 사용 실패");

                return;
            }

            orbitEffect.SetStartAngle(angleStep * i);
        }
    }
}

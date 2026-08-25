using UnityEngine;

public class DamageAuraSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.DamageAura;

    public override bool NeedTarget => false;

    public DamageAuraSkill(float skillCool, SkillEffectBase prefab, SkillEffectPooling pool) : base (skillCool, prefab, pool)
    {

    }

    protected override void UseSkill(Transform player, Transform target)
    {
        skillEffectPool.GetEffect(SkillID, skillPrefab, player.position, player.rotation);

        Debug.Log("데미지 아우라 스킬 사용 성공");
    }
}

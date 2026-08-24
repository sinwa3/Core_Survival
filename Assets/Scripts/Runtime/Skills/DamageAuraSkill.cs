using UnityEngine;

public class DamageAuraSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.DamageAura;
    public DamageAuraSkill(float skillCool, SkillEffectBase prefab, SkillEffectPooling pool) : base (skillCool, prefab, pool)
    {

    }

    protected override void UseSkill(Transform player, Transform target)
    {
        Vector3 dir = (target.transform.position - player.transform.position).normalized;
        dir.y = 0.0f;

        Quaternion skillRot = Quaternion.LookRotation(dir, Vector3.up);
        SkillEffectBase skillEffect = skillEffectPool.GetEffect(SkillID, skillPrefab, player.position, skillRot);

        Debug.Log("데미지 아우라 스킬 사용 성공");
    }
}

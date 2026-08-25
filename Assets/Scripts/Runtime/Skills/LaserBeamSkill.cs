using UnityEngine;

public class LaserBeamSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.LaserBeam;

    public LaserBeamSkill(float skillCool, SkillEffectBase prefab, SkillEffectPooling pool) : base(skillCool, prefab, pool)
    {

    }

    protected override void UseSkill(Transform player, Transform target)
    {
        Vector3 dir = (target.transform.position - player.transform.position).normalized;

        dir.y = 0.0f;

        Quaternion skillRot = Quaternion.LookRotation(dir, Vector3.up);
        skillEffectPool.GetEffect(SkillID, skillPrefab, player.position, skillRot);

        Debug.Log("레이저 스킬 발동 성공");
    }
}

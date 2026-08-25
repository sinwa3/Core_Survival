using UnityEngine;

public class LaserBeamSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.LaserBeam;

    public override bool NeedTarget => false;

    public LaserBeamSkill(float skillCool, SkillEffectBase prefab, SkillEffectPooling pool) : base(skillCool, prefab, pool)
    {

    }

    protected override void UseSkill(Transform player, Transform target)
    {
        Vector2 randPos = Random.insideUnitCircle.normalized;

        Vector3 dir = new Vector3(randPos.x, 0.0f, randPos.y);

        Quaternion randRot = Quaternion.LookRotation(dir);

        skillEffectPool.GetEffect(SkillID, skillPrefab, player.position, randRot);

        Debug.Log("레이저 스킬 발동 성공");
    }
}

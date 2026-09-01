using UnityEngine;

public class LaserBeamSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.LaserBeam;

    public override bool NeedTarget => false;

    public LaserBeamSkill(SkillDataSO dataSO, SkillEffectPooling pool) : base(dataSO, pool)
    {

    }

    protected override void UseSkill(Player player, Transform target)
    {
        Vector2 randPos = Random.insideUnitCircle.normalized;
        Vector3 dir = new Vector3(randPos.x, 0.0f, randPos.y);
        Quaternion randRot = Quaternion.LookRotation(dir);

        SkillEffectBase effect = skillEffectPool.GetEffect(SkillID, SkillPrefab, player.transform.position, randRot);

        if (effect == null)
        {
            Debug.LogWarning("이펙트 null (LaserBeamSkill) / 스킬 사용 실패");

            return;
        }

        effect.SetSkillDamage(CalcDamage(player));
    }
}

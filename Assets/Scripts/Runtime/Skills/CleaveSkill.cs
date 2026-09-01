using UnityEngine;

public class CleaveSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.Cleave;

    public CleaveSkill(SkillDataSO dataSO, SkillEffectPooling pool) : base(dataSO, pool)
    {

    }

    protected override void UseSkill(Player player, Transform target)
    {
        Vector3 dir = (target.transform.position - player.transform.position).normalized;
        dir.y = 0.0f;

        Quaternion skillRot = Quaternion.LookRotation(dir, Vector3.up);
        SkillEffectBase effect = skillEffectPool.GetEffect(SkillID, SkillPrefab, player.transform.position, skillRot);

        if (effect == null)
        {
            Debug.LogWarning("이펙트 null (CleaveSKill) / 스킬 사용 실패");

            return;
        }

        effect.SetSkillDamage(CalcDamage(player));
    }
}

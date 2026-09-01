using UnityEngine;

public class DamageAuraSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.DamageAura;

    public override bool NeedTarget => false;

    public DamageAuraSkill(SkillDataSO dataSO, SkillEffectPooling pool) : base(dataSO, pool)
    {

    }

    protected override void UseSkill(Player player, Transform target)
    {
        SkillEffectBase effect = skillEffectPool.GetEffect(SkillID, SkillPrefab, player.transform.position, player.transform.rotation);

        if (effect == null)
        {
            Debug.LogWarning("이펙트 null (DamageAuraSkill) / 스킬 사용 실패");

            return;
        }

        effect.SetSkillDamage(CalcDamage(player));
    }
}

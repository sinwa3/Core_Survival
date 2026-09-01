using UnityEngine;

public abstract class SkillsBase
{
    public abstract SkillID SkillID
    {
        get;
    }

    // 스킬 자체 정보
    public float SkillBaseDamage => skillData.BaseDamage;
    public int MaxUpgradeLevel => skillData.MaxUpgradeLevel;
    public float SkillCooldown => skillData.Cooldown;
    protected float skillTimer = 0.0f;

    // 스킬 프리팹
    protected SkillEffectBase SkillPrefab => skillData.EffectPrefab;
    // 스킬 풀
    protected SkillEffectPooling skillEffectPool;

    // 타겟 유무
    public virtual bool NeedTarget => true;

    private readonly SkillDataSO skillData;
    public int CurrentSkillLevel { get; protected set; } = 1;
    public bool CanSkillUpgrade => CurrentSkillLevel < MaxUpgradeLevel;

    public float DamageMultiplier => 1.0f + (skillData.DamagePerLevel * (CurrentSkillLevel - 1));
    public float CooldownRemainRatio
    {
        get
        {
            if (SkillCooldown == 0.0f)
            {
                return 0.0f;
            }

            float ratio = Mathf.Clamp01(skillTimer / SkillCooldown);

            return 1.0f - ratio;
        }
    }

    public SkillsBase(SkillDataSO dataSO, SkillEffectPooling effectPool)
    {
        skillEffectPool = effectPool;
        skillData = dataSO;
    }

    // 업데이트에서 스킬 쿨타임을 돌리고 사용
    public void TickCooltime(Player player, Transform target)
    {
        if (skillTimer < SkillCooldown)
        {
            skillTimer += Time.deltaTime;

            return;
        }

        if (NeedTarget && target == null)
        {
            return;
        }

        UseSkill(player, target);
        skillTimer = 0.0f;
    }

    public float CalcDamage(Player player)
    {
        if(player == null)
        {
            Debug.LogWarning($"플레이어 null (SkillBase) / 공격력 반영 안됨");

            return SkillBaseDamage * DamageMultiplier;
        }

        return SkillBaseDamage * DamageMultiplier * player.PlayerStats.attack;
    }

    public void UpgradeSkill()
    {
        if (!CanSkillUpgrade)
        {
            Debug.LogWarning($"스킬 {SkillID} 업그레이드 불가 / 현재 레벨 {CurrentSkillLevel} / 최대 레벨 {MaxUpgradeLevel}");

            return;
        }

        CurrentSkillLevel++;
    }


    // 실제 사용 로직
    protected abstract void UseSkill(Player player, Transform target);
}

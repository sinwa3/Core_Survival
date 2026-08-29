using UnityEngine;

public enum EStatType
{
    None,
    MaxHP,
    Speed
}

[CreateAssetMenu(fileName = "StatOptionSO", menuName = "코어 서바이벌/레벨업 선택지/스탯 강화")]
public class StatUpgradeOptionSO : LevelUpOptionSO
{
    #region 인스펙터
    [Header("부분")]
    [SerializeField] private EStatType _statType;
    [SerializeField] private float _statAmount = 0.0f;
    #endregion

    public override void Apply(SkillManager skillManager, Player player)
    {
        if (player == null)
        {
            Debug.LogWarning("플레이어 null / 인스펙터 확인");

            return;
        }

        switch (_statType)
        {
            case EStatType.MaxHP:
                player.IncreaseHP(_statAmount);
                break;
            case EStatType.Speed:
                player.IncreaseSpeed(_statAmount);
                break;
            default:
                Debug.LogWarning($"스탯 타입 미설정 / {OptionName} 에셋 확인 요망");
                break;
        }
    }
}

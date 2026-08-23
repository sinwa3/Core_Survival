using UnityEngine;

public class CleaveSkill : SkillsBase
{
    public CleaveSkill(float skillCool, GameObject prefab) : base (skillCool, prefab)
    {

    }

    protected override void UseSkill(Transform player, Transform target)
    {
        Vector3 dir = (target.transform.position - player.transform.position).normalized;
        dir.y = 0.0f;

        Quaternion skillRot = Quaternion.LookRotation(dir, Vector3.up);
        Object.Instantiate(skillPrefab, player.position, skillRot);

        Debug.Log("가시 스킬 발동 성공");
    }
}

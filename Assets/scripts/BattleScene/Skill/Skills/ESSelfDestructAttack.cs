using System.Collections;
using TMPro;
using UnityEngine;

public class ESSelfDestructAttack : Skill
{
    public override SkillKind skillKind => SkillKind.ESelfDestructAttack;

    public override string name => "자폭";

    public override string description => $"적에게 몸을 던져 공격력의 {(int)(floatParams[0] * 100)}% 피해를 입힌다.";

    public override bool isTargeting => true;
    public override float[] floatParams => new[]{
        3.5f
    };
    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy != caster.isEnemy;

    }
    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        if (caster is EnemyActor)
        {
            foreach (var skill in (caster as EnemyActor).skillPresets)
            {
                if (skill.SkillKind == SkillKind.ESelfDestructAttack)
                {
                    skill.SkillWeight = 0;
                }
                else if (skill.SkillKind == SkillKind.ESelfDestructReady)
                {
                    skill.SkillWeight = 100;
                }
            }
        }
        float a = 0;
        if (target.transform.position.x < caster.transform.position.y) a = 1;
        else a = -1;
        yield return caster.JumpAttack(() =>
        {
            target.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[0]), true, false, true);
        }, target.transform.position + new Vector3(a, 0, 0));
        caster.GetDamage(battleManager, caster, 999999999, false, true, false);
        yield break;
    }
}
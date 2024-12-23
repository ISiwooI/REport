using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESUnStableAttack : Skill
{
    public override string name => "불안정서";

    public override string description => "무작위 적에게 최대 3회의 피해를 입힌다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new float[]{
        0.7f
    };

    public override SkillKind skillKind => SkillKind.EUnStableAttack;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        List<BattleActor> list;


        yield return caster.FarAttack(() => { return; });
        int i = Utill.random.Next(1, 4);
        for (int j = 0; j < i; j++)
        {
            list = battleManager.GetAliveUnFriendly(caster.isEnemy);
            int ri = Utill.random.Next(list.Count);
            target = list[ri];
            target.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[0]), true, false);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
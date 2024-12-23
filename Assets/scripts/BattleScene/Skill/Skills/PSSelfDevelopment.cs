using System.Collections;
using UnityEngine;

public class PSSelfDevelopment : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.None;

    public override int departmentCost => 0;

    public override int normalCost => 0;

    public override string name => "자기 개발";

    public override string description => $"학과 코스트를{(int)(floatParams[0] * 100)}% 회복하고, 코스트를 {intParams[0]} 획득한다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new[]{
        0.4f
    };
    public override int[] intParams => new[]{
        3
    };
    public override SkillKind skillKind => SkillKind.PSelfDevelopment;

    public override int cardImageIndex => 11;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.TextParticle(battleManager, "자기 개발");
            if (caster is PlayerActor)
            {
                (caster as PlayerActor).DCost += (int)((caster as PlayerActor).MaxDCost * 0.4f);
            }
        });
    }
}
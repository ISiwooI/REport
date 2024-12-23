using System.Collections;

public class ESCharmingParalyse : Skill
{
    public override string name => "매력 발산";

    public override string description => $"대상의 학과 코스트를 최대치의 {(int)(floatParams[0] * 100)}% 감소시킨다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[]{
        0.1f
    };

    public override SkillKind skillKind => SkillKind.ECharmingParalyse;
    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy != caster.isEnemy;

    }
    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {//
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            target.TextParticle(battleManager, "코스트 감소!");
            if (target is PlayerActor)
            {
                (target as PlayerActor).DCost -= (int)((target as PlayerActor).MaxDCost * floatParams[0]);
            }
        });
    }
}
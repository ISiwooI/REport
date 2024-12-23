using System;
using System.Collections;

public class ESCharmingHeal : Skill
{
    public override string name => "시각적 재새동";
    public override string description => $"대상을 자신의 최대 체력의{(int)(floatParams[0] * 100)}% 만큼 회복시킨다.";
    public override bool isTargeting => true;
    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy == caster.isEnemy;

    }
    public override float[] floatParams => new float[]
        {
            0.15f
        };
    public override SkillKind skillKind => SkillKind.ECharmingHeal;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {//1차완
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            target.GetHeal(battleManager, target, (int)(caster.maxHP * floatParams[0]));
        });
    }
}
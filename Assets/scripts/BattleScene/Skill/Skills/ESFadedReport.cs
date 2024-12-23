using System.Collections;

public class ESFadedReport : Skill
{
    public override string name => "빛바랜 과제물";
    public override string description => "지정한 적에게 공격력 비례 대미지를 입힌다.";
    public override bool isTargeting => true;
    public override float[] floatParams => new[]{
        1.4f
    };

    public override SkillKind skillKind => SkillKind.EFadedReport;

    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy != caster.isEnemy;

    }

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            target.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[0]));
        });

    }
}
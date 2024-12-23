using System.Collections;

public class PSTouchOfSalvation : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Theology;

    public override int departmentCost => 20;

    public override int normalCost => 4;

    public override string name => "구원의 손길";

    public override string description => "아군에게 죽음에 이르는 피해를 입었을 때 한번 되살아나는 버프를 제공한다.";

    public override bool isTargeting => true;

    public override SkillKind skillKind => SkillKind.PTouchOfSalvation;

    public override int cardImageIndex => 20;
    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy == caster.isEnemy;

    }
    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            target.GetBuff(battleManager, BattleBuffKind.TouchOfSalvation, 1);
        });
        yield break;
    }
}
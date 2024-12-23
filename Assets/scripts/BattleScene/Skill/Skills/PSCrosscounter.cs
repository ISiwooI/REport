using System.Collections;

public class PSCrosscounter : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Sport;

    public override int departmentCost => 30;

    public override int normalCost => 4;

    public override string name => "크로스 카운터";

    public override string description => "3턴동안 공격을 받을 때 마다 방어력 수치에 비례하게 공격자에게 반격한다.";

    public override bool isTargeting => false;



    public override SkillKind skillKind => SkillKind.PCrosscounter;

    public override int cardImageIndex => 4;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetBuff(battleManager, BattleBuffKind.CrossCounter, 1);
        });
        yield break;
    }
}
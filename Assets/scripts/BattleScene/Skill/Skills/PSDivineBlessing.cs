using System.Collections;

public class PSDivineBlessing : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Theology;

    public override int departmentCost => 60;

    public override int normalCost => 1;

    public override string name => "신성한 축복";

    public override string description => "3 페이즈 동안 지정한 아군의 공격력, 방어력, 속도 수치가 증가한다.";

    public override bool isTargeting => true;



    public override SkillKind skillKind => SkillKind.PDivineBlessing;

    public override int cardImageIndex => 7;
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
            target.GetBuff(battleManager, BattleBuffKind.DivineBlessing, 1);
        });
    }
}
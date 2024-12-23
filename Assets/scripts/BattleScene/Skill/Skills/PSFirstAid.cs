using System.Collections;

public class PSFirstAid : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.None;


    public override int departmentCost => 0;

    public override int normalCost => 3;

    public override string name => "응급 처치";

    public override string description => $"자신 체력 수치의 {floatParams[0] * 100}% 만큼 아군을 회복시킨다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[]{
        0.3f
    };

    public override SkillKind skillKind => SkillKind.PFirstAid;

    public override int cardImageIndex => 6;
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
            target.GetHeal(battleManager, caster, (int)(caster.maxHP * floatParams[0]));
        });
    }
}
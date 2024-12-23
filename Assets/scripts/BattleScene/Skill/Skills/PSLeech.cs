using System.Collections;

public class PSLeech : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Sport;

    public override int departmentCost => 30;

    public override int normalCost => 2;

    public override string name => "생기 흡수";

    public override string description => "3페이즈 동안 공격에 성공했을 때 방어력에 비례해서 체력을 회복한다.";

    public override bool isTargeting => false;

    public override float[] floatParams => throw new System.NotImplementedException();

    public override SkillKind skillKind => SkillKind.PLeech;

    public override int cardImageIndex => 0;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetBuff(battleManager, BattleBuffKind.Leech, 1);
        });
    }
}
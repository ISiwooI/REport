using System.Collections;

public class PSHealtyBody : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Sport;

    public override int departmentCost => 25;

    public override int normalCost => 4;

    public override string name => "건강한 육체";

    public override string description => "3페이즈 동안 자신의 턴이 끝날 때 최대 체력에 비례해서 체력을 회복한다.";

    public override bool isTargeting => false;

    public override SkillKind skillKind => SkillKind.PHealtyBody;

    public override int cardImageIndex => 15;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetBuff(battleManager, BattleBuffKind.HeltyBody, 1);
        });
    }
}
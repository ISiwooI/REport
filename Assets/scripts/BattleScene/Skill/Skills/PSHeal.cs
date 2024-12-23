using System.Collections;

public class PSHeal : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.None;

    public override int departmentCost => 0;

    public override int normalCost => 3;

    public override string name => "회복";

    public override string description => $"자신의 체력을 최대 채력의 {(int)(floatParams[0] * 100)}% 만큼 회복한다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new[]{
        0.3f
    };

    public override SkillKind skillKind => SkillKind.PHeal;

    public override int cardImageIndex => 13;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetHeal(battleManager, caster, (int)(caster.maxHP * floatParams[0]));
        });
        yield break;
    }
}
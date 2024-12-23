using System.Collections;

public class PSSelfDefense : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.None;

    public override int departmentCost => 0;

    public override int normalCost => 3;

    public override string name => "호신술";

    public override string description => $"이번 페이즈가 끝날 때 까지 방어력의 {(int)(floatParams[0] * 100)}% 수치의 보호막을 획득한다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new[]{
        60f
    };

    public override SkillKind skillKind => SkillKind.PSelfDefense;

    public override int cardImageIndex => 16;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetShield(battleManager, caster, (int)(caster.def * floatParams[0]), 1);
        });
        yield break;
    }
}
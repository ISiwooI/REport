using System.Collections;

public class PSMercyProtect : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Theology;

    public override int departmentCost => 30;

    public override int normalCost => 3;

    public override string name => "자애로운 보호";

    public override string description => $"아군에게 2 페이즈 동안 최대 체력의 {(int)(floatParams[0] * 100)}%의 보호막을 부여하고, 보호막이 사라질 때 최대 체력의{(int)(floatParams[1] * 100)}% 만큼 회복시킨다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[] {
        0.5f,
        0.2f
        };

    public override SkillKind skillKind => SkillKind.PMercyProtect;

    public override int cardImageIndex => 9;
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
            target.GetShield(battleManager, caster, (int)(caster.maxHP * floatParams[0]), 2, onExitShield: (manager, actor, isBrake) =>
            {
                actor.GetHeal(battleManager, caster, (int)(caster.maxHP * floatParams[1]));
            });
        });
    }
}
using System.Collections;

public class PSHealingLight : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Theology;

    public override int departmentCost => 30;

    public override int normalCost => 3;

    public override string name => "치유의 빛";

    public override string description => $"모든 아군의 체력을 자신 최대 체력의{(int)(floatParams[0] * 100)}%만큼 회복한다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new[]{
        0.3f
    };

    public override SkillKind skillKind => SkillKind.PHealingLight;

    public override int cardImageIndex => 8;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            foreach (BattleActor a in battleManager.GetAliveFriendly(caster.isEnemy))
            {
                a.GetHeal(battleManager, caster, (int)(caster.maxHP * floatParams[0]));
            }
        });
        yield break;
    }
}
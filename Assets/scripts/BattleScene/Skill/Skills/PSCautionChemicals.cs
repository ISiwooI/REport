using System.Collections;

public class PSCautionChemicals : PlayerSkill
{//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~여기부터
    public override DepartmentKind departmentKind => DepartmentKind.Camical;

    public override int departmentCost => 30;

    public override int normalCost => 4;

    public override string name => "취급주의 화학물질";

    public override string description => $"지정한 적에게 공격력의{(int)(floatParams[0] * 100)}% 피해를 입힌다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[]{
        2.3f
    };

    public override int cardImageIndex => 3;
    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy != caster.isEnemy;

    }
    public override SkillKind skillKind => SkillKind.PCautionChemicals;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            target.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[0]));

        });

    }
}
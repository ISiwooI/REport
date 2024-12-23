using System.Collections;

public class PSFlammableFlask : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Camical;

    public override int departmentCost => 40;

    public override int normalCost => 3;

    public override string name => "인화성 플라스크";

    public override string description => $"적에게 공격력의{floatParams[0]}% 대미지를 주고 추가로 주변 적에게 공격력의{floatParams[1]}% 대미지를 준다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[]{
        1.5f,
        1.2f
    };

    public override SkillKind skillKind => SkillKind.PFlammableFlask;

    public override int cardImageIndex => 5;
    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy != caster.isEnemy;

    }
    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            target.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[0]));
        });
        BattleActor front;
        BattleActor back;
        front = battleManager.GetFrontActor(target);
        back = battleManager.GetBackActor(target);
        if (front != null)
        {
            front.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[1]));
        }
        if (back != null)
        {
            back.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[1]));
        }

    }
}
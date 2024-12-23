using System.Collections;

public class PSGasCanister : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Camical;

    public override int departmentCost => 50;

    public override int normalCost => 3;

    public override string name => "화학공학 가스탄";

    public override string description => $"적에게 공격력의{(int)(floatParams[0] * 100)} 3턴 고정 피해를 주는 중독 상태이상을 부여한다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[]{
        1.3f
    };

    public override SkillKind skillKind => SkillKind.PGasCanister;

    public override int cardImageIndex => 17;
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
        target.GetBuff(battleManager, BattleBuffKind.Poisoned, 1);
    }
}
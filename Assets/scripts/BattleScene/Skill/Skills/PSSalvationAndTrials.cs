using System.Collections;

public class PSSalvationAndTrials : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Theology;

    public override int departmentCost => 40;

    public override int normalCost => 9;

    public override string name => "구원과 시련";

    public override string description => "다음 페이즈에 모든 아군이 피해를 입지 않고, 그 다음 페이즈에 각자의 최대 체력에 비례한 피해를 입는다.";

    public override bool isTargeting => false;

    public override float[] floatParams => throw new System.NotImplementedException();

    public override SkillKind skillKind => SkillKind.PSalvationAndTrials;

    public override int cardImageIndex => 10;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            foreach (BattleActor a in battleManager.GetAliveFriendly(caster.isEnemy))
            {
                a.GetBuff(battleManager, BattleBuffKind.Care, 1);
            }
        });
        yield break;

    }
}
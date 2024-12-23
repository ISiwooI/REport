using System.Collections;

public class PSExperimentFailed : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Camical;

    public override int departmentCost => 70;

    public override int normalCost => 6;

    public override string name => "실험 실패";

    public override string description => $"아군을 포함한 전체 인원에게 공격력의 {(int)(floatParams[0] * 100)}% 피해를 입힌다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new[] { 2.2f };

    public override SkillKind skillKind => SkillKind.PExperimentFailed;

    public override int cardImageIndex => 1;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            foreach (BattleActor a in battleManager.GetAliveActors())
            {
                a.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[0]));
            }
        });

    }
}
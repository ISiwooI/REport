using System.Collections;
using UnityEngine;

public class PSClinicalTrials : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Camical;
    public override int departmentCost => 0;
    public override int normalCost => 2;
    public override string name => "임상 실험";

    public override string description => $"다음 스킬 사용시까지 공격력이 {(int)(floatParams[0] * 100)}% 증가하고, 학과 자원을 {intParams[0]} 회복한다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new float[]{
        1.0f,
    };
    public override int[] intParams => new int[]{
        40
    };
    public override SkillKind skillKind => SkillKind.PClinicalTrials;

    public override int cardImageIndex => 18;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetBuff(battleManager, BattleBuffKind.ClinicalTrials, 1);
            (caster as PlayerActor).DCost += intParams[0];
        }
        );
    }
}
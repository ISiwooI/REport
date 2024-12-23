
using System;
using System.Collections;

public class ESAlcoholic : Skill
{
    public override string name => "알코홀릭";
    public override string description => "2 페이즈 동안 방어력이 증가하고 최대 체력 비례 보호막을 획득한다.";
    public override bool isTargeting => false;
    public override float[] floatParams => new[]{
        0.3f,0.1f
    };
    public override SkillKind skillKind => SkillKind.EAlcoholic;
    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {//1차완
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetBuff(battleManager, BattleBuffKind.Alcoholic, 1);
        });
        caster.GetShield(battleManager, caster, (int)(caster.maxHP * floatParams[1]), 2);
        yield break;
    }
}
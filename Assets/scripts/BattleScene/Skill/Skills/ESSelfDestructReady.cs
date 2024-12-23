using System.Collections;

public class ESSelfDestructReady : Skill
{
    public override SkillKind skillKind => SkillKind.ESelfDestructReady;

    public override string name => "자폭 준비";

    public override string description => "자폭 준비 버프 1 중첩 획득";

    public override bool isTargeting => false;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        BattleBuff b = null;
        b = caster.SerchBuff((value) => value.kind == BattleBuffKind.SelfDestructReady);
        int i = 0;
        if (b != null) i = b.stack;
        i++;
        yield return caster.FarAttack(() =>
        {
            caster.GetBuff(battleManager, BattleBuffKind.SelfDestructReady, i);
        });
    }
}
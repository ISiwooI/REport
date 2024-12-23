/*
도발 구현에 대한 고찰
도발을 구현하기 위해서 필요한 정보.
도발 상대.
도발 대상.
개별 도발을 구현하려면 도발하는 상대나 대상에 대한 정보가 필수.
방법 1 콜백에 현재 행동하려는 대상에 대한 정보를 포함
OnStatcalculate 콜백에 현재 차례의 액터를 포함
*/

using System.Collections;

public class PSTacticalTaunt : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.Sport;

    public override int departmentCost => 30;

    public override int normalCost => 3;

    public override string name => "전술적 도발";

    public override string description => $"2턴간 방어력의 {(int)(floatParams[0] * 100)}%의 보호막을 획득하고, 모든 적을 도발한다.";

    public override bool isTargeting => false;

    public override float[] floatParams => new[]{
        27f
    };

    public override SkillKind skillKind => SkillKind.PTacticalTaunt;

    public override int cardImageIndex => 14;

    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        yield return caster.FarAttack(() =>
        {
            caster.GetShield(battleManager, caster, (int)(caster.def * floatParams[0]), 2, true);
        });
        foreach (BattleActor a in battleManager.GetAliveUnFriendly(caster.isEnemy))
        {
            a.GetBuff(battleManager, BattleBuffKind.TacticalTaunt, 1, caster);
        }
    }
}
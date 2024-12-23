//난동
using System.Collections;
using DG.Tweening;
using UnityEngine.UIElements;

public class ESRanAmuck : Skill
{
    public override string name => "취중난동";

    public override string description => $"지정한 적에게 방어력의 {(int)(floatParams[0] * 100)}% 피해를 입히고, 도발한다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[]{
        4.2f
    };

    public override SkillKind skillKind => SkillKind.ERanAmuck;

    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy != caster.isEnemy;

    }
    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        caster.LookPosition(target.transform.position);
        caster.animator.SetTrigger("Jump");
        float posdiff = -1.2f;
        if (caster.transform.position.x > target.transform.position.x) posdiff *= -1;
        yield return caster.transform.DOJump(new UnityEngine.Vector3(target.transform.position.x + posdiff, target.transform.position.y), 1.5f, 1, caster.jumpAttackDelay).WaitForCompletion();
        yield return caster.NearAttack(() =>
        {
            target.GetDamage(battleManager, caster, (int)(caster.def * floatParams[0]));
        });
        target.GetBuff(battleManager, BattleBuffKind.Taunt, 1, caster);
        caster.LookPosition(caster.OriginPos);
        yield return caster.transform.DOJump(caster.OriginPos, 1.5f, 1, caster.jumpAttackDelay).WaitForCompletion();
        caster.LookPosition(new UnityEngine.Vector3(0, 0, 0));
    }
}
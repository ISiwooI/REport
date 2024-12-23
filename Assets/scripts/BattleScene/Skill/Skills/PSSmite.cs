using System.Collections;
using DG.Tweening;

public class PSSmite : PlayerSkill
{
    public override DepartmentKind departmentKind => DepartmentKind.None;

    public override int departmentCost => 0;

    public override int normalCost => 3;

    public override string name => "강타";

    public override string description => $"적에게 공격력의 {(int)(floatParams[0] * 100)}% 피해를 입힌다.";

    public override bool isTargeting => true;

    public override float[] floatParams => new[]{
        1.3f
        };

    public override SkillKind skillKind => SkillKind.PSmite;

    public override int cardImageIndex => 2;
    public override bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) return false;
        return target.isEnemy != caster.isEnemy;

    }
    public override IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (!IsParametersValid(battleManager, caster, target)) yield break;
        caster.LookPosition(target.transform.position);
        caster.animator.SetBool("IsRunning", true);
        float posdiff = -1.2f;
        if (caster.transform.position.x > target.transform.position.x) posdiff *= -1;
        yield return caster.transform.DOMove(new UnityEngine.Vector3(target.transform.position.x + posdiff, target.transform.position.y), 1f).WaitForCompletion();
        caster.animator.SetBool("IsRunning", false);
        yield return caster.NearAttack(() =>
        {
            target.GetDamage(battleManager, caster, (int)(caster.atk * floatParams[0]), true);
            caster.OnAttack?.Invoke(battleManager, caster, target, (int)(caster.atk * floatParams[0]));
        });
        caster.LookPosition(caster.OriginPos);
        caster.animator.SetBool("IsRunning", true);
        yield return caster.transform.DOMove(caster.OriginPos, 1f).WaitForCompletion();
        caster.animator.SetBool("IsRunning", false);
        caster.LookPosition(new UnityEngine.Vector3(0, 0, 0));
        yield break;
    }
}
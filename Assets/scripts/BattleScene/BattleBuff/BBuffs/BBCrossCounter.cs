using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;

public class BBCrossCounter : BattleBuff
{
    public override string name => "카운터 준비";

    public override string simpleDescription => "공격을 받았을 때 방어력에 비례해서 적에게 반격한다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 3;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => true;

    public override BattleBuffKind kind => BattleBuffKind.CrossCounter;
    public override IEnumerator TurnEndCoroutine(BattleManager manager, BattleActor actor)
    {

        if (manager is null) yield break;
        if (actor is null) yield break;
        if (actor.isDead) yield break;
        if (target is null) yield break;
        if (target.isDead) yield break;
        actor.LookPosition(target.transform.position);
        actor.animator.SetBool("IsRunning", true);
        float posdiff = -1.2f;
        if (actor.transform.position.x > target.transform.position.x) posdiff *= -1;
        yield return actor.transform.DOMove(new UnityEngine.Vector3(target.transform.position.x + posdiff, target.transform.position.y), 1f).WaitForCompletion();
        actor.LookPosition(target.transform.position);
        actor.animator.SetBool("IsRunning", false);
        yield return actor.NearAttack(() =>
        {
            target.GetDamage(manager, actor, (int)(actor.def * 20f), false);
            actor.OnAttack?.Invoke(manager, actor, target, (int)(actor.def * 20f));
        });
        actor.LookPosition(actor.OriginPos);
        actor.animator.SetBool("IsRunning", true);
        yield return actor.transform.DOMove(actor.OriginPos, 1f).WaitForCompletion();
        actor.animator.SetBool("IsRunning", false);
        actor.LookPosition(new UnityEngine.Vector3(0, 0, 0));
        yield break;
    }
    public override void OnAttacked(BattleManager manager, BattleActor self, BattleActor caster, int amount)
    {
        if (self.IsFriendly(caster)) return;
        target = caster;
        self.TurnEndListeners.Add(TurnEndCoroutine);
    }
    public override void OnTurnEnd(BattleManager manager, BattleActor self, BattleActor turnEndActor)
    {
        if (self == turnEndActor)
        {
            leftDuration--;
            if (leftDuration <= 0)
            {
                self.ReleseBuff(this);
            }
        }
    }
}
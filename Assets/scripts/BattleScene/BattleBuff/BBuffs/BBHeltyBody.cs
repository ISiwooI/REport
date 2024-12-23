using System.Collections;
using UnityEngine;

public class BBHeltyBody : BattleBuff
{
    public override string name => "자연 치유";

    public override string simpleDescription => "자신의 턴이 끝날 때 최대 체력에 비례해서 체력을 회복한다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 3;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.HeltyBody;
    public override IEnumerator TurnEndCoroutine(BattleManager manager, BattleActor actor)
    {
        if (actor.isDead) yield break;
        actor.GetHeal(manager, actor, (int)(actor.maxHP * 0.2f), false);
        yield return new WaitForSeconds(0.5f);
    }
    public override void OnTurnEnd(BattleManager manager, BattleActor self, BattleActor turnEndActor)
    {
        if (self.isDead) return;
        if (self == turnEndActor) self.TurnEndListeners.Add(TurnEndCoroutine);
    }
    public override void OnPhaseEnd(BattleManager manager, BattleActor self)
    {
        if (self.isDead) return;
        leftDuration--;
        if (leftDuration <= 0)
        {
            self.ReleseBuff(this);
            return;
        }
    }
}

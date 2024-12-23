using System.Collections;
using UnityEngine;

public class BBPoisoned : BattleBuff
{
    public override string name => "중독";

    public override string simpleDescription => "매 턴 60의 고정 피해를 입는다.";

    public override bool isPositive => false;

    public override bool isNegative => true;

    public override int timeDuration => 3;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.Poisoned;
    public override IEnumerator TurnEndCoroutine(BattleManager manager, BattleActor actor)
    {
        if (actor.isDead) yield break;
        actor.GetDamage(manager, actor, (int)(actor.maxHP * 0.15f), false, true, true);
        yield return new WaitForSeconds(0.5f);
    }
    public override void OnTurnEnd(BattleManager manager, BattleActor self, BattleActor turnEndActor)
    {
        self.TurnEndListeners.Add(TurnEndCoroutine);
        leftDuration--;
        if (leftDuration <= 0)
        {
            self.ReleseBuff(this);
            return;
        }

    }

}

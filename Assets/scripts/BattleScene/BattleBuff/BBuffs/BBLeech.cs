using System;

public class BBLeech : BattleBuff
{
    public override string name => "생기 흡수";

    public override string simpleDescription => "공격에 성공했을 때 방어력에 비례해서 체력을 회복한다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 3;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.Leech;

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
    public override void OnAttack(BattleManager manager, BattleActor self, BattleActor target, int amount)
    {
        if (manager is null)
        {
            throw new ArgumentNullException(nameof(manager));
        }
        if (self is null)
        {
            throw new ArgumentNullException(nameof(self));
        }
        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }
        self.GetHeal(manager, self, (int)(self.def * 10f));
    }
}
public class BBTaunt : BattleBuff
{
    public override string name => "도발";

    public override string simpleDescription => $"{target?.actorName}에게 도발당했다.";

    public override bool isPositive => false;

    public override bool isNegative => true;

    public override int timeDuration => 2;

    public override bool canMultiple => true;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => true;

    public override BattleBuffKind kind => BattleBuffKind.Taunt;
    public override void OnPhaseEnd(BattleManager manager, BattleActor self)
    {

        leftDuration--;
        if (leftDuration <= 0)
        {
            self.ReleseBuff(this);
        }

    }
    public override void OnUpdateTargetableActor(BattleManager manager, BattleActor self)
    {

        if (target == null || target.isDead)
        {
            self.ReleseBuff(this);
        }
        else
        {
            if (target.IsFriendly(self))
            {
                self.tauntedFriendly.Add(target);
            }
            else
            {
                self.tauntedEnemy.Add(target);
            }
        }
    }
}

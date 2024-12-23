public class BBAlcoholic : BattleBuff
{
    public override string name => "알코홀릭";

    public override string simpleDescription => "방어력이 증가한다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 3;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.Alcoholic;

    public override void OnStatCalculate(BattleManager manager, BattleActor self)
    {
        self.defPer += 0.6f;
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

//가호
public class BBSalvation : BattleBuff
{
    public override string name => "가호";

    public override string simpleDescription => "피해에 면역이 된다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 1;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.Salvation;
    public override void OnStatCalculate(BattleManager manager, BattleActor self)
    {
        self.damageMultiplier -= 99999f;
    }
    public override void OnPhaseEnd(BattleManager manager, BattleActor self)
    {
        self.GetDamage(manager, self, (int)(self.maxHP * 0.2f));
        self.ReleseBuff(this);
    }
}

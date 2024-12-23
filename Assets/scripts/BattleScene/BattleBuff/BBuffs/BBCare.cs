//시련
public class BBCare : BattleBuff
{
    public override string name => "보살핌";

    public override string simpleDescription => "다음 페이즈 시작 시 가호 버프를 획득한다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 1;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.Care;

    public override void OnPhaseStart(BattleManager manager, BattleActor self)
    {
        self.GetBuff(manager, BattleBuffKind.Salvation, 1);
        self.ReleseBuff(this);
    }
}



public class BBDivineBlessing : BattleBuff
{
    public override string name => "신성한 축복";

    public override string simpleDescription => "공격력, 방어력, 속도 수치가 증가한다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 3;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.DivineBlessing;
    public override void OnStatCalculate(BattleManager manager, BattleActor self)
    {
        self.atkPer += 0.7f;
        self.defPer += 0.7f;
        self.speedPer += 0.7f;
    }
    public override void OnPhaseEnd(BattleManager manager, BattleActor battleActor)
    {
        if (battleActor.isDead) return;
        leftDuration--;
        if (leftDuration <= 0)
        {
            battleActor.ReleseBuff(this);
        }

    }
}
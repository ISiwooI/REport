public class BBClinicalTrials : BattleBuff
{
    public override string name => "임상실험";

    public override string simpleDescription => "다음 자신의 턴이 끝날 때 까지 공격력이 2배가 된다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 2;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.ClinicalTrials;
    public override void OnStatCalculate(BattleManager manager, BattleActor self)
    {
        self.atkPer += 1.0f;
    }
    public override void OnTurnEnd(BattleManager manager, BattleActor self, BattleActor turnEndActor)
    {
        if (self.isDead) return;
        leftDuration--;
        if (leftDuration == 0)
            self.ReleseBuff(this);
    }
}
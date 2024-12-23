using UnityEngine.AI;

public class BBTouchOfSalvation : BattleBuff
{
    public override string name => "구원의 손길";

    public override string simpleDescription => "행동 불능 상태가 될 때 한번 hp의 일부를 회복하며 부활한다";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => -1;

    public override bool canMultiple => false;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.TouchOfSalvation;
    public override void OnAttacked(BattleManager manager, BattleActor self, BattleActor caster, int amount)
    {
        if (self.hp <= 0)
        {
            self.hp = 1;
            self.GetHeal(manager, self, (int)(self.maxHP * 0.2f), false);
            self.ReleseBuff(this);
        }
    }
}

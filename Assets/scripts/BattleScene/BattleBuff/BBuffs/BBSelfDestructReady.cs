public class BBSelfDestructReady : BattleBuff
{
    public override string name => "자폭 준비";

    public override string simpleDescription => (stack >= 3) ? "곧 자폭 공격을 할 것 같다." : "자폭 공격을 준비중이다.";

    public override bool isPositive => false;

    public override bool isNegative => false;

    public override int timeDuration => 1;

    public override bool canMultiple => false;

    public override bool canStack => true;

    public override int maxStack => 3;

    public override bool needTarget => false;

    public override BattleBuffKind kind => BattleBuffKind.SelfDestructReady;
    public override void OnAttacked(BattleManager manager, BattleActor self, BattleActor caster, int amount)
    {
        if (self is EnemyActor && self.isDead)
        {
            foreach (var skill in (self as EnemyActor).skillPresets)
            {
                if (skill.SkillKind == SkillKind.ESelfDestructAttack)
                {
                    skill.SkillWeight = 0;
                }
                else if (skill.SkillKind == SkillKind.ESelfDestructReady)
                {
                    skill.SkillWeight = 100;
                }
            }
        }
    }

    public override void OnTurnStart(BattleManager manager, BattleActor self, BattleActor turnStartActor)
    {
        if (stack >= maxStack)
        {
            if (self is EnemyActor)
            {
                foreach (var skill in (self as EnemyActor).skillPresets)
                {
                    if (skill.SkillKind == SkillKind.ESelfDestructAttack)
                    {
                        skill.SkillWeight = 9999999;
                    }
                    else if (skill.SkillKind == SkillKind.ESelfDestructReady)
                    {
                        skill.SkillWeight = 0;
                    }
                }
            }
        }
    }
}
public class BattleBuffDelegater
{
    public BattleBuff GetBuff(BattleBuffKind buffKind)
    {
        switch (buffKind)
        {
            case BattleBuffKind.Alcoholic:
                return new BBAlcoholic();
            case BattleBuffKind.ClinicalTrials:
                return new BBClinicalTrials();
            case BattleBuffKind.CrossCounter:
                return new BBCrossCounter();
            case BattleBuffKind.Care:
                return new BBCare();
            case BattleBuffKind.DivineBlessing:
                return new BBDivineBlessing();
            case BattleBuffKind.HeltyBody:
                return new BBHeltyBody();
            case BattleBuffKind.Leech:
                return new BBLeech();
            case BattleBuffKind.Poisoned:
                return new BBPoisoned();
            case BattleBuffKind.Salvation:
                return new BBSalvation();
            case BattleBuffKind.SelfDestructReady:
                return new BBSelfDestructReady();
            case BattleBuffKind.TacticalTaunt:
                return new BBTacticalTaunt();
            case BattleBuffKind.Taunt:
                return new BBTaunt();
            case BattleBuffKind.TouchOfSalvation:
                return new BBTouchOfSalvation();
            default:
                return null; // 또는 적절한 기본 반환값
        }
    }
}
public abstract class SuddenEvent
{
    #region Protected
    public abstract string name { get; }
    public abstract string description { get; }
    public abstract bool isPositive { get; }
    public abstract bool isNegative { get; }
    public abstract bool isAvoidable { get; }
    public abstract int costMoney { get; }
    public abstract int costMental { get; }
    public abstract int costStemina { get; }
    public abstract int rewardMoney { get; }
    public abstract int rewardMental { get; }
    public abstract int rewardStemina { get; }
    public abstract int rewardExp { get; }

    public abstract int weightValue { get; }
    #endregion Protected
    public abstract void AgreeEvent(InGameManager inGameManager);
    public abstract void RejectEvent(InGameManager inGameManager);
    public bool isSameKind(SuddenEventKind suddenEventKind)
    {
        return suddenEventKind == GetKind();
    }
    public abstract SuddenEventKind GetKind();
}
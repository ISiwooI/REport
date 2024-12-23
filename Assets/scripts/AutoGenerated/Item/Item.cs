[System.Serializable]
public abstract class Item
{
    public abstract string name { get; }
    public abstract string simpleDescription { get; }
    public abstract string[] logs { get; }
    public abstract int price { get; }
    public abstract ItemKind itemKind { get; }
    #region runtime
    public int itemCount;
    #endregion runtime
    public abstract void UseItem(InGameManager inGameManager);
}
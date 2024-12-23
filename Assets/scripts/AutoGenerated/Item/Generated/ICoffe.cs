//커피 
[System.Serializable]
public class ICoffe : Item
{
    public override string name => "커피";

    public override string simpleDescription => "일정 시간 지침 효과를 받지 않는다.\n\"정신이 번쩍 카페인 덩어리\"";

    public override string[] logs => new[] { "커피를 마셔 정신이 맑아졌다." };

    public override int price => 50;

    public override ItemKind itemKind => ItemKind.coffe;

    public override void UseItem(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.GetBuff(BuffKind.awakening);
        itemCount--;
    }
}
//스트레스 감소
[System.Serializable]
public class IRabitDoll : Item
{
    public override string name => "토끼 인형";

    public override string simpleDescription => "멘탈을 일정량 회복한다.\n\"때리기 좋아보이는 토끼 인형.\"";

    public override string[] logs => new[]{
        "토끼 인형을 때리니 기분이 조금 괜찮아졌다.","인형이 망가졌다."
    };

    public override int price => 200;
    public override ItemKind itemKind => ItemKind.rabitDoll;
    public override void UseItem(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.MentalGain(50);
        if (Utill.random.Next(100) < 30)
        {
            inGameManager.InGameLog(logs[1]);
            itemCount--;
        }
    }
}
[System.Serializable]
public class BEmotionalWellBeing : Buff
{
    public BEmotionalWellBeing()
    {
        _logs = new string[]{
            "건강한 마음가짐으로 스트레스가 감소했다."
        };
    }
    public override void OnDayLeft(InGameManager inGameManager)
    {
        inGameManager.ReleseBuff(GetKind());
    }
    public override void OnUseMental(InGameManager inGameManager, int mental)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.MentalGain(mental / 4);
    }
    public override string name => "마음의 양식";

    public override string simpleDescription => "멘탈 소모 감소.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 0;//하루 지속

    public override bool canStack => false;

    public override int maxStack => 1;

    public override BuffKind GetKind()
    {
        return BuffKind.emotionalWellBeing;
    }
}
using System.Collections;
/*
, 일정 시간 멘탈 소모 감소(마음의 양식 버프)
*/
public class AReadBook : Activity
{
    public override string simpleDescription => "책을 읽는다\n\"독서는 마음의 양식.\"";

    public override string name => "독서";

    public override int costMoney => 0;

    public override int costMental => 0;

    public override int costStamina => 0;

    public override int rewardMoney => 0;

    public override int rewardMental => 100;

    public override int rewardStamina => 0;

    public override int rewardExp => 0;

    public override int timeDuration => 1;

    public override ActivityKind Kind => ActivityKind.ReadBook;
    public AReadBook()
    {
        _logs = new[]{
            "도서관에 가서 책을 읽었다.",
            "책에 쓰여있는 글귀가 마음에 안정을 가져온다."
        };
    }


    public override IEnumerator ActivityCoroutine(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override void DoActivity(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.TimeLeft(timeDuration);
        inGameManager.InGameLog(logs[1]);
        inGameManager.MentalGain(rewardMental);
        inGameManager.GetBuff(BuffKind.emotionalWellBeing);

    }


}
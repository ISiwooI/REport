using System.Collections;
/*
    랜덤 아르바이트 획득.
*/
public class AJobSearch : Activity
{
    public override string simpleDescription => "아르바이트 자리를 알아본다.\n\"돈이 최고야!\"";
    public override string name => "구직 활동";
    public override int costMoney => 0;
    public override int costMental => 50;
    public override int costStamina => 5;
    public override int rewardMoney => 0;
    public override int rewardMental => 0;
    public override int rewardStamina => 0;
    public override int rewardExp => 0;
    public override int timeDuration => 0;
    public AJobSearch()
    {
        _logs = new string[]{
            "구직 사이트에 접속했다.",
            "나쁘지 않은 조건의 아르바이트를 찾았다.",
            "아르바이트를 목록에 추가했다.",
            "아쉽지만 조건이 마음에 들지 않아 포기했다."
        };
    }
    public override ActivityKind Kind => ActivityKind.JobSearch;
    public override IEnumerator ActivityCoroutine(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override void DoActivity(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.MentalDamage(50);
        inGameManager.StaminaDamage(5);
        inGameManager.InGameLog(logs[2]);
        inGameManager.selectableSchedule.Add(inGameManager.scheduleManager.GetRandomPartTime());

    }


}
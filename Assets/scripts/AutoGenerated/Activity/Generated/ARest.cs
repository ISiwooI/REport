using System.Collections;
/*
시간 짧게 소모, 체력 소량 회복
*/
public class ARest : Activity
{
    public override string simpleDescription => "짧은 시간 휴식을 가진다\n\"적절한 휴식은 중요!\"";

    public override string name => "휴식";

    public override int costMoney => 0;

    public override int costMental => 0;

    public override int costStamina => 0;

    public override int rewardMoney => 0;

    public override int rewardMental => 30;

    public override int rewardStamina => 20;

    public override int rewardExp => 0;

    public override int timeDuration => 1;

    public override ActivityKind Kind => ActivityKind.Rest;

    public ARest()
    {
        _logs = new[]{
            "한적한 곳에서 잠시 휴식했다.",
            "기분이 산뜻해졌다."
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
        inGameManager.StaminaGain(rewardStamina);

    }


}
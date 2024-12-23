using System.Collections;
/*

*/
public class ASleep : Activity
{
    public override string simpleDescription => "느긋하게 잠을 청한다.\n\"우주같이 넓은 공강시간 가지는 꿀같은 수면\"";

    public override string name => "수면";

    public override int costMoney => 0;

    public override int costMental => 0;

    public override int costStamina => 0;

    public override int rewardMoney => 0;

    public override int rewardMental => 80;

    public override int rewardStamina => 100;

    public override int rewardExp => 0;

    public override int timeDuration => 3;


    public override ActivityKind Kind => ActivityKind.Sleep;
    public ASleep()
    {
        _logs = new[]{
            "느긋하게 잠을 청했다.",
            "기분 좋게 잠을 자고 나니 몸이 한결 가벼워졌다."
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
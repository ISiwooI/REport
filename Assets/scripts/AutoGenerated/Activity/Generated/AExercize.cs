using System.Collections;
/*
스테미나 대량 소모, 멘탈 회복, 다음날까지 후휴증 획득, 1주간 건강함 획득(스테 소모 감소)
*/
public class AExercize : Activity
{
    public override string simpleDescription => "헬스장에 가서 운동을 한다.\n\"주기적인 운동은 건강의 비결!\"";

    public override string name => "운동";

    public override int costMoney => 300;

    public override int costMental => 0;

    public override int costStamina => 40;

    public override int rewardMoney => 0;

    public override int rewardMental => 100;

    public override int rewardStamina => 0;

    public override int rewardExp => 0;

    public override int timeDuration => 1;
    public AExercize()
    {
        _logs = new[]{
            "헬스장에 가 운동을 시작했다.",
            "몸과 마음이 건강해지는 기분이 든다.",
             "엄청난 후휴증이 몸을 맴돈다.",
              "약간의 후휴증이 몸을 맴돈다.",
              "몸을 움직이니 머리가 상쾌해진다."
        };
    }
    public override ActivityKind Kind => ActivityKind.Exercize;

    public override IEnumerator ActivityCoroutine(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override void DoActivity(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.MoneyUse(costMoney);
        inGameManager.TimeLeft(timeDuration);
        inGameManager.StaminaDamage(costStamina);
        inGameManager.InGameLog(logs[4]);
        inGameManager.MentalGain(rewardMental);

        inGameManager.GetBuff(BuffKind.musclePain, 3);

    }


}
using System.Collections;
/*
허기 디버프 제거
허기: 멘탈 지속 감소, 스테미나 소모 증가
*/
public class AMeal : Activity
{
    public override string simpleDescription => "식사를 해 허기를 때운다.\n\"금강산도 식후경.\"";

    public override string name => "식사";

    public override int costMoney => 200;

    public override int costMental => 0;

    public override int costStamina => 0;

    public override int rewardMoney => 0;

    public override int rewardMental => 100;

    public override int rewardStamina => 10;

    public override int rewardExp => 0;

    public override int timeDuration => 1;

    public override ActivityKind Kind => ActivityKind.Meal;

    public AMeal()
    {
        _logs = new[]{
            "만족스러운 포만감이 뱃속을 가득 채운다"
        };
    }
    public override IEnumerator ActivityCoroutine(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override void DoActivity(InGameManager inGameManager)
    {
        inGameManager.TimeLeft(timeDuration);
        inGameManager.InGameLog(logs[0]);
        inGameManager.MoneyUse(costMoney);
        inGameManager.StaminaGain(rewardStamina);
        inGameManager.MentalGain(rewardMental);
        if (inGameManager.player.activatedBuffs.ContainsKey(BuffKind.hungry))
        {
            if (inGameManager.player.activatedBuffs[BuffKind.hungry].stack > 8)
            {
                inGameManager.GetBuff(BuffKind.hungry, inGameManager.player.activatedBuffs[BuffKind.hungry].stack - 8);
            }
            else inGameManager.ReleseBuff(BuffKind.hungry);
        }

    }

}
using System.Collections;
/*
아르바이트
*/
public class APartTime : Activity
{
    public override string simpleDescription => "일정에 등록된 아르바이트를 수행한다.";

    public override string name => variableName;

    public override int costMoney => variableCostMoney;

    public override int costMental => variableCostMental;

    public override int costStamina => variableCostStamina;

    public override int rewardMoney => variableRewardMoney;

    public override int rewardMental => variableRewardMental;

    public override int rewardStamina => variableRewardStamina;

    public override int rewardExp => variableRewardExp;

    public override int timeDuration => variableTimeDuration;


    public override ActivityKind Kind => ActivityKind.PartTime;

    string variableName = "아르바이트";
    int variableCostMoney = 0;
    int variableCostMental = 0;
    int variableCostStamina = 0;
    int variableTimeDuration = 0;
    int variableRewardMoney = 0;
    int variableRewardMental = 0;
    int variableRewardStamina = 0;
    int variableRewardExp = 0;

    public APartTime()
    {
        _logs = new[]{
            "아르바이트를 무사히 마쳤다.","지친 몸과 마음이 무겁다."
        };
    }

    public override IEnumerator ActivityCoroutine(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override void DoActivity(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.TimeLeft(variableTimeDuration);
        inGameManager.MoneyGain(variableRewardMoney);
        inGameManager.InGameLog(logs[1]);
        inGameManager.StaminaDamage(variableCostStamina);
        inGameManager.MentalDamage(variableCostMental);

    }

    public override bool UpdateSelectable(InGameManager inGameManager)
    {
        if (
                    inGameManager.scheduleManager.IsCanActivity(inGameManager.player.GetWeekday, inGameManager.player.GetHour) &&
                    inGameManager.scheduleManager.IsPartTime(inGameManager.player.GetWeekday, inGameManager.player.GetHour))
        {
            variableName = inGameManager.scheduleManager.GetPartTime(inGameManager.player.GetWeekday, inGameManager.player.GetHour).name;
            variableCostMental = inGameManager.scheduleManager.GetPartTime(inGameManager.player.GetWeekday, inGameManager.player.GetHour).costMental;
            variableCostStamina = inGameManager.scheduleManager.GetPartTime(inGameManager.player.GetWeekday, inGameManager.player.GetHour).costStemina;
            variableTimeDuration = inGameManager.scheduleManager.GetPartTime(inGameManager.player.GetWeekday, inGameManager.player.GetHour).scheduleTime.timeDuration;
            variableName = inGameManager.scheduleManager.GetPartTime(inGameManager.player.GetWeekday, inGameManager.player.GetHour).name;
            this.isSelectable = true;
            return true;
        }
        isSelectable = false;
        return false;
    }
}
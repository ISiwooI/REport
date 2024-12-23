using System.Collections;
/*
과제 디버프 제거, 경험치 획득
*/
public class AReport : Activity
{
    public override string simpleDescription => "강의중 나온 과제를 수행한다.\n\"오오..과제..나의 오랜 친구..\"";

    public override string name => "과제 수행";

    public override int costMoney => 0;

    public override int costMental => 100;

    public override int costStamina => 10;

    public override int rewardMoney => 0;

    public override int rewardMental => 0;

    public override int rewardStamina => 0;

    public override int rewardExp => 0;

    public override int timeDuration => 2;

    public override ActivityKind Kind => ActivityKind.Report;

    public AReport()
    {
        _logs = new[]{
            "과제를 무사히 끝마쳤다."
        };
    }

    public override IEnumerator ActivityCoroutine(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override void DoActivity(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        if (inGameManager.player.activatedBuffs[BuffKind.report] is null) return;
        inGameManager.TimeLeft(timeDuration);
        int s = inGameManager.player.activatedBuffs[BuffKind.report].stack;
        if (s == 1) inGameManager.ReleseBuff(BuffKind.report);
        else inGameManager.GetBuff(BuffKind.report, s - 1);

    }

    public override bool UpdateSelectable(InGameManager inGameManager)
    {
        if (!base.UpdateSelectable(inGameManager))
        {
            return false;
        }
        if (inGameManager.player.activatedBuffs.ContainsKey(BuffKind.report))
        {
            isSelectable = true;
            return true;
        }
        isSelectable = false;
        return false;

    }
}
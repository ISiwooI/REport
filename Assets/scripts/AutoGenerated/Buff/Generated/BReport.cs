using System.Collections.Generic;
[System.Serializable]
public class BReport : Buff
{
    public BReport()
    {
        _logs = new[]{
            "과제 마감일이 지나버렸다..",
            "과제를 하나 끝마쳤다."
        };
    }
    public override void OnStackBuff(InGameManager inGameManager, BuffKind buffKind, int stack)
    {
        if (buffKind != BuffKind.report) return;
        int sduration = stack - this.stack;
        if (sduration > 0)
        {
            for (int i = 0; i < sduration; i++)
            {
                deadlines.Add(7);
            }
        }
        if (sduration < 0)
        {
            for (int i = 0; i < -sduration; i++)
            {
                if (deadlines.Count > 0)
                    deadlines.RemoveAt(0);
                inGameManager.InGameLog(logs[1]);
            }
        }
    }
    public override void OnGetBuff(InGameManager inGameManager, BuffKind buffKind)
    {
        for (int i = 0; i < stack; i++)
        {
            deadlines.Add(7);
        }
    }
    public override void OnDayLeft(InGameManager inGameManager)
    {
        base.OnDayLeft(inGameManager);
        List<int> temp = new List<int>();

        foreach (int deadline in deadlines)
        {
            temp.Add(deadline - 1);
        }
        temp.Sort();
        deadlines = temp;
        while (deadlines[0] <= 0 && deadlines.Count > 0)
        {
            inGameManager.InGameLog(logs[0]);
            inGameManager.MentalDamage(300);
            deadlines.Remove(deadlines[0]);
            stack--;
        }
        if (deadlines.Count <= 0)
        {
            inGameManager.ReleseBuff(BuffKind.report);
        }
    }
    public override string name => "과제";

    public override string simpleDescription => $"마감일에 맞추지 못하면 후회할 일이 생길 것 같다..\n중첩: {stack}\n남은 마감일 :{deadlines[0]}일";

    public override bool isPositive => false;

    public override bool isNegative => true;

    public override int timeDuration => 0;

    public override bool canStack => true;

    public override int maxStack => 10;
    public List<int> deadlines = new List<int>();
    public override BuffKind GetKind()
    {
        return BuffKind.report;
    }
}
//과제 중첩 감소
[System.Serializable]
public class ISolution : Item
{
    public override string name => "답안지";

    public override string simpleDescription => "과제를 1중첩 감소시킨다.\n\"답안만큼 빠른 방법은 없다.\"";

    public override string[] logs => new[]{
        "답안지를 참고해 빠르게 과제를 끝마쳤다.",
        "진행할 과제가 없다"
    };

    public override int price => 50;
    public override ItemKind itemKind => ItemKind.solution;
    public override void UseItem(InGameManager inGameManager)
    {
        if (!inGameManager.player.activatedBuffs.ContainsKey(BuffKind.report))
        {
            inGameManager.InGameLog(logs[1]);
            return;
        }
        inGameManager.InGameLog(logs[0]);
        int s = inGameManager.player.activatedBuffs[BuffKind.report].stack;
        if (s == 1) inGameManager.ReleseBuff(BuffKind.report);
        else inGameManager.GetBuff(BuffKind.report, s - 1);
        itemCount--;
        return;
    }
}
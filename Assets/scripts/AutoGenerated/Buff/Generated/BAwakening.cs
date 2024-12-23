[System.Serializable]
public class BAwakening : Buff
{
    public override string name => "각성";

    public override void OnGetBuff(InGameManager inGameManager, BuffKind buffKind)
    {
        if (buffKind == BuffKind.awakening)
        {
            leftDuration = timeDuration;
        }
    }
    public override void OnOneHourLeft(InGameManager inGameManager)
    {
        leftDuration--;
        if (leftDuration <= 0) inGameManager.ReleseBuff(GetKind());
    }
    public override string simpleDescription => $"카페인 섭취로 인해 각성했다.\n지침 효과를 받지 않는다.";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 3;

    public override bool canStack => false;

    public override int maxStack => 1;

    public override BuffKind GetKind()
    {
        return BuffKind.awakening;
    }
}
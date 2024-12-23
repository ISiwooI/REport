[System.Serializable]
public class BHungry : Buff
{
    public BHungry()
    {
        _logs = new[]{
        "약간의 배고픔이 느껴진다.",
        "배고픔이 강하게 느껴진다.",
        "심각한 수준의 공복감에 고통을 느낀다."
        };
    }
    public override void OnOneHourLeft(InGameManager inGameManager)
    {
        inGameManager.GetBuff(BuffKind.hungry, stack + 1);
        switch (stack)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                inGameManager.InGameLog(logs[0]);
                inGameManager.MentalDamage(15);
                inGameManager.StaminaDamage(5);
                break;
            case 5:
            case 6:
            case 7:
            case 8:
                inGameManager.InGameLog(logs[1]);
                inGameManager.MentalDamage(100);
                inGameManager.StaminaDamage(30);
                break;
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
                inGameManager.InGameLog(logs[2]);
                inGameManager.MentalDamage(400);
                inGameManager.StaminaDamage(60);
                break;
            default:
                inGameManager.InGameLog(logs[2]);
                inGameManager.MentalDamage(400);
                inGameManager.StaminaDamage(120);
                break;
        }
    }
    public override string name => "허기";

    public override string simpleDescription => $"매 시간 체력과 멘탈을 잃는다.\n 중첩: {stack}";

    public override bool isPositive => false;

    public override bool isNegative => true;

    public override int timeDuration => 0;//식사시 제거

    public override bool canStack => true;

    public override int maxStack => 24;

    public override BuffKind GetKind()
    {
        return BuffKind.hungry;
    }
}
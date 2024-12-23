[System.Serializable]
public class BMusclePain : Buff
{
    public BMusclePain()
    {
        _logs = new[]{
            "근섬유 하나하나가 비명을 지른다.",
            "온몸이 뻐근해 평소보다 깊은 피로를 느낀다.",
            "평소보다 피곤하지만 견딜만 하다.",
            "근육통이 조금 줄어들었다.",
            "근육통이 사라졌다."
        };
    }
    public override void OnUseStamina(InGameManager inGameManager, int stamina)
    {
        switch (stack)
        {
            case 1:
                inGameManager.InGameLog(logs[2]);
                inGameManager.StaminaDamage(stamina / 2, false);
                break;
            case 2:
                inGameManager.InGameLog(logs[1]);
                inGameManager.StaminaDamage(stamina / 3, false);
                break;
            case 3:
                inGameManager.InGameLog(logs[0]);
                inGameManager.StaminaDamage(stamina / 4, false);
                break;
        }
    }
    public override void OnDayLeft(InGameManager inGameManager)
    {
        if (stack == 1)
        {
            inGameManager.InGameLog(logs[4]);
            inGameManager.ReleseBuff(GetKind());
            inGameManager.GetBuff(BuffKind.healthyBody);
        }
        else
        {
            inGameManager.InGameLog(logs[3]);
            inGameManager.GetBuff(GetKind(), stack - 1);
        }
    }
    public override string name => "근육통";

    public override string simpleDescription => $"체력 소모가 증가한다.\n중첩: {stack}";

    public override bool isPositive => false;

    public override bool isNegative => true;

    public override int timeDuration => 3;//3일 지속

    public override bool canStack => true;

    public override int maxStack => 3;

    public override BuffKind GetKind()
    {
        return BuffKind.musclePain;
    }
}
[System.Serializable]
public class BHealthyBody : Buff
{
    public BHealthyBody()
    {
        _logs = new[]{
            "건강한 육체로 인해 체력 소모가 감소했다.",
            "철저한 자기관리로 근육통이 감소했다."
        };
    }

    //근육통 감소
    public override void OnGetBuff(InGameManager inGameManager, BuffKind buffKind)
    {
        if (buffKind == BuffKind.musclePain)
        {
            inGameManager.InGameLog(logs[1]);
            inGameManager.GetBuff(BuffKind.musclePain, inGameManager.player.activatedBuffs[BuffKind.musclePain].stack - 1);
        }
        else if (buffKind == BuffKind.healthyBody)
        {
            leftDuration = 7;
        }
    }
    public override void OnUseStamina(InGameManager inGameManager, int Stamina)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.StaminaGain(Stamina / 4, false);
    }
    public override void OnDayLeft(InGameManager inGameManager)
    {
        leftDuration--;
        if (leftDuration <= 0)
        {
            inGameManager.ReleseBuff(GetKind());
        }
    }
    public override string name => "건강한 육체";

    public override string simpleDescription => $"체력 소모 감소.\n 남은 시간: {leftDuration}일";

    public override bool isPositive => true;

    public override bool isNegative => false;

    public override int timeDuration => 7;//7일

    public override bool canStack => false;

    public override int maxStack => 1;

    public override BuffKind GetKind()
    {
        return BuffKind.healthyBody;
    }
}

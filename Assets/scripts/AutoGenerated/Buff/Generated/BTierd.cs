[System.Serializable]
public class BTired : Buff
{
    public BTired()
    {
        _logs = new[]{
            "누적된 피로가 스트레스가 되어 돌아온다.",
            "당장이라도 쓰러질 것만 같다.."
        };
    }
    public override void OnUseStamina(InGameManager inGameManager, int Stamina)
    {
        if (inGameManager.player.activatedBuffs.ContainsKey(BuffKind.awakening))
        {
            inGameManager.InGameLog("각성 효과로 멘탈을 소모하지 않았다.");
            return;
        }
        switch (stack)
        {
            case 1:
                inGameManager.InGameLog(logs[0]);
                inGameManager.MentalDamage((int)(Stamina * 1.5f));
                break;
            case 2:
                inGameManager.InGameLog(logs[1]);
                inGameManager.MentalDamage(Stamina * 5);
                break;

        }
    }
    public override string name => "지침";

    public override string simpleDescription => $"체력을 소모할때 추가로 멘탈을 소모한다.\n중첩: {stack}";

    public override bool isPositive => false;

    public override bool isNegative => true;

    public override int timeDuration => 0;//멘탈 회복시 삭제

    public override bool canStack => true;

    public override int maxStack => 2;

    public override BuffKind GetKind()
    {
        return BuffKind.tierd;
    }
}
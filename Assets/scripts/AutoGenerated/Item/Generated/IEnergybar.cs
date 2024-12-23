//허기 감소 스테미나 회복
[System.Serializable]
public class IEnergybar : Item
{
    public override string name => "에너지 바";

    public override string simpleDescription => "허기가 줄어들고, 스테미나를 회복한다.\n\"칼로리 덩어리 에너지 바\"";

    public override string[] logs => new[]{
        "에너지 바를 먹어 허기가 약간 해소되었다.",
    };

    public override int price => 10;
    public override ItemKind itemKind => ItemKind.energybar;
    public override void UseItem(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        if (inGameManager.player.activatedBuffs.ContainsKey(BuffKind.hungry))
        {
            if (inGameManager.player.activatedBuffs[BuffKind.hungry].stack > 1)
            {
                inGameManager.GetBuff(BuffKind.hungry, inGameManager.player.activatedBuffs[BuffKind.hungry].stack - 1);
            }
            else inGameManager.ReleseBuff(BuffKind.hungry);
        }
        inGameManager.StaminaGain(10);
        itemCount--;
    }
}
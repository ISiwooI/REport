using UnityEngine;
[System.Serializable]
public abstract class Buff
{
    public abstract string name { get; }
    public abstract string simpleDescription { get; }
    public abstract bool isPositive { get; }
    public abstract bool isNegative { get; }
    public abstract int timeDuration { get; }
    public abstract bool canStack { get; }
    public abstract int maxStack { get; }

    public string[] logs => _logs;
    protected string[] _logs;
    //얼마나 지속할지
    #region runtime
    public int leftDuration;
    protected int _stack;

    public int stack
    {
        get { return _stack; }
        set
        {
            if (canStack)
            {
                _stack = maxStack < value ? maxStack : value;
            }
            else
            {
                _stack = 1;
            };
        }
    }
    #endregion runtime
    public void GetBuff(InGameManager inGameManager, int stack = 1)
    {
        inGameManager.player.activatedBuffs.Add(GetKind(), this);
        AddListener(inGameManager);
        this.stack = stack;
    }
    public void ReleseBuff(InGameManager inGameManager)
    {
        if (inGameManager.player.activatedBuffs.ContainsKey(GetKind()))
        {
            inGameManager.player.activatedBuffs.Remove(GetKind());
            ReleseListener(inGameManager);
        }

    }
    public abstract BuffKind GetKind();
    public void AddListener(InGameManager inGameManager)
    {
        inGameManager.OnUseMoney += OnUseMoney;
        inGameManager.OnUseMental += OnUseMental;
        inGameManager.OnUseStamina += OnUseStamina;
        inGameManager.OnGainMoney += OnGainMoney;
        inGameManager.OnGainMental += OnGainMental;
        inGameManager.OnGainStamina += OnGainStamina;
        inGameManager.OnGetBuff += OnGetBuff;
        inGameManager.OnStackBuff += OnStackBuff;
        inGameManager.OnReleseBuff += OnReleseBuff;
        inGameManager.OnDoActivity += OnDoActivity;
        inGameManager.OnOneHourLeft += OnOneHourLeft;
        inGameManager.OnDayLeft += OnDayLeft;
        inGameManager.OnWeekLeft += OnWeekLeft;
        inGameManager.OnBuyItem += OnBuyItem;
        inGameManager.OnGetItem += OnGetItem;
        inGameManager.OnUseItem += OnUseItem;

    }
    public void ReleseListener(InGameManager inGameManager)
    {
        inGameManager.OnUseMoney -= OnUseMoney;
        inGameManager.OnUseMental -= OnUseMental;
        inGameManager.OnUseStamina -= OnUseStamina;
        inGameManager.OnGainMoney -= OnGainMoney;
        inGameManager.OnGainMental -= OnGainMental;
        inGameManager.OnGainStamina -= OnGainStamina;
        inGameManager.OnGetBuff -= OnGetBuff;
        inGameManager.OnStackBuff -= OnStackBuff;
        inGameManager.OnReleseBuff -= OnReleseBuff;
        inGameManager.OnDoActivity -= OnDoActivity;
        inGameManager.OnOneHourLeft -= OnOneHourLeft;
        inGameManager.OnDayLeft -= OnDayLeft;
        inGameManager.OnWeekLeft -= OnWeekLeft;
        inGameManager.OnBuyItem -= OnBuyItem;
        inGameManager.OnGetItem -= OnGetItem;
        inGameManager.OnUseItem -= OnUseItem;
    }
    #region Listener
    //자원 소모
    public virtual void OnUseMoney(InGameManager inGameManager, int money) { }
    public virtual void OnUseMental(InGameManager inGameManager, int mental) { }
    public virtual void OnUseStamina(InGameManager inGameManager, int Stamina) { }
    //자원 회복
    public virtual void OnGainExp(InGameManager inGameManager, int exp) { }
    public virtual void OnGainMoney(InGameManager inGameManager, int money) { }
    public virtual void OnGainMental(InGameManager inGameManager, int mental) { }
    public virtual void OnGainStamina(InGameManager inGameManager, int Stamina) { }

    //버프 시작
    public virtual void OnGetBuff(InGameManager inGameManager, BuffKind buffKind) { }
    public virtual void OnStackBuff(InGameManager inGameManager, BuffKind buffKind, int stack) { }
    //버프 끝
    public virtual void OnReleseBuff(InGameManager inGameManager, BuffKind buffkind) { }
    //턴 시작
    public virtual void OnTurnStart(InGameManager inGameManager) { }
    //턴 끝
    public virtual void OnTurnEnd(InGameManager inGameManager) { }

    public virtual void OnDoActivity(InGameManager inGameManager, ActivityKind activity) { }
    public virtual void OnOneHourLeft(InGameManager inGameManager) { }
    public virtual void OnDayLeft(InGameManager inGameManager) { }
    public virtual void OnWeekLeft(InGameManager inGameManager) { }
    public virtual void OnBuyItem(InGameManager inGameManager, ItemKind item) { }
    public virtual void OnGetItem(InGameManager inGameManager, ItemKind item) { }
    public virtual void OnUseItem(InGameManager inGameManager, ItemKind item) { }
    public virtual void OnStartBattle(InGameManager inGameManager) { }
    public virtual void OnLevelUp(InGameManager inGameManager) { }
    public virtual void OnGameOver(InGameManager inGameManager) { }
    #endregion Listener

}
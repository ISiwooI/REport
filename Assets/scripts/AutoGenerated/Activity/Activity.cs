using System.Collections;
using TMPro;
using Unity.VisualScripting;


public abstract class Activity
{
    #region protected
    //todo 소모 자원과 획득 자원 분리
    public abstract string simpleDescription { get; }
    public abstract string name { get; }
    public abstract int costMoney { get; }
    public abstract int costMental { get; }
    public abstract int costStamina { get; }
    public abstract int rewardMoney { get; }
    public abstract int rewardMental { get; }
    public abstract int rewardStamina { get; }
    public abstract int rewardExp { get; }
    public abstract int timeDuration { get; }
    public string[] logs => _logs;
    protected string[] _logs;
    #endregion protected
    #region calculated
    public bool isSelectable = false;
    #endregion calculated
    public virtual bool UpdateSelectable(InGameManager inGameManager)
    {
        if (inGameManager.player.money < costMoney)
        {
            isSelectable = false;
            return false;
        }
        if (inGameManager.player.GetHour + timeDuration > 24)
        {
            isSelectable = false;
            return false;
        }
        for (int i = 0; i < timeDuration; i++)
        {
            if (inGameManager.scheduleManager.IsBusy(inGameManager.player.GetWeekday, inGameManager.player.GetHour + i) == true)
            {
                isSelectable = false;
                return false;
            }
        }

        isSelectable = true;
        return true;
    }
    public abstract void DoActivity(InGameManager inGameManager);
    public abstract ActivityKind Kind { get; }
    public bool isSameKind(ActivityKind activityKind) { return activityKind == this.Kind; }
    public abstract IEnumerator ActivityCoroutine(InGameManager inGameManager);

}

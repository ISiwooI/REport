using System;
using System.Collections;
using UnityEngine;
[System.Serializable]
public abstract class BattleBuff
{
    /*
    고찰--
    간단한 버프, 디버프를 제작하는 데에는 무리가 없지만 보편적인 효과(기절, 도발 기타등등)을
    지속시간이나 해제 조건을 자유롭게 바꿔서 적용하는 데에는 무리가 있다. 즉 범용성에는 문제가 없으나 코드 재사용성이 떨어진다.
    자주 사용할 버프, 디버프는 모듈화를 진행해서 제작하는 것도 방법일듯.
    고찰 끝-------
    */
    public abstract string name { get; }
    public abstract string simpleDescription { get; }
    public abstract bool isPositive { get; }
    public abstract bool isNegative { get; }
    public abstract int timeDuration { get; }
    public abstract bool canMultiple { get; }
    public abstract bool canStack { get; }
    public abstract int maxStack { get; }
    public abstract bool needTarget { get; }
    public BattleActor target;
    public abstract BattleBuffKind kind { get; }
    //얼마나 지속할지
    #region runtime
    public int leftDuration;
    protected int _stack;
    public string buffInfo = "";
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
    public override string ToString()
    {
        string result = $"{name}\n{simpleDescription}\n지속: {leftDuration}";

        if (canStack)
        {
            result += $"\n중첩: {stack}";
        }
        return result;
    }
    public BattleBuff()
    {
        leftDuration = timeDuration;
    }
    public void AddListener(BattleActor actor)
    {
        actor.OnPhaseStart += OnPhaseStart;
        actor.OnStatCalculate += OnStatCalculate;
        actor.OnTurnStart += OnTurnStart;
        actor.OnSkill += OnSkill;
        actor.OnShield += OnShild;
        actor.OnHeal += OnHeal;
        actor.OnAttacked += OnAttacked;
        actor.OnAttack += OnAttack;
        actor.OnTurnEnd += OnTurnEnd;
        actor.OnPhaseEnd += OnPhaseEnd;
        actor.OnUpdateTargetableActor += OnUpdateTargetableActor;
    }
    public void ReleseListener(BattleActor actor)
    {
        actor.OnPhaseStart -= OnPhaseStart;
        actor.OnStatCalculate -= OnStatCalculate;
        actor.OnTurnStart -= OnTurnStart;
        actor.OnSkill -= OnSkill;
        actor.OnShield -= OnShild;
        actor.OnHeal -= OnHeal;
        actor.OnAttacked -= OnAttacked;
        actor.OnAttack -= OnAttack;
        actor.OnTurnEnd -= OnTurnEnd;
        actor.OnPhaseEnd -= OnPhaseEnd;
        actor.OnUpdateTargetableActor -= OnUpdateTargetableActor;
    }
    public virtual IEnumerator TurnStartCoroutine(BattleManager manager, BattleActor actor)
    {
        if (manager is null)
        {
            throw new ArgumentNullException(nameof(manager));
        }

        if (actor is null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        yield break;
    }
    public virtual IEnumerator TurnEndCoroutine(BattleManager manager, BattleActor actor)
    {
        if (manager is null)
        {
            throw new ArgumentNullException(nameof(manager));
        }

        if (actor is null)
        {
            throw new ArgumentNullException(nameof(actor));
        }
        yield break;
    }
    #region Listener
    public virtual void OnUpdateTargetableActor(BattleManager manager, BattleActor self) { }
    public virtual void OnPhaseStart(BattleManager manager, BattleActor self) { }
    public virtual void OnStatCalculate(BattleManager manager, BattleActor self) { }
    public virtual void OnTurnStart(BattleManager manager, BattleActor self, BattleActor turnStartActor) { }
    public virtual void OnSkill(BattleManager manager, BattleActor self) { }
    public virtual void OnShild(BattleManager manager, BattleActor self, BattleActor caster, int amount) { }
    public virtual void OnHeal(BattleManager manager, BattleActor self, BattleActor caster, int amount) { }
    public virtual void OnAttacked(BattleManager manager, BattleActor self, BattleActor caster, int amount) { }
    public virtual void OnAttack(BattleManager manager, BattleActor self, BattleActor target, int amount) { }
    public virtual void OnTurnEnd(BattleManager manager, BattleActor self, BattleActor turnEndActor) { }
    public virtual void OnPhaseEnd(BattleManager manager, BattleActor self) { }
    #endregion Listener

}
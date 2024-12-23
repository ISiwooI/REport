using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using BattleListnenrs;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
public class Shield
{
    // 쉴드 지속 동안 효과 적용하는 버프?
    /*
    버프로 구현? 쉴드 콜백 구현?
    버프: 쉴드 적용시 버프 선언 쉴드 적용될 때 액터에 적용 후 쉴드 해제시 해제

    쉴드 콜백: 
    쉴드 적용시 콜백, 매 턴 콜백, 해제 콜백 등 개별적 콜백 생성. 작업량 많음 확장성 좋음.!! 버프 구현도 활용 가능!
    쉴드 부여할 때 람다로 적용 이걸로 하자

     battle actor 콜백 작성
    
    */
    //


    public BattleListener<bool> OnExitShield;
    public int amount = 0;
    public int duration = -1;
    public Shield(int amount, int duration = -1, BattleListener<bool> onExitShield = null)
    {
        this.duration = duration;
        this.amount = amount;
        this.OnExitShield = onExitShield;
    }

}
public abstract class BattleActor : MonoBehaviour
{
    /*
    보편적인 애니메이팅,
    hp,
    공격력,
    방어력,
    추공,
    추방,
    쉴드(좀 복잡한 구현),
    버프, 디버프
    사망

    턴 시작
    콜백(스텟계산)
    스킬 사용
    콜백(즉시 적용)
    석별, 무적=> 피해 감소율 활용?
    콜백들(추가 효과들)battle manager 추가 효과 대기열에 행동 예약
    죽은 액터 처리
    턴 종료
    콜백?
    */
    #region Listener
    public BattleListener OnPhaseStart;
    public BattleListener OnStatCalculate;
    public BattleListener OnUpdateTargetableActor;
    public BattleListener<BattleActor> OnTurnStart;//코루틴 추가
    public BattleListener OnSkill;
    public BattleListener<BattleActor, int> OnShield;
    public BattleListener<BattleActor, int> OnHeal;
    public BattleListener<BattleActor, int> OnAttacked;
    public BattleListener<BattleActor, int> OnAttack;
    public BattleListener<BattleActor> OnTurnEnd;//코루틴 추가
    public BattleListener OnPhaseEnd;
    #endregion Listener
    public List<BattleTaskListener> TurnStartListeners = new List<BattleTaskListener>();
    public List<BattleTaskListener> TurnEndListeners = new List<BattleTaskListener>();

    public List<BattleBuff> ActivatedBuffs = new List<BattleBuff>();

    public List<BattleActor> tauntedEnemy = new List<BattleActor>();
    public List<BattleActor> tauntedFriendly = new List<BattleActor>();

    public List<BattleActor> targetableActors = new List<BattleActor>();
    LinkedList<Shield> shields = new LinkedList<Shield>();
    public Vector3 OriginPos;
    int order = -1;

    public HPBar hpBar;
    [SerializeField] public TMP_Text orderTMP;
    [SerializeField] public Animator animator;
    [SerializeField] float nearAtkDelay = 0.3f;
    [SerializeField] float farAtkDelay = 0.3f;
    [SerializeField] public float jumpAttackDelay = 1f;
    #region stat
    [SerializeField] int _speedWeight;
    [SerializeField] int _def;
    [SerializeField] int _atk;
    [SerializeField] int _hp;
    [SerializeField] int _maxHP;

    public string actorName = "이름";
    public float damageMultiplier = 1;
    public float speedPer = 1;
    public int speedAdd = 0;
    public float defPer = 1.0f;
    public int defAdd = 0;
    public float atkPer = 1.0f;
    public int atkAdd = 0;
    public abstract bool isEnemy { get; }
    public bool hasActedThisPhase = false;
    public bool isDead { get { return hp <= 0; } }
    public bool isIdle { get { return animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"); } }
    public int speed { get { return (int)(_speedWeight * speedPer) + speedAdd; } }
    public int def { get { return (int)(_def * defPer) + defAdd; } }
    public int atk { get { return (int)(_atk * atkPer) + atkAdd; } }
    public int shieldAmount
    {
        get
        {
            int i = 0;
            foreach (Shield shield in shields)
            {
                i += shield.amount;
            }
            return i;
        }
    }
    public int hp { get { return _hp; } set { _hp = value; } }
    public int maxHP { get { return _maxHP; } }
    #endregion stat
    public override string ToString()
    {
        return $"{actorName}\n체력: {hp}/{maxHP}\n보호막: {shieldAmount}\n공격력: {atk}\n방어력: {def}\n속도: {speed}";
    }
    public string BuffsToString()
    {
        string result = "";
        foreach (BattleBuff buff in ActivatedBuffs)
        {
            result += buff.ToString();
            result += $"\n-----------------------\n";
        }
        return result;
    }
    /*
    도발 구현
    targetable 변수

    */
    //
    public virtual void UpdateHPBar()
    {
        hpBar?.SetHP((float)hp / (float)maxHP);
        hpBar?.SetShield((float)shieldAmount / (float)maxHP);
    }
    public virtual void InitActor(int atk, int def, int speed, int hp)
    {

        if (isEnemy)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        this._maxHP = hp;
        this._def = def;
        this._atk = atk;
        this._speedWeight = speed;
        this._hp = maxHP;
        CleanActor();
    }
    public void CleanActor()
    {
        hasActedThisPhase = false;
        damageMultiplier = 1;
        speedPer = 1;
        speedAdd = 0;
        defPer = 1.0f;
        defAdd = 0;
        atkPer = 1.0f;
        OnPhaseStart = null;
        OnStatCalculate = null;
        OnTurnStart = null;
        OnSkill = null;
        OnShield = null;
        OnHeal = null;
        OnAttacked = null;
        OnAttack = null;
        OnTurnEnd = null;
        OnPhaseEnd = null;
        TurnEndListeners.Clear();
        TurnStartListeners.Clear();
        ActivatedBuffs.Clear();
        tauntedEnemy.Clear();
        tauntedFriendly.Clear();
        targetableActors.Clear();
        shields.Clear();

        order = -1;
    }

    public void UpdateTargetableActors(BattleManager manager)
    {
        targetableActors.Clear();
        tauntedEnemy.Clear();
        tauntedFriendly.Clear();
        OnUpdateTargetableActor?.Invoke(manager, this);
        if (tauntedEnemy.Count() > 0)
        {
            targetableActors.AddRange(tauntedEnemy);
        }
        else
        {
            foreach (BattleActor battleActor in manager.GetAliveActors())
            {
                if (!battleActor.IsFriendly(this))
                {
                    targetableActors.Add(battleActor);
                }
            }
        }
        if (tauntedFriendly.Count() > 0)
        {
            targetableActors.AddRange(tauntedFriendly);
        }
        else
        {
            foreach (BattleActor battleActor in manager.GetAliveActors())
            {
                if (battleActor.IsFriendly(this))
                {
                    targetableActors.Add(battleActor);
                }
            }
        }
    }
    public bool IsFriendly(BattleActor actor)
    {
        return actor.isEnemy == isEnemy;
    }
    public void StatCalculate(BattleManager manager)
    {
        damageMultiplier = 1;
        speedPer = 1;
        speedAdd = 0;
        defPer = 1;
        defAdd = 0;
        atkPer = 1;
        atkAdd = 0;
        OnStatCalculate?.Invoke(manager, this);
        damageMultiplier = damageMultiplier < 0f ? 0f : damageMultiplier;
        speedPer = speedPer < 0f ? 0f : speedPer;
        defPer = defPer < 0f ? 0f : defPer;
        atkPer = atkPer < 0f ? 0f : atkPer;
    }
    public void ShieldLeft(BattleManager manager)
    {
        List<Shield> deleteShields = new List<Shield>();
        foreach (Shield s in shields)
        {
            if (s.duration > 0)
            {
                s.duration--;
                if (s.duration == 0)
                {
                    deleteShields.Add(s);
                }
            }
        }
        foreach (Shield s in deleteShields)
        {
            s.OnExitShield?.Invoke(manager, this, false);
            shields.Remove(s);
        }
        UpdateHPBar();
    }
    public void TextParticle(BattleManager manager, string value)
    {
        manager.particleManager.PrintTMPParticle(transform.position.x, transform.position.y + 3.2f, value, Color.white);
    }
    public void GetDamage(BattleManager manager, BattleActor attacker, int Damage, bool isAfterEffect = true, bool isAbsolute = false, bool playAnim = true)
    {
        if (Damage < 0) return;
        int calculatedDamage = Damage;
        if (!isAbsolute)
        {
            calculatedDamage = (int)(damageMultiplier * (Damage - this.def));
        }
        if (calculatedDamage < 0) calculatedDamage = 0;
        Damage = calculatedDamage;
        while (shields.Count > 0 && calculatedDamage > 0)
        {
            if (shields.Last().amount <= calculatedDamage)
            {
                calculatedDamage -= shields.Last().amount;
                shields.Last().OnExitShield?.Invoke(manager, this, true);
                shields.RemoveLast();

            }
            else
            {
                shields.Last().amount -= calculatedDamage;
                calculatedDamage = 0;
            }
        }
        _hp -= calculatedDamage;
        if (isAfterEffect) OnAttacked?.Invoke(manager, this, attacker, Damage);
        //숫자 이펙트 표시
        ParticleSystem ps = manager.particleManager.hitParticlepool.Get();
        ps.transform.position = new Vector3(transform.position.x, transform.position.y + 1.15f, transform.position.z);
        UpdateHPBar();
        if (isDead) hpBar.gameObject.SetActive(false);
        manager.particleManager.PrintTMPParticle(transform.position.x, transform.position.y + 3.2f, Damage.ToString(), Color.red);
        if (playAnim)
        {
            if (isDead) this.animator.SetTrigger("Dead");
            else this.animator.SetTrigger("Hit");
        }
    }
    public void GetHeal(BattleManager manager, BattleActor caster, int amount, bool isAfterEffect = true)
    {
        if (isDead) return;
        _hp += amount;
        if (_hp > _maxHP) _hp = _maxHP;
        if (isAfterEffect) OnHeal?.Invoke(manager, this, caster, amount);
        manager.particleManager.HealParticlepool.Get().transform.position = new Vector3(transform.position.x, transform.position.y + 1.15f, transform.position.z);
        manager.particleManager.PrintTMPParticle(transform.position.x, transform.position.y + 3.2f, amount.ToString(), Color.green);
        SoundManager.PlaySFX(soundKind.heal);
        UpdateHPBar();
        //숫자 이펙트 표시
    }
    public void GetShield(BattleManager manager, BattleActor caster, int amount, int duration, bool isAfterEffect = true, BattleListener<bool> onExitShield = null)
    {
        if (isDead) return;
        Shield shield = new Shield(amount, duration, onExitShield);
        if (shield.duration < 0)
            shields.AddFirst(shield);
        else shields.AddLast(shield);
        if (isAfterEffect) OnShield?.Invoke(manager, this, caster, amount);
        //숫자 이펙트 표시
        manager.particleManager.PrintTMPParticle(transform.position.x, transform.position.y + 3.2f, amount.ToString(), Color.white);
        SoundManager.PlaySFX(soundKind.shield);
        UpdateHPBar();
    }
    #region buff
    public void GetBuff(BattleManager manager, BattleBuffKind battleBuffKind, int stack, BattleActor bufftarget = null)
    {
        if (isDead) return;
        BattleBuff buff = manager.buffDeligator.GetBuff(battleBuffKind);
        buff.stack = stack;
        if (buff.isPositive)
        {
            manager.particleManager.BuffParticlepool.Get().transform.position = new Vector3(transform.position.x, transform.position.y + 1.15f, transform.position.z);
            SoundManager.PlaySFX(soundKind.buff);
        }
        if (buff.needTarget) buff.target = bufftarget;
        manager.particleManager.PrintTMPParticle(transform.position.x, transform.position.y + 3.2f, buff.name, Color.white);
        if (!buff.canMultiple && HasBuff((value) => battleBuffKind == value.kind))
        {
            if (buff.canStack)
            {
                BattleBuff currentbuff = SerchBuff((value) => battleBuffKind == value.kind);
                currentbuff.stack = stack;
                return;
            }
            else
            {
                BattleBuff currentbuff = SerchBuff((value) => battleBuffKind == value.kind);
                ReleseBuff(currentbuff);
            }
        }
        this.ActivatedBuffs.Add(buff);
        buff.AddListener(this);
        UpdateHPBar();

    }
    public BattleBuff SerchBuff(BuffCondition buffCondition)
    {
        foreach (BattleBuff battleBuff in ActivatedBuffs)
        {
            if (buffCondition(battleBuff)) return battleBuff;
        }
        return null;
    }
    public List<BattleBuff> SerchBuffs(BuffCondition buffCondition)
    {
        List<BattleBuff> list = new List<BattleBuff>();
        foreach (BattleBuff battleBuff in ActivatedBuffs)
        {
            if (buffCondition(battleBuff)) list.Add(battleBuff);
        }
        return list;
    }
    public bool HasBuff(BuffCondition buffCondition)
    {
        foreach (BattleBuff battleBuff in ActivatedBuffs)
        {
            if (buffCondition(battleBuff)) return true;
        }
        return false;
    }
    public void ReleseBuff(List<BattleBuff> buffs)
    {
        foreach (BattleBuff battleBuff in buffs)
        {
            ReleseBuff(battleBuff);
        }
    }

    public void ReleseBuff(BattleBuff battleBuff)
    {
        if (!ActivatedBuffs.Contains(battleBuff)) return;
        battleBuff.ReleseListener(this);
        ActivatedBuffs.Remove(battleBuff);
    }
    #endregion buff
    public void DisplayOrder(int i)
    {
        if (i != order) orderTMP.transform.DOShakeScale(0.7f);
        order = i;
        if (i == 0)
        {
            orderTMP.text = "v";
        }
        else if (i < 0)
        {
            orderTMP.text = "";
        }
        else
        {
            orderTMP.text = i.ToString();
        }
    }
    private void OnMouseOver()
    {

    }
    #region anim
    public IEnumerator NearAttack(Action action)
    {
        animator.SetTrigger("Near");
        yield return new WaitForSeconds(nearAtkDelay);
        action?.Invoke();
        yield return new WaitUntil(() => isIdle || isDead);
        yield break;
    }
    public IEnumerator FarAttack(Action action)
    {
        animator.SetTrigger("Magic");
        yield return new WaitForSeconds(farAtkDelay);
        action?.Invoke();
        yield return new WaitUntil(() => isIdle || isDead);
        yield break;
    }
    public IEnumerator JumpAttack(Action action, Vector3 JumpPos)
    {
        LookPosition(JumpPos);
        animator.SetTrigger("Jump");
        yield return transform.DOJump(JumpPos, 1.0f, 1, jumpAttackDelay).WaitForCompletion();
        action?.Invoke();
        yield break;
    }
    public void LookPosition(Vector3 Position)
    {
        transform.rotation = Quaternion.Euler(0, Position.x > transform.position.x ? 180 : 0, 0);
    }
    #endregion anim

}
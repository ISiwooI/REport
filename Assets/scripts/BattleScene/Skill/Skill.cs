using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Skill
{


    /*
    스킬 컨셉

    강타            smite               공격력 계수 공격            학과 포인트 소폭 회복
    응급 처치       first aid           체력 계수 회복(타인)        학과 포인트 소폭 회복
    자기 개발       self development                                학과 포인토 대폭 회복
    호신술          self defense        본인 쉴드 획득(체력 계수)   
    회복            heal                전체 회복(개인 체력 계수)

    크로스 카운터    crosscounter       피격 후 반격(방어력 계수)
    전략적 도발     tactical taunt      도발 후 쉴드, 방어력 증가
    건강한 육체     healthy body        체젠 버프 (체력 계수)
    강인한 신체     tough body          방어력 증가, 받는 피해 감소
    생기 흡수       leech               방계수 공격 후 데미지 비례 회복

    화학공학 가스탄     gas canister        전체 중독부여 받피증
    실험 실패           Experiment failed   아군 포함 광역 피해
    임상실험            Clinical trials     다음 행동까지 공격력 2배, 자원 회복
    flask가연성 플라스크     한명 폭딜           flammabilityFlask
    취급주의 화합물     범위딜              Caution compound

    자애로운 보호   mercy protect       
    구원의 손길     provocation         
    신성한 축복     Divine Blessing     
    구원과 시련     salvation and trials
    치유의 빛       healing light       
    
    자폭

    알코홀릭
    행패
    
    시각적 재새동
    하트 어택(코스트 감소)

    빛바랜 과제물
    길잃은 원념



    기본 공격, 회복, 자원 회복, 쉴드, 범위 공격, 전체 회복 
    피격 후 반격, 도발 후 쉴드, 공격 후 피흡, 체젠 버프, 받피감 획득
    범위 딜 화상 부여후 방깍, 지정 한 적 주변 딜, 한명 폭딜, 다음 행동 시 공격력 2배 자원 회복, 자해 딜+ 적에게 폭딜,방깎, 
    전체 쉴드 후 쉴드 삭제시 회복 , 전체 회복 후 방증, 5턴 석별, 대상 자원 회복 공증 방증, 다음 턴 종료시 까지 전체 무적, 종료 시 현재 채력의 30퍼센트 데미지, 공깎 
 ,   */
    /*
   ESAlcoholic
   ESCharmingHeal
   ESCharmingParalyse
   ESFadedReport
   ESRanAmuck
   ESUnStableAttack
   PSCautionChemicals
   PSClinicalTrials
   PSCrosscounter
   PSDivineBlessing
   PSExperimentFailed
   PSFirstAid
   PSFlammableFlask
   PSGasCanister
   PSHeal
   PSHealingLight
   PSHealtyBody
   PSLeech
   PSMercyProtect
   PSProvocation
   PSSalvationAndTrials
   PSSelfDefense
   PSSelfDevelopment
   PSSmite
   PSTacticalTaunt
   */
    public abstract SkillKind skillKind { get; }
    public abstract string name { get; }
    public abstract string description { get; }
    public abstract bool isTargeting { get; }
    public virtual float[] floatParams { get; }
    public virtual int[] intParams { get; }
    public abstract IEnumerator DoSkill(BattleManager battleManager, BattleActor caster, BattleActor target);
    public bool IsParametersValid(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        if (battleManager is null)
        {
            return false;
        }

        if (caster is null || caster.isDead)
        {
            return false;
        }
        if (isTargeting)
        {
            if (target == null || target.isDead)
            {
                return false;
            }
        }
        return true;
    }
    public virtual bool IsTargetable(BattleManager battleManager, BattleActor caster, BattleActor target)
    {
        return true;
    }
}

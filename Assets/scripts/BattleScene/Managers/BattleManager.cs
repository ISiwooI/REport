using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleListnenrs;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.Pool;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{/*
    적 생성,
    레벨 스케일,
    오브젝트 풀
    이펙트
    스킬 세부 구현
    카드 오브젝트
*/
    #region Pool
    [SerializeField] Transform EnemySpawnPos;
    [SerializeField] Transform[] EnemySlots;
    [SerializeField] Transform[] PlayerSlots;
    [SerializeField] Transform middlePosition;
    [SerializeField] PlayerActor PlayerPrefab;
    [SerializeField] EnemyActor CatPrefab;
    [SerializeField] EnemyActor GhostPrefab;
    [SerializeField] EnemyActor DrunkPrefab;
    [SerializeField] EnemyActor CamicalPrefab;

    ObjectPool<PlayerActor> PlayerPool;
    ObjectPool<EnemyActor> CatPool;
    ObjectPool<EnemyActor> GhostPool;
    ObjectPool<EnemyActor> DrunkPool;
    ObjectPool<EnemyActor> CamicalPool;
    #endregion Pool

    [SerializeField] Transform PlayerGroup;
    [SerializeField] Transform EnemyGroup;
    #region UI
    [SerializeField] TMP_Text[] EnemyOrder;
    [SerializeField] TMP_Text[] PlayerOrder;
    [SerializeField] Costbar costbar;
    [SerializeField] ActorDisplay CurrentTurnDisplay;
    [SerializeField] ActorDisplay SelectedActorDisplay;
    [SerializeField] TMP_Text cardInfoTMP;
    [SerializeField] HPBar[] enemyBar;
    [SerializeField] PlayerBar[] playerBar;

    #endregion UI



    [SerializeField] private StageLog stageLog;
    [SerializeField] public ParticleManager particleManager;
    [SerializeField] public CardManager cardManager;
    [SerializeField] BGLoop bg;
    List<EnemyActor> EnemyList = new List<EnemyActor>();
    List<PlayerActor> PlayerList = new List<PlayerActor>();
    List<BattleActor> Order = new List<BattleActor>();
    List<BattleActor> aliveActors = new List<BattleActor>();
    List<BattleActor> aliveEnemy = new List<BattleActor>();
    List<BattleActor> alivePlayer = new List<BattleActor>();
    public bool isWaiting = false;
    public bool skipThisTurn = false;

    int wave = 0;
    int EnemyLevel { get { return (wave + (GameManager.Inst.battleDificalty * 2)) * 15; } }
    public SkillDelegater skillDelegater = new SkillDelegater();
    public BattleBuffDelegater buffDeligator = new BattleBuffDelegater();


    public BattleActor currentTurnActor;
    BattleActor selectedActor;
    BattleActor targetActor;
    BattleActor displayActor;
    CardObject _selectedCard;
    CardObject useCard;
    public CardObject selectedCard
    {
        get { return _selectedCard; }
        set
        {
            _selectedCard = value;
            CardInfoUpdate();
        }
    }



    public int normalCost
    {
        get
        {
            return _normalCost;
        }
        set
        {
            if (value > 10) value = 10;
            if (value < 0) value = 0;
            _normalCost = value;
            costbar.SetCost(_normalCost);
        }
    }
    int _normalCost;
    /*
    콜백
    OnPhaseStart
    OnTurnStart
    OnTurnEnd
    OnDoSkill
    */

    #region UnityFunc
    private void Awake()
    {

        PlayerPool = new ObjectPool<PlayerActor>(
            () =>
            {
                PlayerActor value = GameObject.Instantiate(PlayerPrefab, PlayerGroup);
                value.CleanActor();
                return value;
            },
            (value) =>
            {
                PlayerList.Add(value);
                value.gameObject.SetActive(true);
                value.animator.SetBool("IsRunning", false);
                value.animator.SetTrigger("Idle");
                value.CleanActor();
            },
            (value) =>
            {
                value.CleanActor();
                PlayerList.Remove(value);
                value.gameObject.SetActive(false);
            },
            (value) =>
            {
                Destroy(value);
            }
        );
        DrunkPool = new ObjectPool<EnemyActor>(
            () =>
            {
                EnemyActor value = GameObject.Instantiate(DrunkPrefab, EnemyGroup);
                value.CleanActor();
                return value;
            },
            (value) =>
            {
                value.kind = EnemyKind.Drunk;
                EnemyList.Add(value);
                value.gameObject.SetActive(true);
                value.animator.SetTrigger("Idle");
                foreach (SpriteRenderer spriteRenderer in value.GetComponentsInChildren<SpriteRenderer>())
                {
                    spriteRenderer.material.color = Color.white;
                    spriteRenderer.color = Color.white;
                }
                value.CleanActor();
            },
            (value) =>
            {

                value.CleanActor();
                EnemyList.Remove(value);
                value.gameObject.SetActive(false);
            },
            (value) =>
            {
                Destroy(value);
            }
        );
        CatPool = new ObjectPool<EnemyActor>(
            () =>
            {
                EnemyActor value = GameObject.Instantiate(CatPrefab, EnemyGroup);
                value.CleanActor();
                return value;
            },
            (value) =>
            {
                value.kind = EnemyKind.Cat;
                EnemyList.Add(value);
                value.gameObject.SetActive(true);
                value.animator.SetTrigger("Idle");
                foreach (SpriteRenderer spriteRenderer in value.GetComponentsInChildren<SpriteRenderer>())
                {
                    spriteRenderer.material.color = Color.white;
                    spriteRenderer.color = Color.white;
                }
                value.CleanActor();
            },
            (value) =>
            {
                value.CleanActor();
                EnemyList.Remove(value);
                value.gameObject.SetActive(false);
            },
            (value) =>
            {
                Destroy(value);
            }
        );
        CamicalPool = new ObjectPool<EnemyActor>(
                    () =>
                    {
                        EnemyActor value = GameObject.Instantiate(CamicalPrefab, EnemyGroup);
                        value.CleanActor();
                        return value;
                    },
                    (value) =>
                    {
                        value.kind = EnemyKind.Camical;
                        EnemyList.Add(value);
                        value.gameObject.SetActive(true);
                        value.animator.SetTrigger("Idle");
                        foreach (SpriteRenderer spriteRenderer in value.GetComponentsInChildren<SpriteRenderer>())
                        {
                            spriteRenderer.material.color = Color.white;
                            spriteRenderer.color = Color.white;
                        }
                        value.CleanActor();
                    },
                    (value) =>
                    {
                        value.CleanActor();
                        EnemyList.Remove(value);
                        value.gameObject.SetActive(false);
                    },
                    (value) =>
                    {
                        Destroy(value);
                    }
                );
        GhostPool = new ObjectPool<EnemyActor>(
                    () =>
                    {
                        EnemyActor value = GameObject.Instantiate(GhostPrefab, EnemyGroup);
                        value.CleanActor();
                        return value;
                    },
                    (value) =>
                    {
                        value.kind = EnemyKind.Ghost;
                        EnemyList.Add(value);
                        value.gameObject.SetActive(true);
                        value.animator.SetTrigger("Idle");

                        foreach (SpriteRenderer spriteRenderer in value.GetComponentsInChildren<SpriteRenderer>())
                        {
                            spriteRenderer.material.color = Color.white;
                            spriteRenderer.color = Color.white;
                        }
                        value.CleanActor();
                    },
                    (value) =>
                    {
                        value.CleanActor();
                        EnemyList.Remove(value);
                        value.gameObject.SetActive(false);
                    },
                    (value) =>
                    {
                        Destroy(value);
                    }
                );
    }

    private void Start()
    {
        SoundManager.Inst.PlayBGM(1);
        PlayerSpawn(0, DepartmentKind.Sport);
        PlayerSpawn(1, DepartmentKind.Theology);
        PlayerSpawn(2, DepartmentKind.Camical);
        wave = 0;
        StartCoroutine(Battle());
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 15);
        bool hitColIsNotNull = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider is not null)
            {
                hitColIsNotNull = true;
                if (hit.collider.tag == "BattleActor")
                {
                    BattleActor actor = hit.collider.GetComponent<BattleActor>();
                    if (actor.isDead) continue;
                    selectedActor = actor;
                    if (actor != displayActor)
                    {
                        displayActor = actor;
                    }
                    SelectedActorDisplay.UpdateUI(displayActor);
                }
            }
        }
        if (hitColIsNotNull == false)
        {
            selectedActor = null;
        }
    }

    #endregion UnityFunc
    #region ActorSpawn
    public void EnemySpawn(int slot, EnemyKind kind)
    {
        ObjectPool<EnemyActor> pool = DrunkPool;
        int hp = 1000;
        int atk = 100;
        int def = 50;
        int speed = 1000;

        switch (kind)
        {
            case EnemyKind.Drunk:
                pool = DrunkPool;
                hp = 1000 + (EnemyLevel * 4);
                atk = 140 + (EnemyLevel);
                def = 30;
                speed = 700;
                break;
            case EnemyKind.Ghost:
                pool = GhostPool;
                hp = 600 + (EnemyLevel * 2);
                atk = 220 + (EnemyLevel * 2);
                def = 15;
                speed = 400;
                break;
            case EnemyKind.Cat:
                pool = CatPool;
                hp = 700 + (EnemyLevel * 3);
                atk = 40 + (EnemyLevel);
                def = 15;
                speed = 600;
                break;
            case EnemyKind.Camical:
                pool = CamicalPool;
                hp = 500 + (EnemyLevel * 2);
                atk = 160 + (EnemyLevel * 2);
                def = 10;
                speed = 200;
                break;
        }

        if (slot < 0 || slot >= EnemySlots.Length) return;
        EnemyActor result;
        result = pool.Get();
        result.transform.position = EnemySlots[slot].transform.position;
        result.LookPosition(middlePosition.position);
        result.InitActor(atk, def, speed, hp);
        result.hpBar = enemyBar[slot];
        enemyBar[slot].gameObject.SetActive(true);
        result.orderTMP = EnemyOrder[slot];
        result.OriginPos = EnemySlots[slot].position;
        result.UpdateHPBar();
    }
    public void PlayerSpawn(int slot, DepartmentKind kind)
    {
        int hp = 1000, atk = 100, def = 50;
        int speed = 800;
        int dc = 100;
        int hair = 0;
        int eye = 0;
        int top = 0;
        int bottom = 0;
        int depart = 0;
        int skin = 0;
        string name = "";
        switch (kind)
        {

            case DepartmentKind.Camical:
                hp = 1000;
                atk = 280;
                def = 20;
                speed = 400;
                dc = 140;

                hair = 1;
                eye = 11;
                top = 6;
                bottom = 0;
                depart = 3;
                skin = 1;
                name = "보일";
                /*
                hair 10
                eye0
                top2
                botton 0
                depart 6
                skin1
                */
                break;
            case DepartmentKind.Sport:
                hp = 1450;
                atk = 140;
                def = 35;
                speed = 900;
                dc = 80;
                name = "리처드";
                hair = 0;
                eye = 1;
                top = 3;
                bottom = 3;
                depart = 4;
                skin = 0;
                /*
                hair 0
                eye1
                top3
                botton 0
                depart 4
                skin0
                */
                break;
            case DepartmentKind.Theology:
                hp = 1300;
                atk = 100;
                def = 20;
                speed = 800;
                dc = 100;
                hair = 2;
                eye = 0;
                top = 2;
                bottom = 0;
                depart = 6;
                skin = 1;
                name = "베드로";
                /*
                hair 1
                eye11
                top6
                botton 0
                depart 3
                skin1
                */
                break;
        }
        if (slot < 0 || slot >= PlayerSlots.Length) return;
        PlayerActor actor = PlayerPool.Get();
        actor.transform.position = PlayerSlots[slot].position;
        actor.InitActor(atk, def, speed, hp, dc);
        actor.LookPosition(middlePosition.position);
        CharacterCustomizeData customizeData = new CharacterCustomizeData(name, hair, eye, top, bottom, depart, skin);
        actor.sr.ApplyCustomizeData(customizeData);
        actor.sr.setEyesSprite(actor.sr.skinSO.eyes[eye]);
        actor.name = name;
        actor.actorName = name;
        actor.departmentKind = kind;
        actor.orderTMP = PlayerOrder[slot];
        actor.OriginPos = PlayerSlots[slot].position;
        actor.hpBar = playerBar[slot];
        actor.hpBar.gameObject.SetActive(true);
        actor.UpdateHPBar();
    }
    #endregion ActorSpawn
    #region Card
    public bool IsUsableCard(CardObject co)
    {
        if (!(currentTurnActor is PlayerActor)) return false;
        if ((skillDelegater.GetSkill(co.skill) as PlayerSkill).departmentKind != (currentTurnActor as PlayerActor).departmentKind && (skillDelegater.GetSkill(co.skill) as PlayerSkill).departmentKind != DepartmentKind.None) return false;
        if ((skillDelegater.GetSkill(co.skill) as PlayerSkill).normalCost > normalCost) return false;
        if (co.isDepartment == true)
        {
            if ((skillDelegater.GetSkill(co.skill) as PlayerSkill).departmentCost > (currentTurnActor as PlayerActor).DCost) return false;
        }
        return true;
    }
    public void OnSkipButton()
    {
        if (!isWaiting) return;
        (currentTurnActor as PlayerActor).DCost += 30;
        normalCost += 3;
        skipThisTurn = true;
        isWaiting = false;
    }
    public bool UseCard(CardObject co)
    {
        if (!isWaiting) return false;
        if (!co.isUsable) return false;
        useCard = co;
        if (!skillDelegater.GetSkill(useCard.skill).isTargeting || selectedActor != null && currentTurnActor.targetableActors.Contains(selectedActor) && skillDelegater.IsTargetable(useCard.skill, this, currentTurnActor, selectedActor))
        {
            PlayerSkill skill = skillDelegater.GetSkill(co.skill) as PlayerSkill;
            normalCost -= skill.normalCost;
            (currentTurnActor as PlayerActor).DCost -= skill.departmentCost;
            if (skill.departmentKind == DepartmentKind.None) (currentTurnActor as PlayerActor).DCost += 30;
            targetActor = selectedActor;
            isWaiting = false;
            return true;
        }
        return false;
    }
    #endregion Card
    #region battleprocess
    public void GameOver()
    {
        stageLog.Print("GAME OVER");
        GameManager.Inst.battleresult = wave;
        GameManager.Inst.gameState = GameState.BattleResult;
        Invoke("LoadMain", 1.0f);
    }
    public void LoadMain()
    {
        SceneManager.LoadScene("main");
    }
    public IEnumerator Battle()
    {
        while (true)
        {
            wave++;
            stageLog.Print($"Wave {wave}");
            yield return new WaitForSeconds(0.5f);
            EnemyGroup.position = EnemySpawnPos.position;
            int i = Utill.random.Next(enemypreset.Count);
            int j = 0;
            foreach (EnemyKind enemyKind in enemypreset[i])
            {
                EnemySpawn(j, enemyKind);
                j++;
            }
            foreach (PlayerActor a in PlayerList)
            {
                a.animator.SetBool("IsRunning", true);
            }
            while (EnemyGroup.transform.position.x > 1.54)
            {
                bg.LoopMoveBG(3.0f * Time.deltaTime);
                EnemyGroup.transform.Translate(-3.0f * Time.deltaTime, 0, 0);

                yield return null;
            }
            foreach (EnemyActor enemyActor in EnemyList)
            {
                enemyActor.OriginPos = enemyActor.transform.position;
            }
            foreach (PlayerActor a in PlayerList)
            {
                a.animator.SetBool("IsRunning", false);
            }
            yield return Wave(wave);

            foreach (TMP_Text tmp in PlayerOrder)
            {
                tmp.text = "";
            }
            foreach (TMP_Text tmp in EnemyOrder)
            {
                tmp.text = "";
            }

        }

    }
    public IEnumerator Wave(int i)
    {
        while (EnemyList.Count != 0)
        {
            foreach (EnemyActor a in EnemyList)
            {
                a.OnPhaseStart?.Invoke(this, a);
            }
            foreach (PlayerActor a in PlayerList)
            {
                a.OnPhaseStart?.Invoke(this, a);
            }
            yield return Phase();
            foreach (PlayerActor playerActor in PlayerList) playerActor.ShieldLeft(this);
            foreach (EnemyActor enemyactor in EnemyList) enemyactor.ShieldLeft(this);
            foreach (EnemyActor a in EnemyList)
            {
                a.OnPhaseEnd?.Invoke(this, a);
            }
            foreach (PlayerActor a in PlayerList)
            {
                a.OnPhaseEnd?.Invoke(this, a);
            }
        }
    }
    public IEnumerator Phase()
    {
        //우선순위 설정
        Order = OrderCalculate();
        foreach (BattleActor actor in Order)
        {
            actor.hasActedThisPhase = false;
        }
        //카드 충전
        while (cardManager.normalCardCount < 5)
        {
            cardManager.DrawNC();
            yield return new WaitForSeconds(0.3f);
        }
        cardManager.ClearDC();
        while (cardManager.departmentCardCount < 5)
        {
            cardManager.DrawDC();
            yield return new WaitForSeconds(0.3f);
        }
        this.normalCost = 10;
        foreach (BattleActor b in Order)
        {

            if (b.isDead) continue;

            currentTurnActor = b;
            yield return Turn();
            Debug.Log("turnend");
            bool enemyalldead = true;
            bool Playeralldead = true;
            foreach (EnemyActor a in EnemyList)
            {
                if (!a.isDead)
                {
                    enemyalldead = false;
                    break;
                }
            }
            if (enemyalldead) break;
            foreach (PlayerActor a in PlayerList)
            {
                if (!a.isDead)
                {
                    Playeralldead = false;
                    break;
                }
            }
            if (Playeralldead)
            {
                GameOver();
            }
        }
        //사망 액터 처리
        List<BattleActor> deadActors = new List<BattleActor>();
        foreach (BattleActor b in PlayerList)
        {
            if (b.isDead) deadActors.Add(b);
        }
        foreach (BattleActor b in deadActors)
        {
            PlayerPool.Release(b as PlayerActor);
        }
        deadActors.Clear();
        foreach (BattleActor b in EnemyList)
        {
            if (b.isDead) deadActors.Add(b);
        }
        foreach (BattleActor b in deadActors)
        {
            if ((b as EnemyActor).kind == EnemyKind.Cat) CatPool.Release(b as EnemyActor);
            else if ((b as EnemyActor).kind == EnemyKind.Drunk) DrunkPool.Release(b as EnemyActor);
            else if ((b as EnemyActor).kind == EnemyKind.Ghost) GhostPool.Release(b as EnemyActor);
            else if ((b as EnemyActor).kind == EnemyKind.Camical) CamicalPool.Release(b as EnemyActor);
        }
    }

    public IEnumerator TurnStart()
    {
        //턴 표시
        //스텟 계산 및 표기
        //대상 지정 가능 계산

        currentTurnActor.tauntedEnemy.Clear();
        currentTurnActor.tauntedFriendly.Clear();
        skipThisTurn = false;
        DisplayOrderAll();
        foreach (BattleActor b in Order)
        {
            b.StatCalculate(this);
            b.TurnStartListeners.Clear();
            if (!b.isDead)
            {
                b.OnTurnStart?.Invoke(this, b, currentTurnActor);
            }
        }
        foreach (BattleActor actor in Order)
        {
            if (actor.isDead == false)
            {
                actor.StatCalculate(this);
                foreach (BattleTaskListener listener in actor.TurnStartListeners)
                {
                    yield return listener(this, actor);
                }
                actor.StatCalculate(this);
            }
        }
        if (!skipThisTurn) isWaiting = true;//대기 상태로 전환
        else isWaiting = false;
        //actor list 콜백 호출
        yield break;
    }
    public IEnumerator Turn()
    {
        CurrentTurnDisplay.UpdateUI(currentTurnActor);
        yield return TurnStart();
        CurrentTurnDisplay.UpdateUI(currentTurnActor);

        currentTurnActor.UpdateTargetableActors(this);
        SkillKind enemySkill = SkillKind.none;
        if (currentTurnActor is PlayerActor) isWaiting = true;
        if (currentTurnActor is EnemyActor)
        {
            //대상, 
            enemySkill = (currentTurnActor as EnemyActor).GetRandomSkill();
            List<BattleActor> targets = new List<BattleActor>();
            foreach (BattleActor battleActor in currentTurnActor.targetableActors)
            {
                if (skillDelegater.IsTargetable(enemySkill, this, currentTurnActor, battleActor))
                {
                    targets.Add(battleActor);
                }
            }
            int i = Utill.random.Next(targets.Count);
            targetActor = targets[i];
            isWaiting = false;
        }
        cardManager.UpdateUsable();

        yield return new WaitUntil(() => !isWaiting);
        if (!skipThisTurn)
        {

            if (currentTurnActor is PlayerActor)
            {
                yield return skillDelegater.DoSkill(useCard.skill, this, currentTurnActor, targetActor);
            }
            else if (currentTurnActor is EnemyActor)
            {
                yield return skillDelegater.DoSkill(enemySkill, this, currentTurnActor, targetActor);
            }

        }
        foreach (BattleActor actor in Order)
        {
            actor.StatCalculate(this);
        }
        skipThisTurn = false;
        yield return new WaitForSeconds(0.4f);

        CurrentTurnDisplay.UpdateUI(currentTurnActor);
        yield return TurnEnd();
        CurrentTurnDisplay.UpdateUI(currentTurnActor);
        currentTurnActor.hasActedThisPhase = true;
        yield break;
    }

    public IEnumerator TurnEnd()
    {
        //스탯 재계산
        //반격, 턴 종료시 이벤트 등등 수행
        foreach (BattleActor b in Order)
        {
            if (!b.isDead)
            {
                b.OnTurnEnd?.Invoke(this, b, currentTurnActor);
                b.StatCalculate(this);
            }
        }
        foreach (BattleActor actor in Order)
        {
            if (actor.isDead == false)
            {
                foreach (BattleTaskListener listener in actor.TurnEndListeners)
                {
                    yield return listener(this, actor);
                }
                actor.StatCalculate(this);
                actor.TurnEndListeners.Clear();
            }
        }

        //턴 종료 이벤트 실행
        //
        yield break;
    }
    public void TakeRest()
    {
        if (isWaiting)
        {
            skipThisTurn = true;
            (currentTurnActor as PlayerActor).DCost += (int)((currentTurnActor as PlayerActor).MaxDCost * 0.2f);
            normalCost += 2;
            isWaiting = false;
        }

    }
    #endregion battleprocess


    /*
        manager< -skill
        스텟 계산
        startcoutine
    */
    public List<BattleActor> GetAliveActors()
    {
        aliveActors.Clear();
        aliveActors.AddRange(PlayerList);
        aliveActors.AddRange(EnemyList);
        List<BattleActor> deleteactor = new List<BattleActor>();
        foreach (BattleActor actor in aliveActors)
        {
            if (actor.isDead) deleteactor.Add(actor);
        }
        foreach (BattleActor actor in deleteactor)
        {
            aliveActors.Remove(actor);
        }

        return aliveActors;

    }
    public BattleActor GetBackActor(BattleActor actor)
    {
        int i;
        if (0 <= (i = EnemyList.IndexOf(actor as EnemyActor)))
        {
            i++;
            while (i < EnemyList.Count())
            {

                if (!EnemyList[i].isDead)
                {
                    return EnemyList[i];
                }
                i++;
            }
        }
        else if (0 <= (i = PlayerList.IndexOf(actor as PlayerActor)))
        {
            i++;
            while (i < PlayerList.Count())
            {

                if (!PlayerList[i].isDead)
                {
                    return PlayerList[i];
                }
                i++;
            }
        }
        return null;
    }
    public BattleActor GetFrontActor(BattleActor actor)
    {
        int i;
        if (0 <= (i = EnemyList.IndexOf(actor as EnemyActor)))
        {
            i--;
            while (i >= 0)
            {

                if (!EnemyList[i].isDead)
                {
                    return EnemyList[i];
                }
                i--;
            }
        }
        else if (0 <= (i = PlayerList.IndexOf(actor as PlayerActor)))
        {
            i--;
            while (i >= 0)
            {

                if (!PlayerList[i].isDead)
                {
                    return PlayerList[i];
                }
                i--;
            }
        }
        return null;
    }
    public List<BattleActor> GetAliveUnFriendly(bool isEnemy)
    {
        if (isEnemy)
        {
            alivePlayer.Clear();
            alivePlayer.AddRange(PlayerList);
            List<BattleActor> deleteactor = new List<BattleActor>();
            foreach (BattleActor actor in aliveActors)
            {
                if (actor.isDead) deleteactor.Add(actor);
            }
            foreach (BattleActor actor in deleteactor)
            {
                aliveActors.Remove(actor);
            }

            return alivePlayer;
        }
        else
        {
            aliveEnemy.Clear();
            aliveEnemy.AddRange(EnemyList);
            List<BattleActor> deleteactor = new List<BattleActor>();
            foreach (BattleActor actor in aliveActors)
            {
                if (actor.isDead) deleteactor.Add(actor);
            }
            foreach (BattleActor actor in deleteactor)
            {
                aliveActors.Remove(actor);
            }
            return aliveEnemy;
        }
    }
    public List<BattleActor> GetAliveFriendly(bool isEnemy)
    {
        if (isEnemy)
        {
            aliveEnemy.Clear();
            aliveEnemy.AddRange(EnemyList);
            List<BattleActor> deleteactor = new List<BattleActor>();
            foreach (BattleActor actor in aliveActors)
            {
                if (actor.isDead) deleteactor.Add(actor);
            }
            foreach (BattleActor actor in deleteactor)
            {
                aliveActors.Remove(actor);
            }
            return aliveEnemy;
        }
        else
        {
            alivePlayer.Clear();
            alivePlayer.AddRange(PlayerList);
            List<BattleActor> deleteactor = new List<BattleActor>();
            foreach (BattleActor actor in aliveActors)
            {
                if (actor.isDead) deleteactor.Add(actor);
            }
            foreach (BattleActor actor in deleteactor)
            {
                aliveActors.Remove(actor);
            }

            return alivePlayer;
        }

    }
    public List<BattleActor> OrderCalculate()
    {
        List<BattleActor> result = new List<BattleActor>();
        List<BattleActor> temp = new List<BattleActor>();
        temp.AddRange<BattleActor>(EnemyList);
        temp.AddRange<BattleActor>(PlayerList);
        while (temp.Count > 0)
        {
            int totalWeight = 0;
            foreach (BattleActor b in temp)
            {
                totalWeight += b.speed;
            }
            int randomValue = Utill.random.Next(1, totalWeight + 1);
            BattleActor battleActor = null;
            foreach (BattleActor b in temp)
            {
                randomValue -= b.speed;
                if (randomValue <= 0)
                {
                    battleActor = b;
                    result.Add(b);
                    break;
                }
            }
            temp.Remove(battleActor);
        }

        while (temp.Count > 0)
        {
            BattleActor battleActor = null;

            temp.Remove(battleActor);
        }
        return result;
    }
    /// <summary>
    /// 행동 순서 표시
    /// </summary>

    public void DisplayOrderAll()
    {
        int i = 0;
        foreach (TMP_Text tmp in PlayerOrder)
        {
            tmp.text = "";
        }
        foreach (TMP_Text tmp in EnemyOrder)
        {
            tmp.text = "";
        }
        foreach (BattleActor actor in Order)
        {
            if (actor.isDead || actor.hasActedThisPhase) continue;
            actor.DisplayOrder(i);
            i++;
        }
    }
    #region UI
    void CardInfoUpdate()
    {
        if (selectedCard == null)
        {
            cardInfoTMP.text = "선택 카드 없음.";
        }
        else
            cardInfoTMP.text = $"{skillDelegater.GetSkill(selectedCard.skill)}";
    }

    #endregion UI
    #region Preset
    List<List<EnemyKind>> enemypreset = new List<List<EnemyKind>>{
        new List<EnemyKind>{EnemyKind.Drunk,EnemyKind.Camical,EnemyKind.Camical},
        new List<EnemyKind>{EnemyKind.Camical,EnemyKind.Camical,EnemyKind.Camical},
        new List<EnemyKind>{EnemyKind.Drunk,EnemyKind.Ghost,EnemyKind.Cat,EnemyKind.Cat},
        new List<EnemyKind>{EnemyKind.Drunk,EnemyKind.Camical,EnemyKind.Cat,EnemyKind.Ghost},
        new List<EnemyKind>{EnemyKind.Drunk,EnemyKind.Ghost,EnemyKind.Cat,EnemyKind.Cat},
        new List<EnemyKind>{EnemyKind.Drunk,EnemyKind.Drunk,EnemyKind.Ghost,EnemyKind.Ghost},
    };
    #endregion Preset
    #region test

    #endregion test
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum InGameState
{
    Unknown, newGame, turnStart, dataSave, standby, execution
}
public class InGameManager : MonoBehaviour
{
    //todo 20240704
    //이벤트 델리게이트 만들기. 완료
    //텍스트에 문자열 올라오게 만드는거 구현(일정 딜레이를 두고 올라오게): ingamelogview 에서 구현
    /*
    뉴게임 로드게임 선택
    로딩 씬 로드
    세이브 데이터 불러오기 or 생성
    오브젝트 초기화, 활성화
    이벤트
    */
    public InGameState inGameState = InGameState.Unknown;
    #region Listener
    public delegate void InGameListener<T>(InGameManager inGameManager, T t);
    public delegate void InGameListener<T, U>(InGameManager inGameManager, T t, U u);
    public delegate void InGameListener(InGameManager inGameManager);
    //자원 소모 이벤트, 획득 이벤트
    public event InGameListener<int> OnUseMoney;
    public event InGameListener<int> OnUseMental;
    public event InGameListener<int> OnUseStamina;
    public event InGameListener<int> OnGainExp;
    public event InGameListener<int> OnGainMoney;
    public event InGameListener<int> OnGainMental;
    public event InGameListener<int> OnGainStamina;
    //버프 이벤트
    public event InGameListener<BuffKind> OnGetBuff;
    public event InGameListener<BuffKind, int> OnStackBuff;
    public event InGameListener<BuffKind> OnReleseBuff;
    //돌발 이벤트 확률 계산시 실행
    public event InGameListener OnActivityProCalc;//확률 계산(turn start 시점에 해도 될듯)안됨 확률 계산 함수를 새로 만들어서 버프 부여, 취소 시에 재실행
    public event InGameListener<ActivityKind> OnDoActivity;//계수 계산
    public event InGameListener<ActivityKind> OnAfterActivity;//행동 후 추가 효과
    public event InGameListener OnTurnStart;//턴 시작시 이벤트
    public event InGameListener OnTurnEnd;//턴 종료시 이벤트
    public event InGameListener OnOneHourLeft;//시간 경과 이벤트
    public event InGameListener<ItemKind> OnBuyItem;//아이템 구매시 추가 효과
    public event InGameListener<ItemKind> OnGetItem;//아이템 획득시 추가 효과
    public event InGameListener<ItemKind> OnUseItem;//아이템 사용시 추가 효과
    public event InGameListener OnStartBattle;//전투 시작시 
    public event InGameListener OnLevelUp;//레벨업 추가 효과
    public event InGameListener OnGameOver;//게임 오버시 효과(쓸까..?)
    public event InGameListener OnDayLeft;
    public event InGameListener OnWeekLeft;
    #endregion Listener
    #region Deligator
    public ActivityDeligator activityDeligator;
    public ItemDelegator itemDelegator;
    public BuffDeligator buffDeligator;
    #endregion Deligator


    public Player player;
    bool isGameOver = false;
    public ScheduleManager scheduleManager;


    public List<Schedule> selectableSchedule = new List<Schedule>();
    #region UIObjects
    [SerializeField] Window ActivityWindow;
    [SerializeField] ScheduleWindow scheduleWindow;
    [SerializeField] Window ShopWindow;
    [SerializeField] ShopView shopView;
    [SerializeField] ScheduleView scheduleView;
    [SerializeField] InGameLogView inGameLogView;
    [SerializeField] ActivityView activityView;
    [SerializeField] TMP_Text mentalTMP;
    [SerializeField] TMP_Text staminaTMP;
    [SerializeField] TMP_Text moneyTMP;
    #endregion UIObjects
    private void Awake()
    {

        activityDeligator = new ActivityDeligator();
        itemDelegator = new ItemDelegator();
        buffDeligator = new BuffDeligator();
        scheduleManager = new ScheduleManager(this);

        if (inGameLogView == null) inGameLogView = GameObject.Find("InGameLogView").GetComponent<InGameLogView>();
        if (activityView == null) activityView = GameObject.Find("ActivityView").GetComponent<ActivityView>();
        if (scheduleView == null) scheduleView = GameObject.Find("ScheduleView").GetComponent<ScheduleView>();


    }
    private void Start()
    {
        SoundManager.Inst.PlayBGM(0);
        ActivityWindow.CloseWindow();
        ShopWindow.CloseWindow();
        if (GameManager.Inst.gameState == GameState.NewGame)
        {
            player = new Player(this);
            inGameState = InGameState.newGame;
            selectableSchedule.AddRange(scheduleManager.GetRandomLesson(40));
            scheduleView.UpdateView();
        }
        else
        {
            scheduleWindow.CloseWindow();
        }
        if (GameManager.Inst.gameState == GameState.LoadData)
        {
            Load();
            foreach (KeyValuePair<BuffKind, Buff> buff in player.activatedBuffs)
            {
                buff.Value.AddListener(this);
            }
        }
        if (GameManager.Inst.gameState == GameState.BattleResult)
        {
            player = GameManager.Inst.player;
            scheduleManager = GameManager.Inst.scheduleManager;
            player.Init(this);
            scheduleManager.Init(this);

            BattleResult();
            foreach (KeyValuePair<BuffKind, Buff> buff in player.activatedBuffs)
            {
                buff.Value.AddListener(this);
            }
            inGameState = InGameState.standby;

        }

        activityDeligator.UpdateSelectable(this);
        UpdateAllUI();
    }
    private void Update()
    {
        if (inGameState == InGameState.execution)
        {
            int mc = inGameLogView.messeges.Count;
            if (mc <= 0)
            {
                inGameState = InGameState.standby;
            }
        }

    }
    public void SaveAndExit()
    {
        Save();
        SceneManager.LoadScene("TitleMenu");
    }
    public void Save()
    {
        string path = Application.persistentDataPath + "\\player.dat";
        string path2 = Application.persistentDataPath + "\\SM.dat";
#if UNITY_EDITOR
        path = Application.streamingAssetsPath + "\\player.dat";
        path2 = Application.streamingAssetsPath + "\\SM.dat";
#endif
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, player);
        FileStream fs2 = new FileStream(path2, FileMode.Create);
        bf.Serialize(fs2, scheduleManager);
        fs.Close();
        fs2.Close();
    }
    public void DeleteData()
    {
        string path = Application.persistentDataPath + "\\player.dat";
        string path2 = Application.persistentDataPath + "\\SM.dat";
#if UNITY_EDITOR
        path = Application.streamingAssetsPath + "\\player.dat";
        path2 = Application.streamingAssetsPath + "\\SM.dat";
#endif
        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        if (System.IO.File.Exists(path2)) System.IO.File.Delete(path2);
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "\\player.dat";
        string path2 = Application.persistentDataPath + "\\SM.dat";
#if UNITY_EDITOR
        path = Application.streamingAssetsPath + "\\player.dat";
        path2 = Application.streamingAssetsPath + "\\SM.dat";
#endif
        if (File.Exists(path) && File.Exists(path2))
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            player = bf.Deserialize(fs) as Player;
            FileStream fs2 = new FileStream(path2, FileMode.Open);
            scheduleManager = bf.Deserialize(fs2) as ScheduleManager;
            player.Init(this);
            scheduleManager.Init(this);
            fs.Close();
            fs2.Close();
        }
        else
        {
            SceneManager.LoadScene("TitleMenu");
        }
    }
    public void BattleResult()
    {
        inGameState = InGameState.execution;
        int result = GameManager.Inst.battleresult;
        InGameLog("수업을 마쳤다.");
        if (result >= 0 && result < 3)
        {
            InGameLog("하나도 이해하지 못했다..");
            MentalDamage(400);
        }
        else if (result >= 3 && result < 4)
        {
            InGameLog("조금은 이해한 것 같다.");
            MentalDamage(200);
            // 25~50 구간 처리
        }
        else if (result >= 4 && result < 5)
        {
            // 50~75 구간 처리
            InGameLog("무난하게 수업을 마쳤다.");
            MentalDamage(100);
        }
        else if (result >= 5)
        {
            // 75 이상의 구간 처리
            InGameLog("완벽하게 이해했다!");
        }
        inGameState = InGameState.standby;
    }
    public void InGameLog(string log, float delay = 0.1f)
    {
        if (isGameOver) return;
        inGameLogView.messeges.Enqueue(new Tuple<string, float>(log, delay));
    }

    public void UpdateAllUI()
    {
        inGameLogView.UpdateView();
        scheduleView.UpdateView();
        activityView.UpdateView();
        shopView.UpdateView();
        UpdateStatus();

    }
    public void UpdateStatus()
    {
        moneyTMP.text = $"금전: {player.money}";
        mentalTMP.text = $"멘탈: {player.mental}/{player.MAXmental}";
        staminaTMP.text = $"스테미나: {player.stamina}/{player.MAXstamina}";
    }
    /// <summary>
    /// 턴 시작 시 호출
    /// </summary>
    public void TurnStart()
    {
        ResetListeners();
        AddAllListeners();
    }
    public void GameClear()
    {
        inGameLogView.messeges.Clear();
        inGameLogView.messeges.Enqueue(new Tuple<string, float>("한 학기를 무사히 마쳤다.", 0f));
        inGameLogView.messeges.Enqueue(new Tuple<string, float>("GameClear", 0f));
        isGameOver = true;
        DeleteData();
        Invoke("LoadTitle", 3f);

    }
    public void GameOver()
    {
        inGameLogView.messeges.Clear();
        inGameLogView.messeges.Enqueue(new Tuple<string, float>("가혹한 학교생활에 마음이 꺾였다.", 0f));
        inGameLogView.messeges.Enqueue(new Tuple<string, float>("GameOver", 0f));
        isGameOver = true;
        DeleteData();
        Invoke("LoadTitle", 3f);
    }
    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleMenu");
    }
    public void StaminaDamage(int dmg, bool afterEffect = true)
    {
        player.stamina -= player.stamina >= dmg ? dmg : player.stamina;
        if (player.stamina <= 0)
        {
            if (player.activatedBuffs.ContainsKey(BuffKind.tierd))
            {
                player.activatedBuffs[BuffKind.tierd].stack = 2;
            }
            GetBuff(BuffKind.tierd, 2);
        }
        else if (player.stamina <= 60)
        {
            GetBuff(BuffKind.tierd, 1);
        }
        else if (player.activatedBuffs.ContainsKey(BuffKind.tierd))
        {
            ReleseBuff(BuffKind.tierd);
        }
        InGameLog("스테미나를 " + dmg + " 잃었다.");
        if (afterEffect) OnUseStamina?.Invoke(this, dmg);
        UpdateStatus();
    }
    public void MentalDamage(int dmg, bool afterEffect = true)
    {
        player.mental -= player.mental >= dmg ? dmg : player.mental;
        if (player.mental <= 0) GameOver();
        InGameLog("멘탈을 " + dmg + " 잃었다.");
        if (afterEffect) OnUseMental?.Invoke(this, dmg);
        UpdateStatus();
    }
    public bool MoneyUse(int use, bool afterEffect = true)
    {
        if (player.money < use) return false;
        player.money -= use;
        if (afterEffect) OnUseMoney?.Invoke(this, use);
        InGameLog("금전을 " + use + " 잃었다.");
        UpdateStatus();
        return true;
    }

    public void StaminaGain(int gain, bool afterEffect = true)
    {
        if (player.stamina + gain > player.MAXstamina) player.stamina = player.MAXstamina;
        else player.stamina += gain;
        if (player.stamina <= 0)
        {
            if (player.activatedBuffs.ContainsKey(BuffKind.tierd))
            {
                player.activatedBuffs[BuffKind.tierd].stack = 2;
            }
            GetBuff(BuffKind.tierd, 2);
        }
        else if (player.stamina <= 60)
        {
            GetBuff(BuffKind.tierd, 1);
        }
        else if (player.activatedBuffs.ContainsKey(BuffKind.tierd))
        {
            ReleseBuff(BuffKind.tierd);
        }
        InGameLog("스테미나를 " + gain + " 얻었다");
        if (afterEffect) OnGainStamina?.Invoke(this, gain);
        UpdateStatus();
    }
    public void MentalGain(int gain, bool afterEffect = true)
    {
        if (player.mental + gain > player.MAXmental) player.mental = player.MAXmental;
        else player.mental += gain;
        InGameLog("멘탈을 " + gain + " 얻었다");
        if (afterEffect) OnGainMental?.Invoke(this, gain);
        UpdateStatus();
    }
    public void MoneyGain(int gain, bool afterEffect = true)
    {
        if (player.money + gain > player.MAXmoney) player.money = player.MAXmoney;
        else player.money += gain;
        InGameLog("금전을 " + gain + " 얻었다");
        if (afterEffect) OnGainMoney?.Invoke(this, gain);
        UpdateStatus();
    }
    /// <summary>
    /// 아이템 사용
    /// </summary>
    /// <param name="itemKind">사용할 아이템</param>
    public bool UseItem(ItemKind itemKind)
    {
        //OnUseItem

        //use
        if (player.inStockItems.ContainsKey(itemKind))
        {
            InGameLog(player.inStockItems[itemKind].name + "을/를 사용했다.");
            player.inStockItems[itemKind].UseItem(this);
            if (player.inStockItems[itemKind].itemCount <= 0)
            {
                player.inStockItems.Remove(itemKind);
            }
            OnUseItem?.Invoke(this, itemKind);
            activityDeligator.UpdateSelectable(this);
            UpdateAllUI();
            return true;
        }
        else
        {
            Debug.LogError("ItemUseError");
            return false;
        }

    }
    /// <summary>
    /// 아이템 획득 시 실행하는 메소드
    /// </summary>
    /// <param name="itemKind">획득할 아이템 종류</param>
    /// todo
    ///
    public void GetItem(ItemKind itemKind)
    {
        if (player.inStockItems.ContainsKey(itemKind))
        {
            player.inStockItems[itemKind].itemCount++;
        }
        else
        {
            player.inStockItems[itemKind] = itemDelegator.GetItem(itemKind);
            player.inStockItems[itemKind].itemCount = 1;
        }


        InGameLog(itemDelegator.GetItem(itemKind).name + "을/를 획득했다.");
        OnGetItem?.Invoke(this, itemKind);
    }
    public void BuyItem(ItemKind itemKind)
    {
        if (inGameState == InGameState.execution) return;
        inGameState = InGameState.execution;
        Item item = itemDelegator.GetItem(itemKind);
        GetItem(itemKind);
        MoneyUse(item.price);
        OnBuyItem?.Invoke(this, itemKind);
        UpdateAllUI();
    }
    /// <summary>
    /// 버프 획득시 실행하는 메서드
    /// </summary>
    /// <param name="buffKind">버프 종류</param>
    /// todo----
    public void GetBuff(BuffKind buffKind, int stack = 1)
    {
        if (player.activatedBuffs.ContainsKey(buffKind))
        {
            if (player.activatedBuffs[buffKind].stack == stack) return;
            OnStackBuff?.Invoke(this, buffKind, stack);
            player.activatedBuffs[buffKind].stack = stack;
            InGameLog(player.activatedBuffs[buffKind].name + ": " + player.activatedBuffs[buffKind].stack + " 중첩");
            return;
        }
        else
        {

            buffDeligator.GetBuff(buffKind).GetBuff(this, stack);
            InGameLog(player.activatedBuffs[buffKind].name + " 획득");
            OnGetBuff?.Invoke(this, buffKind);
        }
        activityDeligator.UpdateSelectable(this);

        //여기도 버프 아이콘 갱신 등 추가
    }
    public void ReleseBuff(BuffKind buffKind)
    {
        InGameLog($"{player.activatedBuffs[buffKind].name} 제거");
        OnReleseBuff?.Invoke(this, buffKind);
        player.activatedBuffs[buffKind].ReleseBuff(this);
        activityDeligator.UpdateSelectable(this);
        //버프 아이콘 갱신 등 추가
    }
    public void TimeLeft(int d)
    {
        InGameLog(d + "시간 경과.");
        player.TimeLeft(d);
        if (!player.activatedBuffs.ContainsKey(BuffKind.hungry))
        {
            GetBuff(BuffKind.hungry, 0);
        }
        for (int i = 0; i < d; i++)
        {
            OnOneHourLeft?.Invoke(this);
        }

        selectableSchedule.Clear();

    }
    public void DayLeft()
    {
        InGameLog("하루 경과.");
        player.DayLeft();
        OnDayLeft?.Invoke(this);
        StaminaGain(player.MAXstamina, false);
    }
    public void WeekLeft()
    {
        player.WeekLeft();
        OnWeekLeft?.Invoke(this);
        if (player.GetWeek > 15)
        {
            GameClear();
        }
    }
    /// <summary>
    /// activity 실행
    /// </summary>
    /// <param name="activityKind">실행할 activity</param>
    public void DoActivity(ActivityKind activityKind)
    {
        if (inGameState == InGameState.execution) return;
        inGameState = InGameState.execution;
        OnDoActivity?.Invoke(this, activityKind);
        activityDeligator.DoActivity(this, activityKind);
        activityDeligator.UpdateSelectable(this);
        UpdateAllUI();
    }
    public void AddSchedule(Schedule schedule)
    {
        if (inGameState == InGameState.execution) return;
        if (!scheduleManager.IsCanAdd(schedule))
        {
            InGameLog("스케줄을 추가할 수 없다. 시간이 겹치지 않는지 확인해 보자.", 0.0f);
            return;
        }
        if (selectableSchedule.Contains(schedule))
        {
            selectableSchedule.Remove(schedule);
        }
        scheduleManager.AddSchedule(schedule);
        InGameLog($"{schedule.name} 이/가 일정에 추가되었다.", 0.0f);
        activityDeligator.UpdateSelectable(this);
        UpdateAllUI();

    }
    public void RemoveSchedule(Schedule schedule)
    {
        if (inGameState == InGameState.execution) return;
        scheduleManager.RemoveSchedule(schedule);
        InGameLog($"{schedule.name} 이/가 일정에서 제거되었다.", 0.0f);
        selectableSchedule.Add(schedule);
        activityDeligator.UpdateSelectable(this);
        UpdateAllUI();
    }
    #region ListenerFunc
    /// <summary>
    /// 리스너 비우기
    /// </summary>
    public void ResetListeners()
    {
        OnUseMoney = null;
        OnUseMental = null;
        OnUseStamina = null;
        OnGainMoney = null;
        OnGainStamina = null;
        OnGainMental = null;
        OnGainExp = null;
        OnGetBuff = null;
        OnStackBuff = null;
        OnReleseBuff = null;
        OnActivityProCalc = null;
        OnDoActivity = null;
        OnAfterActivity = null;
        OnTurnStart = null;
        OnTurnEnd = null;
        OnOneHourLeft = null;
        OnBuyItem = null;
        OnGetItem = null;
        OnUseItem = null;
        OnStartBattle = null;
        OnLevelUp = null;
        OnGameOver = null;
        OnDayLeft = null;
    }
    /// <summary>
    /// 가지고 있는 모든 버프의 리스너 등록
    /// </summary>
    public void AddAllListeners()
    {
        foreach (KeyValuePair<BuffKind, Buff> buff in player.activatedBuffs)
        {
            buff.Value.AddListener(this);
        }
    }
    #endregion ListenerFunc
    //testArea_______________________________________________________________________
    #region testArea



    #endregion testArea
    //testArea______________________________________________________________________
}





using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UtillTime;
using UnityEditor;
[System.Serializable]
public class Player
{
    public Player(InGameManager inGameManager)
    {
        this.inGameManager = inGameManager;
        inStockItems = new SortedDictionary<ItemKind, Item>();
        activatedBuffs = new SortedDictionary<BuffKind, Buff>();


    }
    [System.NonSerialized]
    InGameManager inGameManager;
    public void Init(InGameManager inGameManager)
    {
        this.inGameManager = inGameManager;
        //버프 델리게이터들 등록
    }
    //델리게이트로 플레이어가 행동하거나 피해를 입는 등의 사건 발생시 등록된 메서드들 실행(버프, 디버프 구현)
    //버프 배열: 델리게이트는 실행될 때마다 초기화되므로 정보를 담을 버프 클래스들의 배열을 따로 등록



    //액티비티 관련
    //아이템 관련
    public SortedDictionary<ItemKind, Item> inStockItems;
    //버프 관련
    public SortedDictionary<BuffKind, Buff> activatedBuffs;
    //카드 관련

    //날짜 관련
    private int week = 1;
    private int hour = 9;
    private int semester = 1;

    public int GetSemester => semester;
    private WeekDay weekDay = WeekDay.monday;
    public int GetWeek => week;
    public int GetHour => hour;
    public WeekDay GetWeekday => weekDay;

    public void TimeLeft(int duration = 1)
    {
        hour += duration;
        if (hour > 24)
        {
            hour = 24;
        }
        else if (hour < 9)
        {
            hour = 9;
        }
        if (hour >= 24)
        {
            inGameManager.DayLeft();
        }
    }
    public void DayLeft()
    {
        if (weekDay == WeekDay.sunday)
        {
            weekDay = WeekDay.monday;
            hour = 9;
            inGameManager.WeekLeft();
            return;
        }
        weekDay++;
        hour = 9;
    }
    public void WeekLeft()
    {
        week++;
    }
    //스케줄



    public Lesson GetNowLesson()
    {
        return inGameManager.scheduleManager.GetLesson(weekDay, hour);
    }
    public PartTime GetNowPartTime()
    {
        return inGameManager.scheduleManager.GetPartTime(weekDay, hour);
    }



    //자원
    public int MAXmental = 1000;
    public int MAXstamina = 200;
    public int MAXmoney = 999999;
    public int mental = 1000;
    public int stamina = 200;
    public int money = 1000;
    public int level;
    public int exp;




}

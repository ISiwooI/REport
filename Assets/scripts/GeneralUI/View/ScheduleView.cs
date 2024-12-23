using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using UtillTime;

public class ScheduleView : View
{
    public override void UpdateView()
    {
        base.UpdateView();
        DisplaySchedule(inGameManager.selectableSchedule, inGameManager.scheduleManager.scheduleList);
        for (WeekDay weekDay = WeekDay.monday; weekDay <= WeekDay.sunday; weekDay++)
        {
            for (int i = 9; i <= 23; i++)
            {
                GetTable(weekDay)[i - 9].color = Color.white;
                if (inGameManager.scheduleManager.IsBusy(weekDay, i))
                {
                    GetTable(weekDay)[i - 9].color = Color.gray;
                }
            }
        }
    }
    [SerializeField]
    GameObject contents;
    [SerializeField]
    GameObject selectedContents;
    [SerializeField]
    ScheduleContainer ScheduleContainerPrefab;
    IObjectPool<ScheduleContainer> schedulePool;
    IObjectPool<ScheduleContainer> SelectedSchedulePool;

    List<ScheduleContainer> Containers = new List<ScheduleContainer>();

    List<ScheduleContainer> selectedContainers = new List<ScheduleContainer>();

    public Image[] mondayTable;
    public Image[] tuesdayTable;
    public Image[] wednesdayTable;
    public Image[] thursdayTable;
    public Image[] fridayTable;
    public Image[] saturdayTable;
    public Image[] sundayTable;
    Image[] GetTable(WeekDay weekDay)
    {
        switch (weekDay)
        {
            case WeekDay.monday:
                return mondayTable;
            case WeekDay.tuesday:
                return tuesdayTable;
            case WeekDay.wednesday:
                return wednesdayTable;
            case WeekDay.thursday:
                return thursdayTable;
            case WeekDay.friday:
                return fridayTable;
            case WeekDay.saturday:
                return saturdayTable;
            case WeekDay.sunday:
                return sundayTable;
        }
        return null;
    }
    public Image[][] Table;
    protected override void Awake()
    {
        base.Awake();
        #region PoolInit
        schedulePool = new ObjectPool<ScheduleContainer>(() =>
        {
            ScheduleContainer sc = GameObject.Instantiate(ScheduleContainerPrefab, contents.transform).GetComponent<ScheduleContainer>();
            sc.Init(this, true);
            return sc;
        }
        , (value) =>
        {
            Containers.Add(value);
            value.gameObject.SetActive(true);
        }
        , (value) =>
        {
            Containers.Remove(value);
            value.gameObject.SetActive(false);
        }
        , (value) =>
        {
            GameObject.Destroy(value);
        }
        , true, 10);
        SelectedSchedulePool = new ObjectPool<ScheduleContainer>(() =>
        {
            ScheduleContainer sc = GameObject.Instantiate(ScheduleContainerPrefab, selectedContents.transform).GetComponent<ScheduleContainer>();
            sc.Init(this, false);
            return sc;
        }
        , (value) =>
        {
            selectedContainers.Add(value);
            value.gameObject.SetActive(true);
        }
        , (value) =>
        {
            selectedContainers.Remove(value);
            value.gameObject.SetActive(false);
        }
        , (value) =>
        {
            GameObject.Destroy(value);
        }
        , true, 10);
        #endregion PoolInit

    }
    public void DisplaySchedule(List<Schedule> LeftSchedule, List<Schedule> RightSchedule)
    {
        RemoveAll();
        foreach (Schedule s in LeftSchedule)
        {
            AddSchedule(s, true);
        }
        foreach (Schedule s in RightSchedule)
        {
            AddSchedule(s, false);
        }
    }

    public void AddSchedule(Schedule schedule, bool isLeft)
    {
        ScheduleContainer sc;
        if (isLeft)
        {
            sc = schedulePool.Get();
            sc.SetSchedule(schedule);
        }
        else
        {
            sc = SelectedSchedulePool.Get();
            sc.SetSchedule(schedule);
        }
    }
    public void RemoveAll()
    {
        for (int i = Containers.Count - 1; i >= 0; i--)
        {
            ScheduleContainer sc = Containers[i];
            schedulePool.Release(sc);
        }
        Containers.Clear();
        for (int i = selectedContainers.Count - 1; i >= 0; i--)
        {
            ScheduleContainer sc = selectedContainers[i];
            SelectedSchedulePool.Release(sc);
        }
        selectedContainers.Clear();
    }

    public void OnScheduleButton(Schedule schedule, bool isLeft)
    {
        if (isLeft)
        {
            inGameManager.AddSchedule(schedule);
        }
        else
        {
            inGameManager.RemoveSchedule(schedule);
        }

    }
}

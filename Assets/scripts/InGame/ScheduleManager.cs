using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using UtillTime;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.PlayerLoop;
/*P
UnityEngine.Object에서 파생되지 않은 커스텀 클래스는 
Unity 에디터에서 구조체가 직렬화되는 방법과 비슷하게 값에 따라 인라인에서 직렬화됩니다.
 커스텀 클래스의 인스턴스에 대한 레퍼런스를 여러 다른 필드에 저장하면 직렬화할 때 별도의 오브젝트가 됩니다.
  그 후 Unity 에디터가 필드를 역직렬화하면 데이터가 똑같은 여러 개의 개별 오브젝트가 필드에 포함됩니다.
*/
[System.Serializable]
public class ScheduleManager
{

    public void Init(InGameManager inGameManager)
    {
        this.inGameManager = inGameManager;
    }
    public ScheduleManager(InGameManager inGameManager)
    {
        this.inGameManager = inGameManager;
        scheduleList = new List<Schedule>();
        schedules = new SortedDictionary<WeekDay, SortedDictionary<int, Schedule>>();
        busyTable = new SortedDictionary<WeekDay, SortedDictionary<int, bool>>();
        canActivity = new SortedDictionary<WeekDay, SortedDictionary<int, bool>>();
        for (WeekDay wd = WeekDay.monday; wd <= WeekDay.sunday; wd++)
        {
            schedules[wd] = new SortedDictionary<int, Schedule>();
            busyTable[wd] = new SortedDictionary<int, bool>();
            canActivity[wd] = new SortedDictionary<int, bool>();
            for (int i = 9; i <= 24; i++)
            {
                busyTable[wd][i] = false;
                canActivity[wd][i] = false;
            }
        }

    }
    [System.NonSerialized]
    public InGameManager inGameManager;
    public List<Schedule> scheduleList;
    SortedDictionary<WeekDay, SortedDictionary<int, Schedule>> schedules;
    SortedDictionary<WeekDay, SortedDictionary<int, bool>> busyTable;
    SortedDictionary<WeekDay, SortedDictionary<int, bool>> canActivity;
    ScheduleTimePreset Presets()
    {
        if (scheduleTimePresets == null)
        {
            scheduleTimePresets = new ScheduleTimePreset();
        }
        return scheduleTimePresets;

    }
    [System.NonSerialized]
    ScheduleTimePreset scheduleTimePresets = new ScheduleTimePreset();
    public bool IsBusy(WeekDay weekDay, int time)
    {
        if (!TimeCheck(time))
        {
            Debug.LogWarning("잘못된 시간 접근");
            return true;
        }
        return busyTable[weekDay][time];
    }
    /// <summary>
    /// 올바른 시간 범위인지 체크
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool TimeCheck(int time)
    {
        if (time < 9 || time > 24)
        {
            return false;
        }
        else return true;
    }
    public bool isHaveSchedule(Schedule schedule)
    {
        return scheduleList.Contains(schedule);
    }
    public bool IsCanAdd(Schedule schedule)
    {
        return IsCanAdd(schedule.scheduleTime);
    }
    public bool IsCanAdd(ScheduleTime scheduleTime)
    {

        if (!TimeCheck(scheduleTime.startHour) || !TimeCheck(scheduleTime.startHour + scheduleTime.timeDuration - 1)) return false;
        for (int i = 0; i < scheduleTime.timeDuration; i++)
        {
            if (IsBusy(scheduleTime.weekDay, i + scheduleTime.startHour))
            {
                return false;
            }
        }

        return true;
    }
    public bool IsCanActivity(WeekDay weekDay, int time)
    {
        if (!TimeCheck(time))
        {
            Debug.LogWarning("잘못된 시간 접근");
            return false;
        }
        return canActivity[weekDay][time];
    }
    public Schedule GetSchedule(WeekDay weekDay, int time)
    {
        if (IsBusy(weekDay, time)) return schedules[weekDay][time];
        else return null;
    }
    public Lesson GetLesson(WeekDay weekDay, int time)
    {
        if (IsLesson(weekDay, time)) return schedules[weekDay][time] as Lesson;
        else return null;
    }
    public PartTime GetPartTime(WeekDay weekDay, int time)
    {
        if (IsPartTime(weekDay, time)) return schedules[weekDay][time] as PartTime;
        else return null;
    }
    public bool IsLesson(WeekDay weekDay, int time)
    {
        return schedules[weekDay][time] is Lesson;
    }
    public bool IsPartTime(WeekDay weekDay, int time)
    {
        return schedules[weekDay][time] is PartTime;
    }


    public bool AddSchedule(Schedule schedule)
    {
        if (!IsCanAdd(schedule)) return false;
        if (isHaveSchedule(schedule)) return false;
        canActivity[schedule.scheduleTime.weekDay][schedule.scheduleTime.startHour] = true;
        for (int i = 0; i < schedule.scheduleTime.timeDuration; i++)
        {
            busyTable[schedule.scheduleTime.weekDay][i + schedule.scheduleTime.startHour] = true;
            schedules[schedule.scheduleTime.weekDay][i + schedule.scheduleTime.startHour] = schedule;
        }

        scheduleList.Add(schedule);
        return true;
    }
    public bool RemoveSchedule(WeekDay weekDay, int time)
    {
        if (schedules[weekDay][time] == null) return false;
        if (!TimeCheck(time)) return false;

        RemoveSchedule(schedules[weekDay][time]);
        scheduleList.Remove(schedules[weekDay][time]);
        return true;

    }
    public bool RemoveSchedule(Schedule schedule)
    {
        if (!isHaveSchedule(schedule)) return false;

        for (int i = 0; i < schedule.scheduleTime.timeDuration; i++)
        {
            busyTable[schedule.scheduleTime.weekDay][i + schedule.scheduleTime.startHour] = false;
            schedules[schedule.scheduleTime.weekDay][i + schedule.scheduleTime.startHour] = null;
        }

        scheduleList.Remove(schedule);
        return true;
    }
    public PartTime GetRandomPartTime()
    {
        string name;
        int costMental;
        int costStemina;
        int rewardMoney;
        List<string> names = new List<string>{
            "편의점"
            ,"카페"
            ,"상하차"
            ,"pc방"
            ,"과외"
            ,"노래방"
            ,"서빙"
            ,"서류작업"
        };
        int nindex = Utill.random.Next(names.Count);
        name = names[nindex];
        costMental = Utill.random.Next(50, 200);
        costStemina = Utill.random.Next(20, 50);
        rewardMoney = Utill.random.Next(300, 400);
        return new PartTime(name, costMental, costStemina, rewardMoney, GetRandomScheduleTime(1));
    }
    public List<Schedule> GetRandomLesson(int amount)
    {
        List<Schedule> result = new List<Schedule>();

        List<string> lessonNames = new List<string>
        {
            "엔티티요리학",
            "엔티티행동학",
            "엔티티 언어학",
            "어디서든잘자기",
            "비상사태 대응법",
            "올바른사망확인법",
            "파괴공작대처법",
            "몸으로배우는방어술",
            "현지조달생존법",
            "방사능환경행동강령",
            "야간환경행동강령",
            "혹한환경행동강령",
            "쉽게 배우는 도주",
            "이기는 전투 설계법",
            "던전 구조",
            "함정 및 엔티티",
            "신중한 전진법",
            "정신이 나가지 않는 법",
            "던전과 커뮤니케이션",
            "동료와 소통",
            "긴급상황 신속 소통법",
            "비언어적 소통",
            "엔티티 도축학",
            "은밀전투",
            "고립행동강령",
            "던전 이동법",
            "올바른 감정",
            "패닉 방지학",
            "불필요한 전투 회피법",
            "던전에서 나 잃지 않기",
            "엔티티 교란학",
            "보급관리학",
            "청결과 감염",
            "튼튼한 육체",
            "작전 설계와 돌발상황",
            "위화감과 안목",
            "던전의 역사",
            "올바른 엔티티 취급방법",
            "인물로 알아보는 던전사",
            "던전 평화 생명",
            "던전봉사",
            "던전 지형 분석 및 활용",
            "던전생태학",
            "해외 던전 사례",
            "논리와 비판적 사고",
            "비극의 이해",
            "언어와 구현",
            "던전시대의 이해",
            "던전 노동사",
            "던전과 엔티티",
            "생활속 던전",
            "확률과 통계"
        };
        if (amount > lessonNames.Count || amount < 0)
        {
            amount = lessonNames.Count;
        }
        for (int i = 0; i < amount; i++)
        {
            int nindex = Utill.random.Next(lessonNames.Count);
            string name = lessonNames[nindex];
            lessonNames.RemoveAt(nindex);
            int dif = Utill.random.Next(3);
            Lesson.LessonDifficulty difficulty = Lesson.LessonDifficulty.Normal;

            switch (dif)
            {
                case 0:
                    difficulty = Lesson.LessonDifficulty.Easy;
                    break;
                case 1:
                    difficulty = Lesson.LessonDifficulty.Normal;
                    break;
                case 2:
                    difficulty = Lesson.LessonDifficulty.Hard;
                    break;

            }
            result.Add(new Lesson(Utill.random.Next(101), difficulty, name, GetRandomScheduleTime(0)));
        };
        return result;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">0: lesson, 1~: parttime</param>
    /// <returns></returns>
    private ScheduleTime GetRandomScheduleTime(int type)
    {
        List<ScheduleTime> result = new List<ScheduleTime>();
        if (type == 0)
        {
            foreach (ScheduleTime st in scheduleTimePresets.lessonSchedulePreset)
            {
                if (IsCanAdd(st)) result.Add(st);
            }
        }
        else
        {
            foreach (ScheduleTime st in scheduleTimePresets.partTimeSchedulePreset)
            {
                if (IsCanAdd(st)) result.Add(st);
            }
        }
        System.Random random = new System.Random();
        int i = random.Next(result.Count);

        return result[i];
    }
}
public class ScheduleTimePreset
{
    public readonly ReadOnlyCollection<ScheduleTime> partTimeSchedulePreset;
    public readonly ReadOnlyCollection<ScheduleTime> lessonSchedulePreset;
    public ScheduleTimePreset()
    {
        List<ScheduleTime> temp = new List<ScheduleTime>();
        for (int startTime = 9; startTime < 16; startTime += 3)
        {
            for (WeekDay weekDay = WeekDay.monday; weekDay <= WeekDay.friday; weekDay++)
            {
                temp.Add(new ScheduleTime(weekDay, startTime, 3));
            }
        }
        lessonSchedulePreset = temp.AsReadOnly();
        List<ScheduleTime> temp2 = new List<ScheduleTime>();
        for (int startTime = 9; startTime < 24; startTime += 1)
        {
            for (WeekDay weekDay = WeekDay.monday; weekDay <= WeekDay.sunday; weekDay++)
            {
                for (int duration = 1; duration + startTime <= 24 && duration < 6; duration++)
                    temp2.Add(new ScheduleTime(weekDay, startTime, duration));
            }
        }
        partTimeSchedulePreset = temp2.AsReadOnly();
    }
}
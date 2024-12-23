using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtillTime;

public class ScheduleContainer : MonoBehaviour
{
    public TMP_Text ScheduleNameTMP;
    public TMP_Text DescriptionTMP;
    public TMP_Text TimeDescriptionTMP;
    public Schedule schedule;
    public ScheduleView view;
    public bool isLeft;
    [SerializeField] Button applyButton;
    public void Init(ScheduleView view, bool isLeft)
    {
        this.isLeft = isLeft;
        this.view = view;
    }

    public void SetSchedule(Schedule s)
    {
        ScheduleNameTMP.text = s.name;
        string weekday = Utill.WeekDayToString(s.scheduleTime.weekDay);
        TimeDescriptionTMP.text = $"{weekday}\n{s.scheduleTime.startHour}~{s.scheduleTime.startHour + s.scheduleTime.timeDuration}";
        if (s is Lesson)
        {
            Lesson l = s as Lesson;
            string dif = Lesson.DifficultyToString(l.difficulty);
            DescriptionTMP.text = $"난이도: {dif}\n과제량: {l.reportProbability}";
        }
        else if (s is PartTime)
        {
            PartTime p = s as PartTime;
            DescriptionTMP.text = $"수입: {p.rewardMoney}\n멘탈: -{p.costMental}\n스테미나: -{p.costStemina}";
        }
        schedule = s;
        if (!isLeft && schedule is Lesson && view.inGameManager.inGameState != InGameState.newGame)
        {
            applyButton.gameObject.SetActive(false);
        }
    }
    public void OnPushButton()
    {
        Debug.Log("click");
        view.OnScheduleButton(schedule, isLeft);
    }
}
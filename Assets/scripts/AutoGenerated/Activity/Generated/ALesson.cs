using System.Collections;
using UnityEngine.SceneManagement;
/*
수강
*/
public class ALesson : Activity
{
    public void UpdateLesson(InGameManager inGameManager)
    {
        calculatedTimeDuration = inGameManager.player.GetNowLesson().scheduleTime.timeDuration;
    }
    public override string simpleDescription => "예정에 있던 수업을 수강한다.\n\"공부는 학생의 본분!\"";

    public override string name => "강의 수강";

    public override int costMoney => 0;
    public override int costMental => 200;
    public override int costStamina => 20;
    public override int rewardMoney => 0;
    public override int rewardMental => 0;
    public override int rewardStamina => 0;
    public override int rewardExp => 100;
    public override int timeDuration => calculatedTimeDuration;
    private int calculatedTimeDuration;
    public override ActivityKind Kind => ActivityKind.Lesson;

    public ALesson()
    {
        _logs = new string[]{
            "강의실에 도착했다.","강의를 들으며 피로가 누적된다.","강의 내용을 하나도 이해하지 못했다..","어느정도는 강의를 이해한 것 같다..","무난하게 수업을 마쳤다.","완벽하게 이해했다!","과제가 출제되었다.."
        };
    }
    public override IEnumerator ActivityCoroutine(InGameManager inGameManager)
    {
        throw new System.NotImplementedException();
    }

    public override void DoActivity(InGameManager inGameManager)
    {
        inGameManager.InGameLog(logs[0]);
        inGameManager.MentalDamage(costMental);
        inGameManager.StaminaDamage(costStamina);
        int temp = Utill.random.Next(0, 100);
        GameManager.Inst.nowLesson = inGameManager.player.GetNowLesson();
        if (temp < inGameManager.player.GetNowLesson().reportProbability)
        {
            inGameManager.InGameLog(logs[6]);
            inGameManager.GetBuff(BuffKind.report, inGameManager.player.activatedBuffs.ContainsKey(BuffKind.report) ? inGameManager.player.activatedBuffs[BuffKind.report].stack + 1 : 1);
        }

        int dif = 0;
        if (inGameManager.player.GetNowLesson().difficulty == Lesson.LessonDifficulty.Easy) dif = 0;
        if (inGameManager.player.GetNowLesson().difficulty == Lesson.LessonDifficulty.Normal) dif = 1;
        if (inGameManager.player.GetNowLesson().difficulty == Lesson.LessonDifficulty.Hard) dif = 2;
        inGameManager.TimeLeft(calculatedTimeDuration);
        GameManager.Inst.battleDificalty = dif;
        GameManager.Inst.player = inGameManager.player;
        GameManager.Inst.scheduleManager = inGameManager.scheduleManager;
        GameManager.Inst.gameState = GameState.Battle;
        SceneManager.LoadScene("BattleScene");
        /**/
    }

    public override bool UpdateSelectable(InGameManager inGameManager)
    {
        if (
            inGameManager.scheduleManager.IsCanActivity(inGameManager.player.GetWeekday, inGameManager.player.GetHour) &&
            inGameManager.scheduleManager.IsLesson(inGameManager.player.GetWeekday, inGameManager.player.GetHour))
        {
            calculatedTimeDuration = inGameManager.player.GetNowLesson().scheduleTime.timeDuration;

            this.isSelectable = true;
            return true;
        }
        isSelectable = false;
        return false;
    }
}
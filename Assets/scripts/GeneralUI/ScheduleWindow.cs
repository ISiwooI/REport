

public class ScheduleWindow : Window
{
    public InGameManager inGameManager;
    public override void CloseWindow()
    {
        if (inGameManager.inGameState == InGameState.newGame)
        {
            if (inGameManager.scheduleManager.scheduleList.Count < 5)
            {
                inGameManager.InGameLog("수업이 너무 적다. 조금 더 신청하자", 0.0f);
            }
            else if (inGameManager.scheduleManager.scheduleList.Count > 7)
            {
                inGameManager.InGameLog("수업이 너무 많다. 조금 줄일 필요가 있다.", 0.0f);
            }
            else
            {
                inGameManager.inGameState = InGameState.standby;
                base.CloseWindow();
            }
        }
        else
        {
            base.CloseWindow();
        }
    }
}
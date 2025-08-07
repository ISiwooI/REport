
public class GameManager
{
    public GameState gameState = GameState.NewGame;
    public Player player;
    public ScheduleManager scheduleManager;
    public Lesson nowLesson;
    public int battleresult = 0;
    public int battleDificalty = 2;


    private static GameManager instance;
    public static GameManager Inst
    {
        get
        {
            if (null == instance)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
}
public enum GameState
{
    TitleMenu, CustomizeMenu, MainMenu, Battle, TestMode, LoadData, NewGame, BattleResult
}

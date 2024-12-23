using System.Collections;

namespace BattleListnenrs
{
    public delegate bool BuffCondition(BattleBuff battleBuff);
    public delegate IEnumerator BattleTaskListener(BattleManager manager, BattleActor actor);
    public delegate void BattleListener(BattleManager manager, BattleActor actor);
    public delegate void BattleListener<T>(BattleManager manager, BattleActor actor, T t);
    public delegate void BattleListener<T, U>(BattleManager manager, BattleActor actor, T t, U u);
    public delegate void BattleListener<T, U, V>(BattleManager manager, BattleActor actor, T t, U u, V v);
}
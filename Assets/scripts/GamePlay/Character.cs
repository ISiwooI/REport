[System.Serializable]
public class Character
{
    public bool isPlayer;
    public CharacterCustomizeData cutomizeData;
    public StatusLV statLV;
}
public class StatusLV
{
    /*
공격력: 공격 카드 사용시 계수
방어력: 방어 카드 사용시 일부 계수, 피해 감소
체력: 최대 체력
속도: 수치가 높을 수록 같은턴에 먼저 행동함
행동력: 턴마다 행동할 확률
-회피율: 공격을 회피 할 확률
-저항력: cc기를 버틸 확률
스테: 학과 카드 사용시 코스트
스테재생:  턴마다 회복하는 스테미나
회복력:  회복, 쉴드 지원 카드 계수
    */
    public int statPoint => level - totalLV;
    public int level;
    public int totalLV => atkLV + defLV + hpLV + speedLV + dynamismLV + dodgeLV + resistanceLV + staminaLV + staminaRecoveryLV + supportLV;
    public int atkLV;
    public int defLV;
    public int hpLV;
    public int speedLV;
    public int dynamismLV;
    public int dodgeLV;
    public int resistanceLV;
    public int staminaLV;
    public int staminaRecoveryLV;
    public int supportLV;

}
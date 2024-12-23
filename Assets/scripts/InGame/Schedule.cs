
using UtillTime;
[System.Serializable]
public record Schedule
{
    public string name;
    public ScheduleTime scheduleTime;
}
/* 강의 이름 후보
엔티티요리학
엔티티행동학
엔티티 언어학
어디서든잘자기
비상사태 대응법
올바른사망확인법
파괴공작대처법
몸으로배우는방어술
현지조달생존법
방사능환경행동강령
야간환경행동강령
혹한환경행동강령
쉽게 배우는 도주
이기는 전투 설계법
던전 구조
함정 및 엔티티
신중한 전진법
정신이 나가지 않는 법
던전과 커뮤니케이션
동료와 소통
긴급상황 신속 소통법
비언어적 소통
엔티티 도축학
은밀전투
고립행동강령
던전 이동법
올바른 감정not emotion
패닉 방지학
불필요한 전투 회피법
던전에서 나 잃지 않기
엔티티 교란학
보급관리학
청결과 감염
튼튼한 육체
작전 설계와 돌발상황
위화감과 안목
던전의 역사
올바른 엔티티 취급방법
인물로 알아보는 던전사
던전 평화 생명
던전봉사
던전 지형 분석 및 활용
던전생태학
해외 던전 사례
논리와 비판적 사고
비극의 이해
언어와 구현
던전시대의 이해
던전 노동사
던전과 엔티티
생활속 던전
확률과 통계
*/
[System.Serializable]
public record Lesson : Schedule
{
    public enum LessonDifficulty
    {
        Easy, Normal, Hard
    }
    public LessonDifficulty difficulty;
    public int reportProbability;
    public Lesson(int reportProbability, LessonDifficulty difficulty, string name, ScheduleTime scheduleTime)
    {
        this.reportProbability = reportProbability;
        this.difficulty = difficulty;
        this.name = name;
        this.scheduleTime = scheduleTime;
    }
    public static string DifficultyToString(LessonDifficulty difficulty)
    {
        switch (difficulty)
        {
            case LessonDifficulty.Easy:
                return "쉬움";
            case LessonDifficulty.Normal:
                return "보통";
            case LessonDifficulty.Hard:
                return "어려움";
            default:
                return "???";
        }
    }

    /*
    과제량
    몹 배치
    베이스 난이도
    */

}
/* 알바 이름 후보
편의점
카페
상하차
pc방
과외
노래방
서빙
서류작업
*/
[System.Serializable]
public record PartTime : Schedule
{
    /*
    코스트
    보상
    버프 디버프
    */
    public int costMental;
    public int costStemina;

    public int rewardMoney;
    public PartTime(string name, int costMental, int costStamina, int rewardMoney, ScheduleTime scheduleTime)
    {
        this.name = name;
        this.costMental = costMental;
        this.costStemina = costStamina;
        this.rewardMoney = rewardMoney;
        this.scheduleTime = scheduleTime;
    }

}

[System.Serializable]
public struct ScheduleTime
{

    public ScheduleTime(WeekDay weekDay, int startHour, int timeDuration)
    {
        this.weekDay = weekDay;
        this.startHour = startHour;
        this.timeDuration = timeDuration;
    }
    public WeekDay weekDay;
    public int startHour;
    public int timeDuration;
}
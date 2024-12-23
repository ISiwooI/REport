using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
namespace UtillTime
{
    /// <summary>
    /// 방학, 학기 구분
    /// </summary>
    public enum SessionState
    {
        winterVac, firstSession, summerVac, secondSession
    }
    /// <summary>
    /// 학년 int로 교체
    /// </summary>
    public enum Grade
    {
        first, second, third, forth, fifth
    }
    /// <summary>
    /// 연차 삭제or int 로 교체
    /// </summary>
    public enum Year
    {
        first, second, third, forth, fifth, sixth
    }
    /// <summary>
    /// 요일 int로 교체
    /// </summary>
    public enum WeekDay
    {
        monday, tuesday, wednesday, thursday, friday, saturday, sunday
    }

}
/*캐릭터 커스터마이즈 파라미터
[System.Serializable]
public struct CustomizeParameters
{
    public int hair;
    public Color hairColor;
    public int eyes;
    public Color eyesColor;
    public int top;
    public int bottom;
    public int department;
    public int body;
}
*/
public class Utill
{
    public static string DepartmentToString(DepartmentKind kind)
    {
        switch (kind)
        {
            case DepartmentKind.None:
                return "일반";
            case DepartmentKind.Camical:
                return "화학공학과";
            case DepartmentKind.Sport:
                return "스포츠학과";
            case DepartmentKind.Theology:
                return "신학과";
        }
        return "";

    }
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            int k = Utill.random.Next(n--);
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }
    public static System.Random random = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF); //랜덤 시드값
    public static string WeekDayToString(UtillTime.WeekDay weekDay)
    {
        switch (weekDay)
        {
            case UtillTime.WeekDay.monday:
                return "월요일";
            case UtillTime.WeekDay.tuesday:
                return "화요일";
            case UtillTime.WeekDay.wednesday:
                return "수요일";
            case UtillTime.WeekDay.thursday:
                return "목요일";
            case UtillTime.WeekDay.friday:
                return "금요일";
            case UtillTime.WeekDay.saturday:
                return "토요일";
            case UtillTime.WeekDay.sunday:
                return "일요일";
            default:
                return "알 수 없는 요일"; // 기본값 추가
        }
    }
    /// <summary>
    /// 랜덤한 문자열 반환
    /// </summary>
    /// <param name="_nLength">문자열 길이</param>
    /// <returns>랜덤 문자열</returns>
    public static string RandomString(int _nLength = 12)
    {
        const string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  //문자 생성 풀
        char[] chRandom = new char[_nLength];

        for (int i = 0; i < _nLength; i++)
        {
            chRandom[i] = strPool[random.Next(strPool.Length)];
        }
        string strRet = new String(chRandom);   // char to string
        return strRet;
    }

    public enum TypeEnum { Float, Int, Bool, String };


    public static string saveDataPath()
    {
        return Application.dataPath + "/saveData";
    }
    public static string dataSlotPath(int i)
    {
        return saveDataPath() + "/Slot" + i;
    }
    public static void CopyDirectory(string sourceFolder, string destFolder)
    {
        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);

        string[] files = Directory.GetFiles(sourceFolder);
        string[] folders = Directory.GetDirectories(sourceFolder);

        foreach (string file in files)
        {
            string name = Path.GetFileName(file);
            string dest = Path.Combine(destFolder, name);
            File.Copy(file, dest);
        }

        foreach (string folder in folders)
        {
            string name = Path.GetFileName(folder);
            string dest = Path.Combine(destFolder, name);
            CopyDirectory(folder, dest);
        }
    }
    /// <summary>
    /// content의 문자열을 정규식 패턴을 이용해서 changstr의 문자열로 바꿈
    /// </summary>
    /// <param name="content">오리지날</param>
    /// <param name="reg">정규식 패턴</param>
    /// <param name="changestr">바꿀 문자열</param>
    /// <returns>바꾼 문자열</returns>
    public static string replaceStartToEnd(string content, string reg, string changestr)
    {
        return Regex.Replace(content, reg, changestr);
    }
}


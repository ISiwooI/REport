using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        //     TextAsset data = Resources.Load(file) as TextAsset;
        string data = File.ReadAllText(file);
        var lines = Regex.Split(data, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                bool b;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                else if (bool.TryParse(value, out b))
                {
                    finalvalue = b;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    /// <summary>
    /// List<Dictionary<string, object>> 의 딕셔너리가 string 배열의 키 값을 모두 가졌는지 체크
    /// </summary>
    /// <param name="dl">대상 리스트 딕셔너리</param>
    /// <param name="strings">확인할 키 값들</param>
    /// <returns>키 값을 가졌는지</returns>
    public static bool HasKey(List<Dictionary<string, object>> dl, string[] strings)
    {
        try
        {
            if (dl.Count == 0) { Debug.LogWarning("Utill.cs, ListDicIsUniqueName: 비어있는 CSV"); return false; }
            foreach (string s in strings)
            {
                if (!dl[0].ContainsKey(s)) { Debug.LogWarning("csv Has not key:" + s); return false; }
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Utill.cs, ListDicHasKey: " + e.ToString());
            return false;
        }


        return true;
    }
    /// <summary>
    /// List<Dictionary<string, object>> 의 딕셔너리가 해당 키를 가졌는지 체크
    /// </summary>
    /// <param name="dl">대상 리스트 딕셔너리</param>
    /// <param name="s">확인할 키 값</param>
    /// <returns></returns>
    public static bool HasKey(List<Dictionary<string, object>> dl, string s)
    {
        try
        {
            if (dl.Count == 0) { Debug.LogWarning("Utill.cs, ListDicIsUniqueName: 비어있는 CSV"); return false; }

            if (!dl[0].ContainsKey(s)) { Debug.LogWarning("csv Has not key:" + s); return false; }

            return true;
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("CSVReader, ListDicHasKey: " + e.ToString());
            return false;
        }

    }

    public static bool ColumnTypeCheck(List<Dictionary<string, object>> ld, string s, Utill.TypeEnum type)
    {
        try
        {
            if (ld.Count == 0) { Debug.LogWarning("blank CSV"); return false; }
            if (!HasKey(ld, s)) { Debug.LogWarning("HasNotKey"); return false; }

            foreach (Dictionary<string, object> entry in ld)
            {
                switch (type)
                {
                    case Utill.TypeEnum.String:
                        if (!(entry[s] is string)) { Debug.LogWarning(entry[s] + " is " + entry[s].GetType().ToString()); return false; }
                        else
                            break;
                    case Utill.TypeEnum.Bool:
                        if (!(entry[s] is bool)) { Debug.LogWarning(entry[s] + " is " + entry[s].GetType().ToString()); return false; }
                        else
                            break;
                    case Utill.TypeEnum.Int:
                        if (!(entry[s] is int)) { Debug.LogWarning(entry[s] + " is " + entry[s].GetType().ToString()); return false; }
                        else
                            break;
                    case Utill.TypeEnum.Float:
                        if (!(entry[s] is float)) { Debug.LogWarning(entry[s] + " is " + entry[s].GetType().ToString()); return false; }
                        else
                            break;
                }
                return true;
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogException(e);
            return false;
        }
        return true;
    }

    public static bool isUnique(List<Dictionary<string, object>> dl, string column)
    {
        if (!HasKey(dl, column)) { Debug.LogWarning("CSV has not contain " + column); return false; }
        try
        {
            if (dl.Count == 0) { Debug.LogWarning("CSVReader, isUnique: 비어있는 CSV"); return false; }

            foreach (Dictionary<string, object> dic in dl)
            {
                int i = 0;
                foreach (Dictionary<string, object> dic2 in dl)
                {
                    if (dic2[column] == dic[column]) i++;
                    if (i > 1) { Debug.Log("CSV Has Not Unique value:" + dic[column]); return false; }
                }
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Utill.cs, isUnique: " + e.ToString());
            return false;
        }
        return true;
    }
}

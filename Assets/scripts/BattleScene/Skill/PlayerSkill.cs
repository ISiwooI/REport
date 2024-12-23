using System;

using UnityEngine;
public abstract class PlayerSkill : Skill
{
    public abstract int cardImageIndex { get; }
    public abstract DepartmentKind departmentKind { get; }
    public abstract int departmentCost { get; }
    public abstract int normalCost { get; }
    public override string ToString()
    {
        try
        {
            string result = $"{name}\n{Utill.DepartmentToString(departmentKind)}\n코스트: {normalCost}";
            if (departmentKind != DepartmentKind.None) result += $"\n학과 코스트: {departmentCost}";
            result += $"\n{description}";
            return result;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
            return "exception";
        }
    }

}
public enum DepartmentKind
{
    None,
    Camical,
    Sport,
    Theology
}
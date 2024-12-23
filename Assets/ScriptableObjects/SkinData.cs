
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Parts
{
    public string name;

}
[System.Serializable]
public class singleSpriteParts : Parts
{
    public Sprite sprite;
}
[System.Serializable]
public class bodyParts : Parts
{
    public Sprite body;
    public Sprite head;
    public Sprite leftArm;
    public Sprite rightArm;
    public Sprite leftFoot;
    public Sprite rightFoot;
}
[System.Serializable]
public class eyeParts : Parts
{
    public Sprite back;
    public Sprite front;
}

[System.Serializable]
public class ClothParts : Parts
{
    public Sprite body;
    public Sprite right;
    public Sprite left;
}
[System.Serializable]
public class PantsParts : Parts
{
    public Sprite left;
    public Sprite right;
}
[System.Serializable]
public class departmentParts : Parts
{
    public Sprite armor;
    public Sprite rightArmor;
    public Sprite leftArmor;
    public Sprite rightWeapon;
    public Sprite leftWeapon;
    public Sprite back;
}



[CreateAssetMenu(fileName = "SkinSO", menuName = "ScriptableObject/SkinSO")]
public class SkinData : ScriptableObject
{
    public singleSpriteParts[] hairs;
    public eyeParts[] eyes;
    public ClothParts[] tops;
    public PantsParts[] pants;
    public departmentParts[] departmentSkin;
    public bodyParts[] bodys;
    public static List<string> partsListToStringList(Parts[] list)
    {
        List<string> result = new List<string>();
        foreach (Parts p in list)
        {
            result.Add(p.name);
        }
        return result;
    }
}

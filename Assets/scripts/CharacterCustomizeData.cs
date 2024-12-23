using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CharacterCustomizeData
{
    public CharacterCustomizeData(string name, int hairIndex, int eyesIndex, int topIndex, int bottomIndex, int departmentIndex, int skinColorIndex)
    {
        this.name = name;
        this.hairIndex = hairIndex;
        this.eyesIndex = eyesIndex;
        this.topIndex = topIndex;
        this.bottomIndex = bottomIndex;
        this.departmentIndex = departmentIndex;
        this.skinColorIndex = skinColorIndex;
    }
    public string name;
    public int hairIndex;
    public int eyesIndex;
    public int topIndex;
    public int bottomIndex;
    public int departmentIndex;
    public int skinColorIndex;
}


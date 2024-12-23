
using UnityEngine;
using System.IO;


public class IOManager : MonoBehaviour
{
    private static IOManager instance = null;
    private static GameObject obj = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    public static IOManager Inst
    {
        get
        {
            if (null == instance)
            {
                obj = new GameObject("IOManager");
                obj.AddComponent<IOManager>();
                return instance;
            }
            return instance;
        }
    }
    public void generateDirectory()
    {
        if (!Directory.Exists(Utill.saveDataPath()))
        {
            Directory.CreateDirectory(Utill.saveDataPath());
        }
    }
    public void generateNewDataSlot(int Slot)
    {
        generateDirectory();
        if (!Directory.Exists(Utill.dataSlotPath(Slot)))
        {
            Directory.CreateDirectory(Utill.dataSlotPath(Slot));
        }
    }
    public void saveCustomizeData(int Slot, CustomizeData data)
    {

    }
    public CustomizeData loadCustomizeData(int Slot)
    {
        CustomizeData result = new CustomizeData();
        return result;
    }
}
#region 데이터 스트림 클래스
public class ProggressData
{

}
[System.Serializable]
public class CharacterData
{

}
[System.Serializable]
public struct CustomizeData
{
    public CustomizeData(string name, int eyes, int hair, int top, int bottom, int department, int SkinColor)
    {
        this.name = name;
        this.top = top;
        this.eyes = eyes;
        this.SkinColor = SkinColor;
        this.hair = hair;
        this.department = department;
        this.bottom = bottom;
    }
    public string name;
    public int eyes;
    public int hair;
    public int top;
    public int bottom;
    public int department;
    public int SkinColor;
}
[System.Serializable]
public struct StatusLevelData
{
    public int atkLv;
    public int defLv;
    public int hpLv;
    public int spdLv;
    public int supLv;
    public int lucLv;
}
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffContainer : MonoBehaviour
{
    [SerializeField] TMP_Text bufftmp;
    [SerializeField] ContentSizeFitter contentSizeFitter;
    InGameLogView view;
    public void Init(InGameLogView view)
    {
        this.view = view;
    }
    public void SetBuff(Buff buff)
    {
        string des = buff.simpleDescription;
        bufftmp.text = $"{buff.name}\n{des}";
    }
}
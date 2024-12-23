/*
updateview
Onview closed
Onview opened
*/
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
public class View : MonoBehaviour
{
    public event Action OnWindowClosed;
    public event Action OnWindowOpened;
    public InGameManager inGameManager;
    protected virtual void Awake()
    {
        if (inGameManager == null)
        {
            inGameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();
        }
    }

    public virtual void CloseWindow()
    {
        OnWindowClosed?.Invoke();
    }
    public virtual void OpenWindow()
    {
        UpdateView();
        OnWindowOpened?.Invoke();
    }

    public virtual void UpdateView() { }
}

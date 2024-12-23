using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum WindowStat
{
    Closed, Opened
}
public class Window : MonoBehaviour
{

    [SerializeField] protected bool EnableCloseButton;
    [SerializeField] protected string TitlebarText;

    [SerializeField] Button CloseButton;
    [SerializeField] TMP_Text titlebarTMP;
    [SerializeField] public UnityEvent<Window> UEOnDestroy;
    [SerializeField] public UnityEvent<Window> UEOnClose;

    [SerializeField] public UnityEvent<Window> UEOnOpen;
    [SerializeField] public View view;
    protected WindowStat windowStat;

    public void Init(bool EnableCloseButton, string TitlebarText)
    {
        this.TitlebarText = TitlebarText;
        this.EnableCloseButton = EnableCloseButton;
        this.CloseButton.onClick.AddListener(CloseWindow);
        CloseButton.gameObject.SetActive(EnableCloseButton);
        titlebarTMP.text = this.TitlebarText;
    }
    protected virtual void Awake()
    {
        Init(this.EnableCloseButton, TitlebarText);
    }

    public virtual void CloseWindow()
    {
        gameObject.SetActive(false);
        windowStat = WindowStat.Closed;
        UEOnClose.Invoke(this);
        view?.CloseWindow();
    }
    public virtual void OpenWindow()
    {
        gameObject.SetActive(true);
        windowStat = WindowStat.Opened;
        UEOnOpen.Invoke(this);
        view?.OpenWindow();

    }
    protected void OnDestroy()
    {
        UEOnDestroy.Invoke(this);
    }
}

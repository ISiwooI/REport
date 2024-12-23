using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupWindow : Window
{
    [SerializeField] public bool EnableCancelButton;
    [SerializeField] public bool EnableApplyButton;
    [SerializeField] Button CancelButton;
    [SerializeField] Button ApplyButton;
    [SerializeField] public string MainText;
    [SerializeField] public TMP_Text mainTMP;
    [SerializeField] public UnityEvent UEOnCancelButtonDown;
    [SerializeField] public UnityEvent UEOnApplyButtonDown;
    public void Init(bool EnableCloseButton, string TitlebarText, bool EnableApplyButton, bool EnableCancelButton, string MainText)
    {

        base.Init(EnableCloseButton, TitlebarText);
        this.MainText = MainText;
        mainTMP.text = MainText;
        this.EnableApplyButton = EnableApplyButton;
        this.EnableCancelButton = EnableCancelButton;
        CancelButton.gameObject.SetActive(EnableCancelButton);
        ApplyButton.gameObject.SetActive(EnableApplyButton);
    }
    protected override void Awake()
    {
        Init(EnableCloseButton, TitlebarText, EnableApplyButton, EnableCancelButton, MainText);
    }

    public void OnCancelButtonDown()
    {
        UEOnCancelButtonDown.Invoke();
        Destroy(gameObject);

    }
    public void OnApplyButtonDown()
    {
        UEOnApplyButtonDown.Invoke();
        Destroy(gameObject);
    }

}

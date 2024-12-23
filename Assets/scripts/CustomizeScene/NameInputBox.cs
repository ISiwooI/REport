using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NameInputBox : MonoBehaviour
{
    [SerializeField]public TMP_InputField inputField;
    [SerializeField]public UnityEvent<String> OnApplyButtonDown;
    [SerializeField]public UnityEvent OnExitButtonDown;
    [SerializeField]Button exitButton;
    [SerializeField]Button applyButton;
    
    // Start is called before the first frame update
    void Awake()
    {
        inputField.onValueChanged.AddListener((word)=>inputField.text=Regex.Replace(word,@"[^0-9a-zA-Z가-힣]",""));
        exitButton.onClick.AddListener(()=>OnExitButtonDown.Invoke());
        applyButton.onClick.AddListener(()=>OnApplyButtonDown.Invoke(inputField.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

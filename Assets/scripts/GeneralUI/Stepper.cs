using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Stepper : MonoBehaviour
{
    public UnityEvent<int> OnUpdateStepper;
    
    [SerializeField]
    TMP_Text TMP;
    [SerializeField]
    Button leftbutton;
    [SerializeField]
    Button rightbutton;
    public int index;
    [SerializeField]
    public List<string> strings;
    int Length=>strings.Count-1;
    public void OnRightButtonDown(){
        if(++index>Length){
            index=0;
        }
        updateTMP();
    }
    public void OnLeftButtonDown(){
        if(--index<0){
            index=Length;
        }
        updateTMP();
    }
    void updateTMP(){
        if(strings.Count!=0){
            if(Length<index)index=Length;
            if(0>index)index=0;
            TMP.text=strings[index];
        }
        OnUpdateStepper.Invoke(index);
        
    }
    public void SetList(List<string> strings){
        this.strings=strings;
        updateTMP();
    }
    private void Start() {
        updateTMP();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomizeMenu : MonoBehaviour
{
    #region serializefield
    [SerializeField] SkinData skinData;
    [SerializeField] PopupCanvas popupCanvas;
    [SerializeField] TMP_Text PlayerNameTMP;
    [Header("UnityEvents")]
    [SerializeField] UnityEvent<singleSpriteParts> UEOnHairChange;
    [SerializeField] UnityEvent<bodyParts> UEOnBodyChange;
    [SerializeField] UnityEvent<eyeParts> UEOnEyeChange;
    [SerializeField] UnityEvent<ClothParts> UEOnTopChange;
    [SerializeField] UnityEvent<PantsParts> UEOnBottomChange;
    [SerializeField] UnityEvent<departmentParts> UEOnDepartmentChange;
    [SerializeField] UnityEvent<bodyParts> UEOnSkinColorChange;
    [Header("UI")]
    [SerializeField] Button NameButton;
    [SerializeField] Stepper hairStepper;
    [SerializeField] Stepper eyesStepper;
    [SerializeField] Stepper topStepper;
    [SerializeField] Stepper buttomStepper;
    [SerializeField] Stepper departmentStepper;
    [SerializeField] Stepper SkinColorStepper;
    [SerializeField] NameInputBox nameInputBox;
    #endregion serializefield
    bool NameInputBoxAvailable = false;
    string playerName = "";
    // Start is called before the first frame update
    void Awake()
    {
        NameButton.onClick.AddListener(() =>
        {
            if (NameInputBoxAvailable == false)
            {
                nameInputBox.gameObject.SetActive(true);
                nameInputBox.inputField.text = "";
                NameInputBoxAvailable = true;
            }
        });
        nameInputBox.OnExitButtonDown.AddListener(() =>
        {
            if (NameInputBoxAvailable == true)
            {
                NameInputBoxAvailable = false;
                nameInputBox.gameObject.SetActive(false);
            }
        });
        nameInputBox.OnApplyButtonDown.AddListener((text) =>
        {
            if (NameInputBoxAvailable == true)
            {
                NameInputBoxAvailable = false;
                playerName = nameInputBox.inputField.text;
                PlayerNameTMP.text = (playerName == "") ? "이름" : playerName;
                nameInputBox.gameObject.SetActive(false);
            }
        });
        hairStepper.SetList(SkinData.partsListToStringList(skinData.hairs));
        eyesStepper.SetList(SkinData.partsListToStringList(skinData.eyes));
        topStepper.SetList(SkinData.partsListToStringList(skinData.tops));
        buttomStepper.SetList(SkinData.partsListToStringList(skinData.pants));
        departmentStepper.SetList(SkinData.partsListToStringList(skinData.departmentSkin));
        SkinColorStepper.SetList(SkinData.partsListToStringList(skinData.bodys));
    }
    public void OnHairChange(int i)
    {
        UEOnHairChange.Invoke(skinData.hairs[i]);
    }
    public void OnEyeChange(int i)
    {
        UEOnEyeChange.Invoke(skinData.eyes[i]);
    }
    public void OnTopChange(int i)
    {
        UEOnTopChange.Invoke(skinData.tops[i]);
    }
    public void OnBottomChange(int i)
    {
        UEOnBottomChange.Invoke(skinData.pants[i]);
    }
    public void OnDepartmentChange(int i)
    {
        UEOnDepartmentChange.Invoke(skinData.departmentSkin[i]);
    }
    public void OnSkinColorChange(int i)
    {
        UEOnBodyChange.Invoke(skinData.bodys[i]);
    }
    public void OnPlayerNameChange(string name)
    {
        PlayerNameTMP.text = name;
    }
    public void OnApplyButtonDown()
    {
        if (playerName == "")
        {
            popupCanvas.CallPopupWindow(true, "경고!", true, false, "플레이어 이름을 설정해 주십시오!");

        }
        else
        {

            PopupWindow popupWindow = popupCanvas.CallPopupWindow(true, "", true, true, "이대로 캐릭터를 생성하시겠습니까?");
            popupWindow.UEOnApplyButtonDown.AddListener(() =>
            {
                ApplyCharacter();
            });

        }
    }

    void ApplyCharacter()
    {

    }

}

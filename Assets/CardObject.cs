using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class CardObject : MonoBehaviour
{
    public CardManager cardManager;
    public SkillKind skill;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] TMP_Text normalCostTMP;
    [SerializeField] TMP_Text departmentCostTMP;
    [SerializeField] SpriteRenderer cardImage;
    [SerializeField] SpriteRenderer departmentCostSprite;
    [SerializeField] public int Orderindex = 0;
    [SerializeField] AudioSource[] audioSource;
    public SortingGroup sortingGroup;

    bool isSelected = false;
    public bool isUsable = true;
    public bool isDepartment = false;
    private void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
        if (audioSource == null) audioSource = GetComponents<AudioSource>();
    }
    private void Update()
    {
        if (isSelected)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mp.x, mp.y, 0);
        }
    }
    private void OnMouseEnter()
    {

        if (!isSelected) SoundManager.PlaySFX(soundKind.cardPick);
        sortingGroup.sortingOrder = 10000;
        cardManager.battleManager.selectedCard = this;

    }
    private void OnMouseExit()
    {

        sortingGroup.sortingOrder = Orderindex;
        if (!isSelected) cardManager.battleManager.selectedCard = null;

    }
    private void OnMouseDown()
    {
        if (isUsable)
        {

            audioSource[1].Play();
            SoundManager.PlaySFX(soundKind.cardSelect);
            isSelected = true;
        }
    }
    private void OnMouseUp()
    {
        isSelected = false;
        SoundManager.PlaySFX(soundKind.cardUse);
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -0.4f)
        {
            cardManager.AlignCard();
        }
        else
        {

            cardManager.UseCard(this, isDepartment);
        }
    }
    public void SetSelectable(bool b)
    {
        if (!b)
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = Color.gray;
            }
            normalCostTMP.color = Color.gray;
            departmentCostTMP.color = Color.gray;
        }
        else
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = Color.white;
            }
            normalCostTMP.color = Color.white;
            departmentCostTMP.color = Color.white;
        }
        isUsable = b;
    }
    public void InitCard(SkillKind skillKind)
    {
        skill = skillKind;
        Skill target = cardManager.battleManager.skillDelegater.GetSkill(skillKind);
        if (!(target is PlayerSkill)) return;
        PlayerSkill playerSkill = target as PlayerSkill;
        try
        {
            SetImage(cardManager.cardSpriteData.GetSprite(playerSkill.cardImageIndex));
            if (playerSkill.departmentKind != DepartmentKind.None)
            {
                SetCardKind(true);
                SetDepartmentCost(playerSkill.departmentCost);
            }
            else
            {
                SetCardKind(false);
            }
            SetNormalCost(playerSkill.normalCost);
        }
        catch (System.NotImplementedException ex)
        {
            UnityEngine.Debug.LogWarning(ex.ToString());
        }

    }
    public void SetImage(Sprite sprite)
    {
        cardImage.sprite = sprite;
    }
    public void SetNormalCost(int c)
    {
        normalCostTMP.text = c.ToString();
    }
    public void SetDepartmentCost(int c)
    {
        departmentCostTMP.text = c.ToString();
    }
    public void SetCardKind(bool isDepartment)
    {
        if (isDepartment)
        {
            departmentCostSprite.gameObject.SetActive(true);
            this.isDepartment = true;
        }
        else
        {
            departmentCostSprite.gameObject.SetActive(false);
            this.isDepartment = false;
        }
    }
}

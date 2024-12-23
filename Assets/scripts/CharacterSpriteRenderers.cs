using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#region rendererclass
[System.Serializable]
public class BodyRenderers
{
    [SerializeField] public SpriteRenderer head;
    [SerializeField] public SpriteRenderer body;
    [SerializeField] public SpriteRenderer leftArm;
    [SerializeField] public SpriteRenderer rightArm;
    [SerializeField] public SpriteRenderer leftFoot;
    [SerializeField] public SpriteRenderer rightFoot;
}
[System.Serializable]
public class EyesRenderers
{
    [SerializeField] public SpriteRenderer leftFront;
    [SerializeField] public SpriteRenderer rightFront;
    [SerializeField] public SpriteRenderer leftBack;
    [SerializeField] public SpriteRenderer rightBack;
}
[System.Serializable]
public class TopRenderers
{
    [SerializeField] public SpriteRenderer left;
    [SerializeField] public SpriteRenderer right;
    [SerializeField] public SpriteRenderer body;
}
[System.Serializable]
public class BottomRenderers
{
    [SerializeField] public SpriteRenderer left;
    [SerializeField] public SpriteRenderer right;
}
[System.Serializable]
public class DepartmentRenderers
{
    [SerializeField] public SpriteRenderer leftArmor;
    [SerializeField] public SpriteRenderer rightArmor;
    [SerializeField] public SpriteRenderer bodyArmor;
    [SerializeField] public SpriteRenderer back;
    [SerializeField] public SpriteRenderer leftWeapon;
    [SerializeField] public SpriteRenderer rightWeapon;
}
#endregion
public class CharacterSpriteRenderers : MonoBehaviour
{
    [SerializeField] SpriteRenderer hairRend;
    [SerializeField] EyesRenderers eyes;
    [SerializeField] TopRenderers top;
    [SerializeField] BottomRenderers bottom;
    [SerializeField] DepartmentRenderers department;
    [SerializeField] BodyRenderers body;
    [SerializeField] public SkinData skinSO;
    List<SpriteRenderer> AllSR = new List<SpriteRenderer>();
    private void Awake()
    {
        AllSR = GetComponentsInChildren<SpriteRenderer>().ToList<SpriteRenderer>();
    }
    public void ApplyCustomizeData(CharacterCustomizeData data)
    {
        setHairSprite(skinSO.hairs[data.hairIndex]);
        setEyesSprite(skinSO.eyes[data.eyesIndex]);
        setTopSprite(skinSO.tops[data.topIndex]);
        setBottomSprite(skinSO.pants[data.bottomIndex]);
        setDepartmentSprite(skinSO.departmentSkin[data.departmentIndex]);
        SetBodySprite(skinSO.bodys[data.skinColorIndex]);
    }
    public void setHairSprite(singleSpriteParts parts)
    {
        hairRend.sprite = parts.sprite;
    }
    public void setEyesSprite(eyeParts parts)
    {
        eyes.leftBack.sprite = parts.back;
        eyes.rightBack.sprite = parts.back;
        eyes.rightFront.sprite = parts.front;
        eyes.leftFront.sprite = parts.front;
    }
    public void setTopSprite(ClothParts parts)
    {
        top.left.sprite = parts.left;
        top.right.sprite = parts.right;
        top.body.sprite = parts.body;
    }
    public void setBottomSprite(PantsParts parts)
    {
        bottom.left.sprite = parts.left;
        bottom.right.sprite = parts.right;
    }
    public void setDepartmentSprite(departmentParts parts)
    {
        department.leftWeapon.sprite = parts.leftWeapon;
        department.rightWeapon.sprite = parts.rightWeapon;
        department.back.sprite = parts.back;
        department.bodyArmor.sprite = parts.armor;
        department.leftArmor.sprite = parts.leftArmor;
        department.rightArmor.sprite = parts.rightArmor;
    }
    public void SetBodySprite(bodyParts parts)
    {
        body.body.sprite = parts.body;
        body.head.sprite = parts.head;
        body.leftFoot.sprite = parts.leftFoot;
        body.rightFoot.sprite = parts.rightFoot;
        body.leftArm.sprite = parts.leftArm;
        body.rightArm.sprite = parts.rightArm;
    }

}

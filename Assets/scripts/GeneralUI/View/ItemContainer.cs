using TMPro;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text descriptionTMP;
    [SerializeField] TMP_Text amountPriceTMP;
    Item item;
    ShopView shopView;
    bool isLeft;
    public void Init(ShopView shopView, bool isLeft)
    {
        this.isLeft = isLeft;
        this.shopView = shopView;
    }
    public void SetItem(Item item)
    {
        this.item = item;
        nameTMP.text = item.name;
        descriptionTMP.text = item.simpleDescription;
        amountPriceTMP.text = isLeft ? item.price.ToString() : item.itemCount.ToString();
    }
    public void OnPushButton()
    {
        shopView.OnPushButton(isLeft, item);
    }
}
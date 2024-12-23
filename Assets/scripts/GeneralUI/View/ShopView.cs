using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShopView : View
{
    public GameObject shopPrefab;
    public GameObject inventoryPrefab;
    IObjectPool<ItemContainer> shoppool;
    IObjectPool<ItemContainer> inventorypool;
    public GameObject shopContants;
    public GameObject inventoryContants;

    List<ItemContainer> shopContainers = new List<ItemContainer>();
    List<ItemContainer> inventoryContainers = new List<ItemContainer>();
    public override void UpdateView()
    {
        base.UpdateView();
        RemoveAll();
        foreach (Item item in inGameManager.player.inStockItems.Values)
        {
            ItemContainer ic = inventorypool.Get();
            ic.SetItem(item);
        }
        foreach (Item item in inGameManager.itemDelegator.items)
        {
            ItemContainer ic = shoppool.Get();
            ic.SetItem(item);
        }

    }
    protected override void Awake()
    {
        base.Awake();
        shoppool = new ObjectPool<ItemContainer>(() =>
        {
            ItemContainer sc = GameObject.Instantiate(shopPrefab, shopContants.transform).GetComponent<ItemContainer>();
            sc.Init(this, true);
            return sc;
        }
        , (value) =>
        {
            shopContainers.Add(value);
            value.gameObject.SetActive(true);
        }
        , (value) =>
        {
            shopContainers.Remove(value);
            value.gameObject.SetActive(false);
        }
        , (value) =>
        {
            GameObject.Destroy(value);
        }
        , true, 10);
        inventorypool = new ObjectPool<ItemContainer>(() =>
        {
            ItemContainer sc = GameObject.Instantiate(inventoryPrefab, inventoryContants.transform).GetComponent<ItemContainer>();
            sc.Init(this, false);
            return sc;
        }
        , (value) =>
        {
            inventoryContainers.Add(value);
            value.gameObject.SetActive(true);
        }
        , (value) =>
        {
            inventoryContainers.Remove(value);
            value.gameObject.SetActive(false);
        }
        , (value) =>
        {
            GameObject.Destroy(value);
        }
        , true, 10);
    }
    public void OnPushButton(bool isLeft, Item item)
    {
        if (isLeft)
        {
            inGameManager.BuyItem(item.itemKind);
        }
        else
        {
            inGameManager.UseItem(item.itemKind);
        }
    }
    public void RemoveAll()
    {
        for (int i = inventoryContainers.Count - 1; i >= 0; i--)
        {
            ItemContainer sc = inventoryContainers[i];
            inventorypool.Release(sc);
        }
        inventoryContainers.Clear();
        for (int i = shopContainers.Count - 1; i >= 0; i--)
        {
            ItemContainer sc = shopContainers[i];
            shoppool.Release(sc);
        }
        shopContainers.Clear();
    }
}
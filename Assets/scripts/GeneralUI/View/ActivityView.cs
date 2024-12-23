using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Pool;
public class ActivityView : View
{
    [SerializeField]
    GameObject contents;
    [SerializeField]
    ActivityContainer activityContainerPrefab;
    IObjectPool<ActivityContainer> activityPool;

    List<ActivityContainer> Containers = new List<ActivityContainer>();

    protected override void Awake()
    {
        base.Awake();
        activityPool = new ObjectPool<ActivityContainer>(() =>
        {
            ActivityContainer ac = GameObject.Instantiate(activityContainerPrefab, contents.transform).GetComponent<ActivityContainer>();
            ac.Init(this);
            return ac;
        }
        , (value) =>
        {
            Containers.Add(value);
            value.gameObject.SetActive(true);
        }
        , (value) =>
        {
            Containers.Remove(value);
            value.gameObject.SetActive(false);
        }
        , (value) =>
        {
            GameObject.Destroy(value);
        }
        , true, 10);

    }
    private void Start()
    {
        UpdateView();
    }
    public void DisplayActivity(List<Activity> al)
    {
        RemoveAll();
        foreach (Activity a in al)
        {
            AddActivity(a);
        }
    }
    public void AddActivity(Activity activity)
    {
        ActivityContainer ac;
        ac = activityPool.Get();
        ac.SetActivity(activity);
    }
    public void RemoveAll()
    {
        for (int i = Containers.Count - 1; i >= 0; i--)
        {
            ActivityContainer ac = Containers[i];
            activityPool.Release(ac);
        }
        Containers.Clear();
    }
    public override void UpdateView()
    {
        base.UpdateView();
        DisplayActivity(inGameManager.activityDeligator.GetSelectavleActivity());
    }
    public void OnActivityButton(ActivityKind activityKind)
    {
        inGameManager.DoActivity(activityKind);
    }
    /*
    //testarea_____________________________________________________________________
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            activityPool.Get();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveAll();
        }

    }
    //testarea_____________________________________________________________________
    */
}

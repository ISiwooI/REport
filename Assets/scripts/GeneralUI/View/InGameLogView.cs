
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Pool;
using UtillTime;
public class InGameLogView : View
{
    [SerializeField] TMP_Text[] tmps;
    [SerializeField] Queue<Tuple<TMP_Text, ContentSizeFitter>> _logs = new Queue<Tuple<TMP_Text, ContentSizeFitter>>();
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] TMP_Text TimeTMP;
    public Queue<Tuple<string, float>> messeges = new Queue<Tuple<string, float>>();
    [SerializeField] private float messegeDelay = 0.7f;
    public bool canPrint = true;
    float delay;

    [SerializeField] BuffContainer buffContainer;
    ObjectPool<BuffContainer> buffPool;
    [SerializeField] GameObject contents;
    List<BuffContainer> Containers = new List<BuffContainer>();
    protected override void Awake()
    {
        base.Awake();
        foreach (var tmp in tmps)
        {
            _logs.Enqueue(new Tuple<TMP_Text, ContentSizeFitter>(tmp, tmp.gameObject.GetComponent<ContentSizeFitter>()));
            tmp.transform.SetAsLastSibling();
        }
        scrollbar.value = 0;
        buffPool = new ObjectPool<BuffContainer>(() =>
        {
            BuffContainer ac = GameObject.Instantiate(buffContainer, contents.transform).GetComponent<BuffContainer>();
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
        StartCoroutine(messegeDelayCoroutin());
    }
    public void RemoveAll()
    {
        for (int i = Containers.Count - 1; i >= 0; i--)
        {
            BuffContainer bc = Containers[i];
            buffPool.Release(bc);
        }
        Containers.Clear();
    }
    public void UpdateBuff()
    {
        RemoveAll();
        foreach (Buff buff in inGameManager.player.activatedBuffs.Values)
        {
            AddBuff(buff);
        }
    }
    public void AddBuff(Buff buff)
    {
        BuffContainer ac;
        ac = buffPool.Get();
        ac.SetBuff(buff);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)ac.transform);
    }
    public override void UpdateView()
    {
        base.UpdateView();
        UpdateBuff();
        TimeUpdate();
    }
    public void TimeUpdate()
    {
        string result;
        string week = "월요일";
        switch (inGameManager.player.GetWeekday)
        {
            case WeekDay.monday:
                week = "월요일";
                break;
            case WeekDay.tuesday:
                week = "화요일";
                break;
            case WeekDay.wednesday:
                week = "수요일";
                break;
            case WeekDay.thursday:
                week = "목요일";
                break;
            case WeekDay.friday:
                week = "금요일";
                break;
            case WeekDay.saturday:
                week = "토요일";
                break;
            case WeekDay.sunday:
                week = "일요일";
                break;
        }
        result = inGameManager.player.GetSemester + "학기 " + inGameManager.player.GetWeek + $"주차\n{week}\n" + inGameManager.player.GetHour.ToString("D2") + ":00";
        TimeTMP.text = result;
    }
    private void Update()
    {
        if (canPrint && messeges.Count != 0)
        {
            Tuple<string, float> tmp = messeges.Dequeue();
            PrintLog(tmp.Item1);
            delay = tmp.Item2;
        }
    }
    private IEnumerator messegeDelayCoroutin()
    {
        while (true)
        {
            yield return new WaitUntil(() => !canPrint);
            yield return new WaitForSeconds(delay);
            canPrint = true;
        }
    }
    void PrintLog(string str)
    {
        var value = _logs.Dequeue();

        value.Item1.text = str;
        value.Item1.transform.SetAsLastSibling();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)value.Item2.transform);
        _logs.Enqueue(value);

        scrollbar.value = 0;
        canPrint = false;
    }
    //test area-------------------------------------------------------------------test area//
    /*
    private void Update()
    {

        string rs = Utill.RandomString(100);
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(rs);
            PrintLog(rs);
        }
    }
    */
    //----------------------------------test area--------------------------------------------------------
}
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class StageLog : MonoBehaviour
{
    TMP_Text tmp;
    Queue<Tuple<float, string>> logQueue = new Queue<Tuple<float, string>>();
    bool isPrinting = false;
    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        if (logQueue.Count > 0 && isPrinting == false)
        {
            isPrinting = true;
            Tuple<float, string> t = logQueue.Dequeue();
            tmp.text = t.Item2;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(1.0f, 0.7f).SetEase(Ease.OutElastic));
            sequence.AppendInterval(t.Item1);
            sequence.Append(transform.DOScale(0f, 0.7f).SetEase(Ease.InElastic));
            sequence.OnComplete(() =>
            {
                isPrinting = false;
            });

        }
    }
    public void Print(string message, float duration = 1f)
    {
        logQueue.Enqueue(new Tuple<float, string>(duration, message));
    }
}

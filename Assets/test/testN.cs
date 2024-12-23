using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class testN : MonoBehaviour
{
    // Start is called before the first frame update
    public Queue<string> queue = new Queue<string>();
    public RemoteClass remoteClass;
    public delegate void Remote();
    public event Remote remoteEventHandler;
    public Remote remote;
    public SuddenEventSample sample = new SuddenEventSample();
    public inFuncClass inFuncClass;
    private void FixedUpdate()
    {

    }
    IEnumerator DelayLog()
    {
        while (true)
        {
            yield return new WaitUntil(() => queue.Count != 0);
            yield return new WaitForSeconds(0.5f);
            Debug.Log(queue.Dequeue());
        }
    }
    private void Awake()
    {
        StartCoroutine(DelayLog());

    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //클래스 할당
            inFuncClass = remoteClass.GetInFuncClass("name", 1, 2.3f, false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            inFuncClass.Print();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            remote?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            remote = remoteClass.Remote;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            remote = remoteClass.ReRemote;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            remote -= remoteClass.Remote;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            remote -= remoteClass.ReRemote;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            remoteEventHandler += remoteClass.ReRemote;

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            remoteEventHandler += remoteClass.Remote;

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            remoteEventHandler?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            queue.Enqueue("0.0");
            queue.Enqueue("0.5");
            queue.Enqueue("1.0");
            queue.Enqueue("1.5");
            queue.Enqueue("2.0");
            queue.Enqueue("2.5");
            queue.Enqueue("3.0");
            queue.Enqueue("3.5");
            queue.Enqueue("4.0");
            queue.Enqueue("4.5");

        }

    }
    void DefRemote(RemoteClass remoteClass)
    {
        remoteClass.Remote();
    }
    void ReRemote(RemoteClass remoteClass)
    {
        remoteClass.ReRemote();
    }

}

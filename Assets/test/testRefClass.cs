using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRefClass : MonoBehaviour
{
    // Start is called before the first frame update
    public RemoteClass remoteClass;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Remote(ref remoteClass);

    }
    void Remote(ref RemoteClass remoteClass)
    {
        remoteClass.Remote();
    }
}

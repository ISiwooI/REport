using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class inFuncClass
{//asdas
    public string name;
    public int n;
    public float f;
    public bool b;
    public inFuncClass(string name, int n, float f, bool b)
    {
        this.name = name;
        this.n = n;
        this.f = f;
        this.b = b;
    }
    public void Print()
    {
        Debug.Log("name: " + name + "\nint: " + n + "\nfloat: " + f + "\nbool: " + b + "\n");
    }
}
public class RemoteClass : MonoBehaviour
{
    int i = 0;
    // Start is called before the first frame update
    public inFuncClass GetInFuncClass(string name, int n, float f, bool b)
    {
        return new inFuncClass(name, n, f, b);
    }
    public void Remote()
    {
        i++;
        Debug.Log("Remoted.\ni=" + i);
    }
    public void ReRemote()
    {
        Debug.Log("ReRe");
    }
}

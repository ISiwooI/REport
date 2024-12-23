using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    public Transform hp;
    public Transform shield;
    // Update is called once per frame
    public void SetHP(float f)
    {
        if (f < 0) f = 0;
        if (f > 1) f = 1;
        hp.DOScaleX(f, 0.4f);
    }
    public void SetShield(float f)
    {
        if (f < 0) f = 0;
        if (f > 1) f = 1;
        shield.DOScaleX(f, 0.4f);
    }
}

using UnityEngine;
using DG.Tweening;
public class PlayerBar : HPBar
{
    public Transform cost;
    public void SetCost(float f)
    {
        if (f < 0) f = 0;
        if (f > 1) f = 1;
        cost.DOScaleX(f, 0.4f);
    }
}
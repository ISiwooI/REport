using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BGLoop : MonoBehaviour
{
    [SerializeField] Transform firstBGTr;
    [SerializeField] Transform secondBGTr;
    [SerializeField] Transform firstOutsideTr;
    [SerializeField] Transform secondOutsideTr;
    [SerializeField] Transform firstDepartTr;
    [SerializeField] Transform secondDepartTr;
    [SerializeField] float relocationXPos;
    [SerializeField] Sprite hardBG;
    [SerializeField] Sprite NormalBG;
    [SerializeField] Sprite EasyBG;
    float xdiff = 0;
    public void LoopMoveBG(float dis)
    {
        dis = -math.abs(dis);
        firstBGTr.Translate(new Vector3(dis, 0, 0));
        secondBGTr.Translate(new Vector3(dis, 0, 0));
        firstOutsideTr.Translate(new Vector3(dis / 4.0f, 0, 0));
        secondOutsideTr.Translate(new Vector3(dis / 4.0f, 0, 0));
        firstDepartTr.Translate(new Vector3(dis / 2.0f, 0, 0));
        secondDepartTr.Translate(new Vector3(dis / 2.0f, 0, 0));
        if (firstBGTr.position.x <= relocationXPos) firstBGTr.Translate(new Vector3(xdiff, 0, 0));
        if (secondBGTr.position.x <= relocationXPos) secondBGTr.Translate(new Vector3(xdiff, 0, 0));
        if (firstOutsideTr.position.x <= relocationXPos) firstOutsideTr.Translate(new Vector3(xdiff, 0, 0));
        if (secondOutsideTr.position.x <= relocationXPos) secondOutsideTr.Translate(new Vector3(xdiff, 0, 0));
        if (firstDepartTr.position.x <= relocationXPos) firstDepartTr.Translate(new Vector3(xdiff, 0, 0));
        if (secondDepartTr.position.x <= relocationXPos) secondDepartTr.Translate(new Vector3(xdiff, 0, 0));
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (GameManager.Inst.battleDificalty)
        {
            case 0:
                firstBGTr.gameObject.GetComponent<SpriteRenderer>().sprite = EasyBG;
                secondBGTr.gameObject.GetComponent<SpriteRenderer>().sprite = EasyBG;
                break;
            case 1:
                firstBGTr.gameObject.GetComponent<SpriteRenderer>().sprite = NormalBG;
                secondBGTr.gameObject.GetComponent<SpriteRenderer>().sprite = NormalBG;
                break;
            case 2:
                firstBGTr.gameObject.GetComponent<SpriteRenderer>().sprite = hardBG;
                secondBGTr.gameObject.GetComponent<SpriteRenderer>().sprite = hardBG;
                break;
        }
        xdiff = secondBGTr.position.x - firstBGTr.position.x;
        xdiff *= 2;
    }


}


using UnityEngine;

public class Costbar : MonoBehaviour
{
    int cost = 10;
    [SerializeField] UnityEngine.UI.Image[] images;
    public void SetCost(int c)
    {
        cost = c;
        foreach (var renderer in images)
        {
            renderer.gameObject.SetActive(false);
        }
        if (cost > 10) cost = 10;
        if (cost < 0) cost = 0;
        for (int i = 0; i < cost; i++)
        {
            images[i].gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) SetCost(Utill.random.Next(0, 11));
    }
}

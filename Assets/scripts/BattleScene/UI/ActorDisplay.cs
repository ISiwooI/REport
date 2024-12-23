using TMPro;
using UnityEngine;

public class ActorDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text stat;
    [SerializeField] TMP_Text buff;

    public void UpdateUI(BattleActor actor)
    {
        stat.text = actor.ToString();
        buff.text = actor.BuffsToString();
    }
}
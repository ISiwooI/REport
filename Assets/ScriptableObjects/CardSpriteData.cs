using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardSprites", menuName = "ScriptableObject/CardSprites")]
public class CardSpriteData : ScriptableObject
{
    [SerializeField] Sprite none;
    [SerializeField] Sprite[] sprites;
    public Sprite GetSprite(int index)
    {
        if (index < 0 || index >= sprites.Length)
        {
            return none;
        }
        else
        {
            return sprites[index];
        }
    }
}

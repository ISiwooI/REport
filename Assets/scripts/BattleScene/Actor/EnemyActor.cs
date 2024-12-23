using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActor : BattleActor
{
    /*
    스킬, 확률, 
    ?? 특정 적에게 특정 버프를 통해 패턴을 제작?
    버프에서 is를 통해 적인지 판단 후 버프 제거?
    */
    int level;
    [SerializeField] public EnemySkillPreset[] skillPresets;
    [SerializeField] public EnemyKind kind;
    public override bool isEnemy => true;
    private void OnEnable()
    {
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {

            spriteRenderer.material.color = Color.white;
            Debug.Log(spriteRenderer.color + spriteRenderer.gameObject.name);
        }
    }
    public override void InitActor(int atk, int def, int speed, int hp)
    {

        base.InitActor(atk, def, speed, hp);
    }
    public SkillKind GetRandomSkill()
    {

        int weightSum = 0;
        foreach (EnemySkillPreset value in skillPresets)
        {
            weightSum += value.SkillWeight;
        }
        int randomint = Utill.random.Next(1, weightSum + 1);

        for (int i = 0; i < skillPresets.Length; i++)
        {
            randomint -= skillPresets[i].SkillWeight;
            if (randomint <= 0)
            {
                return skillPresets[i].SkillKind;
            }
        }
        return skillPresets[1].SkillKind;
    }
}
[Serializable]
public class EnemySkillPreset
{
    public SkillKind SkillKind;
    public int SkillWeight;
}
public enum EnemyKind
{
    Drunk, Ghost, Cat, Camical
}
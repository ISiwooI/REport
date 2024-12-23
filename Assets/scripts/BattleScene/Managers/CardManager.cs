using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using System.Security.Cryptography;
using System.Linq;
using UnityEditor;
public class CardManager : MonoBehaviour
{
    List<SkillKind> normalDeck = new List<SkillKind>
    {
SkillKind.PHeal,
SkillKind.PHeal,
SkillKind.PHeal,
SkillKind.PSmite,
SkillKind.PSmite,
SkillKind.PSmite,
SkillKind.PSelfDevelopment,
SkillKind.PSelfDevelopment,
SkillKind.PSelfDevelopment,
SkillKind.PFirstAid,
SkillKind.PFirstAid,
SkillKind.PFirstAid,
SkillKind.PSelfDefense,
SkillKind.PSelfDefense,
SkillKind.PSelfDefense
    };
    List<SkillKind> unusedNormalSkill = new List<SkillKind>();
    List<SkillKind> departmenetDeck = new List<SkillKind>
    {
    SkillKind.PCautionChemicals,
    SkillKind.PClinicalTrials,
    SkillKind.PCrosscounter,
    SkillKind.PDivineBlessing,
    SkillKind.PExperimentFailed,
    SkillKind.PFlammableFlask,
    SkillKind.PGasCanister,
    SkillKind.PHealingLight,
    SkillKind.PHealtyBody,
    SkillKind.PLeech,
    SkillKind.PMercyProtect,
    SkillKind.PSalvationAndTrials,
    SkillKind.PTacticalTaunt,
    SkillKind.PTouchOfSalvation,
    SkillKind.PCautionChemicals,
    SkillKind.PClinicalTrials,
    SkillKind.PCrosscounter,
    SkillKind.PDivineBlessing,
    SkillKind.PExperimentFailed,
    SkillKind.PFlammableFlask,
    SkillKind.PGasCanister,
    SkillKind.PHealingLight,
    SkillKind.PHealtyBody,
    SkillKind.PLeech,
    SkillKind.PMercyProtect,
    SkillKind.PSalvationAndTrials,
    SkillKind.PTacticalTaunt,
    SkillKind.PTouchOfSalvation,
    SkillKind.PCautionChemicals,
    SkillKind.PClinicalTrials,
    SkillKind.PCrosscounter,
    SkillKind.PDivineBlessing,
    SkillKind.PExperimentFailed,
    SkillKind.PFlammableFlask,
    SkillKind.PGasCanister,
    SkillKind.PHealingLight,
    SkillKind.PHealtyBody,
    SkillKind.PLeech,
    SkillKind.PMercyProtect,
    SkillKind.PSalvationAndTrials,
    SkillKind.PTacticalTaunt,
    SkillKind.PTouchOfSalvation
    };
    List<SkillKind> unusedDepartmentSkill = new List<SkillKind>();


    List<CardObject> normalCards = new List<CardObject>(0);
    List<CardObject> departmentCards = new List<CardObject>();
    public int normalCardCount { get { return normalCards.Count; } }
    public int departmentCardCount { get { return departmentCards.Count; } }

    [SerializeField] CardObject CardPrefab;
    ObjectPool<CardObject> cardOP;
    [SerializeField] Transform dcl, ncl, dcr, ncr, dcSpawn, ncSpawn;
    public CardSpriteData cardSpriteData;
    public BattleManager battleManager;
    public SkillKind GetRandomSkill(bool isDepartment)
    {
        if (isDepartment)
        {
            if (unusedDepartmentSkill.Count == 0)
            {
                unusedDepartmentSkill.AddRange(departmenetDeck);
                Utill.Shuffle<SkillKind>(unusedDepartmentSkill);
            }
            SkillKind result = unusedDepartmentSkill.First();
            unusedDepartmentSkill.RemoveAt(0);
            return result;
        }
        else
        {
            if (unusedNormalSkill.Count == 0)
            {
                unusedNormalSkill.AddRange(normalDeck);
                Utill.Shuffle<SkillKind>(unusedNormalSkill);
            }
            SkillKind result = unusedNormalSkill.First();
            unusedNormalSkill.RemoveAt(0);
            return result;
        }
    }
    private void Awake()
    {
        unusedNormalSkill.Clear();

        cardOP = new ObjectPool<CardObject>(
            () =>
            {
                CardObject co = Instantiate(CardPrefab);
                co.cardManager = this;
                return co;
            }
            , (co) =>
            {
                co.gameObject.SetActive(true);
            }
            , (co) =>
            {
                co.gameObject.SetActive(false);
            }
            , (co) =>
            {
                Destroy(co.gameObject);
            }
            , true, 20, 40);
    }
    public void UpdateUsable()
    {
        foreach (var card in normalCards)
        {
            card.SetSelectable(battleManager.IsUsableCard(card));
        }
        foreach (var card in departmentCards)
        {
            card.SetSelectable(battleManager.IsUsableCard(card));
        }
    }
    public void DrawDC()
    {
        CardObject co = cardOP.Get();
        SoundManager.PlaySFX(soundKind.cardPick);
        co.transform.position = dcSpawn.transform.position;
        co.transform.rotation = dcSpawn.transform.rotation;
        co.InitCard(GetRandomSkill(true));
        departmentCards.Add(co);
        AlignCard();
    }
    public void DrawNC()
    {

        CardObject co = cardOP.Get();
        SoundManager.PlaySFX(soundKind.cardPick);
        co.transform.position = ncSpawn.transform.position;
        co.transform.rotation = ncSpawn.transform.rotation;
        co.InitCard(GetRandomSkill(false));
        normalCards.Add(co);
        AlignCard();
    }
    public void UseCard(CardObject cardObject, bool isDepartment)
    {
        if (battleManager.UseCard(cardObject))
        {

            if (isDepartment)
            {
                departmentCards.Remove(cardObject);
            }
            else
            {
                normalCards.Remove(cardObject);
            }
            //use Card Func
            cardOP.Release(cardObject);
        }
        AlignCard();
    }
    public void ClearDC()
    {
        foreach (CardObject cardObject in departmentCards)
        {
            cardOP.Release(cardObject);
        }
        departmentCards.Clear();
    }
    public void AlignCard()
    {
        int dcount = departmentCards.Count - 1;
        if (departmentCards.Count < 6) dcount = 4;
        int i = 0;
        foreach (CardObject co in departmentCards)
        {
            co.sortingGroup.sortingOrder = i;
            co.Orderindex = i;
            Vector3 pos = new Vector3(dcr.position.x + ((dcl.position.x - dcr.position.x) / dcount * i), dcr.position.y + ((dcl.position.y - dcr.position.y) * i), 0);
            co.transform.DOMove(pos, 0.5f, false).SetEase(Ease.OutSine);
            i++;
        }
        int ncount = normalCards.Count - 1;
        if (normalCards.Count < 6) ncount = 4;
        i = 0;
        foreach (CardObject co in normalCards)
        {
            co.sortingGroup.sortingOrder = i;
            co.Orderindex = i;
            Vector3 pos = new Vector3(ncr.position.x + ((ncl.position.x - ncr.position.x) / ncount * i), ncr.position.y + ((ncl.position.y - ncr.position.y) * i), 0);
            co.transform.DOMove(pos, 0.5f, false).SetEase(Ease.OutSine);
            i++;
        }
    }

}

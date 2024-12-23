using System.Collections;
using System.Collections.Generic;

public class SkillDelegater
{
    SortedList<SkillKind, Skill> skills;
    public SkillDelegater()
    {
        skills = new SortedList<SkillKind, Skill>
        {
            { SkillKind.EAlcoholic, new ESAlcoholic() },
            { SkillKind.ECharmingHeal, new ESCharmingHeal() },
            { SkillKind.ECharmingParalyse, new ESCharmingParalyse() },
            { SkillKind.EFadedReport, new ESFadedReport() },
            { SkillKind.ERanAmuck, new ESRanAmuck() },
            { SkillKind.ESelfDestructAttack, new ESSelfDestructAttack() },
            { SkillKind.ESelfDestructReady, new ESSelfDestructReady() },
            { SkillKind.EUnStableAttack, new ESUnStableAttack() },
            { SkillKind.PCautionChemicals, new PSCautionChemicals() },
            { SkillKind.PClinicalTrials, new PSClinicalTrials() },
            { SkillKind.PCrosscounter, new PSCrosscounter() },
            { SkillKind.PDivineBlessing, new PSDivineBlessing() },
            { SkillKind.PExperimentFailed, new PSExperimentFailed() },
            { SkillKind.PFirstAid, new PSFirstAid() },
            { SkillKind.PFlammableFlask, new PSFlammableFlask() },
            { SkillKind.PGasCanister, new PSGasCanister() },
            { SkillKind.PHeal, new PSHeal() },
            { SkillKind.PHealingLight, new PSHealingLight() },
            { SkillKind.PHealtyBody, new PSHealtyBody() },
            { SkillKind.PLeech, new PSLeech() },
            { SkillKind.PMercyProtect, new PSMercyProtect() },
            { SkillKind.PSalvationAndTrials, new PSSalvationAndTrials() },
            { SkillKind.PSelfDefense, new PSSelfDefense() },
            { SkillKind.PSelfDevelopment, new PSSelfDevelopment() },
            { SkillKind.PSmite, new PSSmite() },
            { SkillKind.PTacticalTaunt, new PSTacticalTaunt() },
            { SkillKind.PTouchOfSalvation, new PSTouchOfSalvation() }
        };
    }
    public Skill GetSkill(SkillKind sk)
    {
        if (sk == SkillKind.none) return null;
        return skills[sk];
    }
    public bool IsTargetable(SkillKind skillKind, BattleManager manager, BattleActor caster, BattleActor target = null)
    {
        return skills[skillKind].IsTargetable(manager, caster, target);
    }
    public IEnumerator DoSkill(SkillKind skillKind, BattleManager battleManager, BattleActor caster, BattleActor target = null)
    {
        return skills[skillKind].DoSkill(battleManager, caster, target);
    }
}
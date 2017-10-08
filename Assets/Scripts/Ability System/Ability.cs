using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability {

    private BasicObjectInformation objectInfo;
    private List<AbilityBehaviours> behaviours;
    private bool requiresTarget;
    private bool canCastOnSelf;
    private float cooldown;                             //Seconds.
    private GameObject abilityPrefab;                   //ability prefab
    private float castTime;                             //Seconds.
    private float cost;
    private AbilityType type;

    public enum AbilityType
    {
        Spell,
        Melee
    }

    public Ability(BasicObjectInformation aBasicInfo)
    {
        objectInfo = aBasicInfo;
        behaviours = new List<global::AbilityBehaviours>();
        cooldown = 1f;
        requiresTarget = false;
        canCastOnSelf = false;
    }

    public Ability(BasicObjectInformation aBasicInfo, List<AbilityBehaviours> abehaviours)
    {
        objectInfo = aBasicInfo;
        behaviours = new List<AbilityBehaviours>();
        behaviours = abehaviours;
        cooldown = 1f;
        requiresTarget = false;
        canCastOnSelf = false;
    }

    public Ability(BasicObjectInformation aBasicInfo, List<AbilityBehaviours> abehaviours, bool arequireTarget, float acooldown, GameObject abilityPb)
    {
        objectInfo = aBasicInfo;
        behaviours = new List<AbilityBehaviours>();
        behaviours = abehaviours;
        cooldown = acooldown;
        requiresTarget = arequireTarget;
        canCastOnSelf = false;
        abilityPrefab = abilityPb;
    }

    public BasicObjectInformation AbilityInfo
    {
        get { return objectInfo; }
    }

    public float AbilityCooldown
    {
        get { return cooldown; }
    }

    public List<AbilityBehaviours> AbilityBehaviours
    {
        get { return behaviours; }
    }

    public GameObject AbilityPrefab
    {
        set { abilityPrefab = value; }
    }

    //This is the method that will be called anytime we use an ability.
    public virtual void UseAbility(GameObject player)
    {
        foreach (AbilityBehaviours b in AbilityBehaviours)
        {
            if (b.AbilityBehaviourStartTime == global::AbilityBehaviours.BehaviourStartTimes.Beginning)
            {
                b.PerformBehaviour(player, abilityPrefab);
            }
        }
    }

}

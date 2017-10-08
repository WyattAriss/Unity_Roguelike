using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrostboltAbility : Ability
{

    private const string aName = "Frostbolt";
    private const string aDescription = "A frozen bolt that freezes on impact.";
    //private const Sprite icon = Resources.Load();                                     //send file path.

    private float aoeEffectDamage = 10f;
    private float aoeEffectRadius = 2f;
    private float aoeEffectDuration = 3f;
    private float dotDamage = 5f;
    private float dotDuration = 4f;
    private float dotDamageTickDuration = 2f;
    public int manaCost = 5;

    //ranged, at the start, max distance, requires target
    public FrostboltAbility()
        : base(new BasicObjectInformation(aName, aDescription))
    {
        this.AbilityBehaviours.Add(new Ranged(17f, 20f, true));
    }

}

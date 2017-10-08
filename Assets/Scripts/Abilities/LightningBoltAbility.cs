using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningBoltAbility : Ability
{

    private const string aName = "Lightning Bolt";
    private const string aDescription = "A charged bolt of lightning.";
    //private const Sprite icon = Resources.Load();                                     //send file path.

    private float aoeEffectDamage = 50f;
    private float aoeEffectRadius = 2f;
    private float aoeEffectDuration = 3f;
    private float dotDamage = 5f;
    private float dotDuration = 4f;
    private float dotDamageTickDuration = 2f;
    public int manaCost = 5;

    //ranged, at the start, max distance, requires target
    public LightningBoltAbility()
        : base(new BasicObjectInformation(aName, aDescription))
    {
        this.AbilityBehaviours.Add(new Ranged(17f, 20f, true));
    }

}

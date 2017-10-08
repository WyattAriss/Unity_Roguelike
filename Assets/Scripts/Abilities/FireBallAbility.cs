using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBallAbility : Ability {

    private const string aName = "Fire Ball";
    private const string aDescription = "A firey mass that explodes on impact.";
    //private const Sprite icon = Resources.Load();                                     //send file path.

    private float aoeEffectDamage = 50f;
    private float aoeEffectRadius = 2f;
    private float aoeEffectDuration = 3f;
    private float dotDamage = 5f;
    private float dotDuration = 4f;
    private float dotDamageTickDuration = 2f;
    public int manaCost = 5;

    //ranged, at the start, max distance, requires target
    public FireBallAbility()
        : base(new BasicObjectInformation(aName, aDescription))
    {
        this.AbilityBehaviours.Add(new Ranged(17f, 20f, true));
        this.AbilityBehaviours.Add(new AreaOfEffect(aoeEffectRadius, aoeEffectDuration, aoeEffectDamage));
        this.AbilityBehaviours.Add(new DamageOverTime(dotDuration, dotDamage, dotDamageTickDuration));
    }

}

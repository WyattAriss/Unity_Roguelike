using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class DamageOverTime : AbilityBehaviours {

    private const string abName = "Damage over time";
    private const string abDescription = "Deals damage to the target over time.";
    private const BehaviourStartTimes startTime = BehaviourStartTimes.Beginning;        //
    //private const Sprite icon = Resources.Load();                                     //send file path.

    private float effectDuration;                                                       //How long the effect lasts.
    private Stopwatch durationTimer = new Stopwatch();
    private float baseEffectDamage;                                                     //Damage oer tick
    private float damageTickDuration;                                                   //time between ticks



    public DamageOverTime(float ed, float bd, float dtd) 
        : base(new BasicObjectInformation(abName, abDescription /*,icon*/), startTime)
    {
        effectDuration = ed;
        baseEffectDamage = bd;
        damageTickDuration = dtd;
    }

    public override void PerformBehaviour(GameObject playerObject, GameObject objectHit)
    {

        //StartCoroutine(DOT());
    }

    private IEnumerator DOT()
    {
        durationTimer.Start();                                                          //Turns on timer.

        while (durationTimer.Elapsed.TotalSeconds <= effectDuration)
        {
            //onDamage(list<targets>, baseDamage);
            yield return new WaitForSeconds(damageTickDuration);

        }

        durationTimer.Stop();
        durationTimer.Reset();

        yield return null;
    }

}

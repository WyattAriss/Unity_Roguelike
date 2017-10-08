using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Slow : AbilityBehaviours {

    private const string abName = "Slow";
    private const string abDescription = "Slows objectts movement speed.";
    private const BehaviourStartTimes startTime = BehaviourStartTimes.End;              //On impact.
    //private const Sprite icon = Resources.Load();                                     //send file path.

    private float effectDuration;                                                       //How long the effect lasts.
    private float slowPercent;

    public Slow(float ed, float sp) 
        : base(new BasicObjectInformation(abName, abDescription /*,icon*/), startTime)
    {
        effectDuration = ed;
        slowPercent = sp;
    }

    public override void PerformBehaviour(GameObject playerObject, GameObject objectHit)
    {
       
        //StartCoroutine(SlowMovement(objectHit));
    }

    private IEnumerator SlowMovement(GameObject objectHit)
    {

        //if(objectHit.GetComponent<"Movement">() != null)
        //get movement variable
        //apply percentage slow to it

        yield return new WaitForSeconds(effectDuration);

        //reset object's movement speed

        yield return null;
    }
}

using UnityEngine;
using System.Collections;
using System.Diagnostics;                                                               //Access to stopwatch class.

[RequireComponent(typeof(BoxCollider2D))]
public class AreaOfEffect : AbilityBehaviours {

    private const string abName = "Area of Effect";
    private const string abDescription = "Creates an area of damage.";
    private const BehaviourStartTimes startTime = BehaviourStartTimes.End;              //On impact.
    //private const Sprite icon = Resources.Load();                                     //send file path.

    private float areaRadius;                                                           //Radius of sphere collider.
    private float effectDuration;                                                       //How long the effect lasts.
    private Stopwatch durationTimer = new Stopwatch();
    private float baseEffectDamage;
    private bool isOccupied;
    private float damageTickDuration;

    public AreaOfEffect(float ar, float ed, float bd) 
        : base(new BasicObjectInformation(abName, abDescription /*,icon*/), startTime)
    {
        areaRadius = ar;
        effectDuration = ed;
        baseEffectDamage = bd;
        isOccupied = false;

    }

    public override void PerformBehaviour(GameObject playerObject, GameObject objectHit)
    {
        //SphereCollider sc = this.gameObject.GetComponent<SphereCollider>();

        //This code doesnt need requires component at top
        /*
        if (this.gameObject.GetComponent<SphereCollider>() == null)
            sc = this.gameObject.GetComponent<SphereCollider>();
        else
            sc = this.gameObject.GetComponent<SphereCollider>();
        */

        //sc.radius = areaRadius;
        //sc.isTrigger = true;

        //StartCoroutine(AOE());

        Job.make(AOE(), true);
    }

    private IEnumerator AOE()
    {
        durationTimer.Start();                                                          //turns on timer.

        while(durationTimer.Elapsed.TotalSeconds <= effectDuration)
        {
            if(isOccupied)
            {
                UnityEngine.Debug.Log("COROUTINE AOE");
                //onDamage(list<targets>, baseDamage);
            }

            yield return new WaitForSeconds(damageTickDuration);

        }

        durationTimer.Stop();
        durationTimer.Reset();

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isOccupied)
        {
            UnityEngine.Debug.Log("ENTER TRIGGER2");
            //do damage here again
        }
        else
            isOccupied = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isOccupied = false;
    }

}

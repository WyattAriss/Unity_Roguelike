using UnityEngine;
using System.Collections;

public class Ranged : AbilityBehaviours {

    private const string abName = "Ranged";
    private const string abDescription = "A ranged attack";
    private const BehaviourStartTimes startTime = BehaviourStartTimes.Beginning;
    //private const Sprite icon = Resources.Load();                                     //send file path

    private float minDistance;
    private float maxDistance;
    private bool isRandomOn;
    private float lifeDistance;

    public Ranged()
        : base(new BasicObjectInformation(abName, abDescription /*,icon*/), startTime)
    {
        minDistance = 50;
        maxDistance = 80;
        isRandomOn = true;
    }

    public Ranged(float minDist, float maxDist, bool isRandom) 
        : base(new BasicObjectInformation(abName, abDescription /*,icon*/), startTime)
    {
        minDistance = minDist;
        maxDistance = maxDist;
        isRandomOn = isRandom;
    }

    //Using a job manager to handle coroutines
    public override void PerformBehaviour(GameObject playerObject, GameObject abilityPrefab)
    {
        lifeDistance = isRandomOn ? Random.Range(minDistance, maxDistance) : maxDistance;
        //Debug.Log("Distance: " + lifeDistance);
        Job.make(CheckDistance(playerObject.transform.position, abilityPrefab), true);
        //StartCoroutine(CheckDistance(playerObject.transform.position));
    }

    private IEnumerator CheckDistance(Vector3 startPosition, GameObject abilityPrefab)
    {
        float tempdistance = Vector3.Distance(startPosition, abilityPrefab.transform.position);
        while (tempdistance < lifeDistance)
        {
            if (abilityPrefab == null) break;
            tempdistance = Vector3.Distance(startPosition, abilityPrefab.transform.position);

            yield return null;
        }
        //abilityPrefab.gameObject.SetActive(false);                                               //Could destroy object here. Or object pooling code (put objects as child under game object).
        GameObject.Destroy(abilityPrefab);
        yield return null;
    }

    public float MinDistance
    {
        get { return minDistance; }
    }

    public float MaxDistance
    {
        get { return maxDistance; }
    }

}

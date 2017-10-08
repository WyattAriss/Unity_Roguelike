using UnityEngine;
using System.Collections;

public class AbilityBehaviours {

    private BasicObjectInformation objectInfo;
    private BehaviourStartTimes startTime;

    public AbilityBehaviours(BasicObjectInformation basicInfo, BehaviourStartTimes sTime)
    {
        objectInfo = basicInfo;
        startTime = sTime;
    }

    public enum BehaviourStartTimes
    {
        Beginning,
        Middle,
        End
    }

    //we want a gameobject, which is our target,
    public virtual void PerformBehaviour(GameObject playerObject, GameObject objectHit)
    {
        Debug.LogWarning("NEED TO ADD BEHAVIOUR");
    }

    public BasicObjectInformation AbilityBehaviourInfo
    {
        get { return objectInfo; }
    }

    public BehaviourStartTimes AbilityBehaviourStartTime
    {
        get { return startTime; }
    }

}

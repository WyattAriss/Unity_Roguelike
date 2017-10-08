using UnityEngine;
using System.Collections;

public class DoNotDestroy2 : MonoBehaviour
{

    public static DoNotDestroy2 cInstance = null;               //Static instance of Inventory which allows it to be accessed by any other script.

    // Use this for initialization
    void Start()
    {


        //Check if cInstance already exists
        if (cInstance == null)

            //if not, set cInstance to this
            cInstance = this;

        //If iInstance already exists and it's not this:
        else if (cInstance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a PlayerInventory.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {


    }
}


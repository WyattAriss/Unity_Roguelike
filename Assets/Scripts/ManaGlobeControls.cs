using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Completed;

public class ManaGlobeControls : MonoBehaviour
{

    private float globeValue;
    Player playerCharacterRef;

    void Start()
    {
        GameObject playerCharacterGameObject = GameObject.Find("Player");
        playerCharacterRef = playerCharacterGameObject.GetComponent<Player>();
        globeValue = this.GetComponent<Slider>().value;
    }

    // Update is called once per frame
    void Update()
    {

        this.GetComponent<Slider>().value = (float)playerCharacterRef.mana / (float)playerCharacterRef.playerMaxStamina;

    }
}

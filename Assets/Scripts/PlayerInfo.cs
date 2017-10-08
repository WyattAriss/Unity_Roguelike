using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

    public string charName;
    public int charClass;                                              //1 = Warrior, 2 = Rogue, 3 = Sorcerer
    public int charStrength;
    public int charDexterity;
    public int charIntelligence;
    public int charVitality;
    public int charWillpower;



    MainMenu mainMenuRef;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void SetAttributes()
    {
        GameObject mainMenuObjectRef = GameObject.FindGameObjectWithTag("Main Menu Camera");
        mainMenuRef = mainMenuObjectRef.GetComponent<MainMenu>();

        if (!mainMenuRef.characterGenDone)
        {
            charName = mainMenuRef.tempCharName;
            charClass = mainMenuRef.tempCharClass;
            charStrength = mainMenuRef.tempStrength;
            charDexterity = mainMenuRef.tempDexterity;
            charIntelligence = mainMenuRef.tempIntelligence;
            charVitality = mainMenuRef.tempVitality;
            charWillpower = mainMenuRef.tempWillpower;
            charWillpower = mainMenuRef.tempWillpower;

        }
    }
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
            SetAttributes();
	}
}

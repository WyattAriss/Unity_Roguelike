using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{


    public GUISkin mySkin;
    private Rect standardMenuWindow = new Rect((Screen.width / 20), (Screen.height / 25), (Screen.width / 4) + Screen.width / 30, (Screen.height / 2) + Screen.height / 3);
    private Rect windowRect0Main = new Rect();
    private Rect windowRect1NewGame = new Rect();
    private Rect windowRect2NewChar = new Rect();
    private Rect windowRect3Stats = new Rect();
    private Rect toolTipRect= new Rect();
    private bool showWindowMain = true;
    private bool showWindowNewGame = false;
    private bool showWindowNewChar = false;
    private bool showWindowStats = false;

    public bool characterGenDone = true;

    private float mouseX;
    private float mouseY;

    //windowRect0Main = standardMenuWindow; //Why doesnt this work?

    private string lastToolTip;

    private string currentToolTip = "";

    private string tempClassHolder;

    #region Player Information

    public string tempCharName = "Name";
    public int tempCharClass;                                          //1 = Warrior, 2 = Rogue, 3 = Sorcerer
    public int charAttributePoints = 8;
    public int tempStrength = 10;
    public int tempDexterity = 10;
    public int tempIntelligence = 10;
    public int tempVitality = 10;
    public int tempWillpower = 10;                                     //Make willpower have an impact on the extra random amount of hit points gained on a level up.

    #endregion

    void OnGUI()
    {

        mouseX = Event.current.mousePosition.x;
        mouseY = Event.current.mousePosition.y;

        GUI.skin = mySkin;
        if (showWindowMain) windowRect0Main = GUI.Window(0, windowRect0Main, MainMenuWindow, "");
        if (showWindowNewGame) windowRect1NewGame = GUI.Window(1, windowRect1NewGame, NewGameWindow, "");
        if (showWindowNewChar) windowRect2NewChar = GUI.Window(2, windowRect2NewChar, NewCharWindow, "");
        if (showWindowStats) windowRect3Stats = GUI.Window(3, windowRect3Stats, StatsWindow, "");

        //if (currentToolTip != "") Debug.Log("WORK!");

    }

    void BackButton()
    {
        if (GUILayout.Button("Back"))
        {
            showWindowNewGame = false;
            showWindowNewChar = false;
            showWindowStats = false;
            showWindowMain = true;

        }
    }

    void MainMenuWindow(int windowID)
    {

        windowRect0Main = standardMenuWindow;

        GUILayout.BeginVertical();
        GUILayout.Space(40);

        if (GUILayout.Button("New Game"))
        {
            showWindowMain = false;
            showWindowNewChar = false;
            showWindowStats = false;
            showWindowNewGame = true;
            
        }
        else if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }

        GUILayout.EndVertical();

        //GUI.DragWindow(new Rect(0, 0, 10000, 10000)); //Comment out later.
    }

    void NewGameWindow(int windowID)
    {

        windowRect1NewGame = standardMenuWindow;

        GUILayout.BeginVertical();
        GUILayout.Space(40);

        BackButton();

        if (GUILayout.Button("Generate New Character"))
        {
            showWindowNewGame = false;
            showWindowMain = false;
            showWindowStats = false;
            showWindowNewChar = true;
        }

        GUILayout.EndVertical();

        //GUI.DragWindow(new Rect(0, 0, 10000, 10000)); //Comment out later.

    }

    void NewCharWindow (int windowID)
    {

        windowRect2NewChar = standardMenuWindow;

        GUILayout.BeginVertical();
        GUILayout.Space(40);

        BackButton();

        GUILayout.Label("Choose Your Class");

        if (GUILayout.Button(new GUIContent("Warrior", "The warrior is a melee fighter who uses physical strength and superior stamina to overcome his foes. Strength and vitality are his primary attributes.")))
        {
            tempCharClass = 1;
            tempClassHolder = "Warrior";
            showWindowNewGame = false;
            showWindowMain = false;
            showWindowNewChar = false;
            showWindowStats = true;
        }

        else if (GUILayout.Button(new GUIContent("Rogue", "The rogue is proficient at both melee and ranged combat but uses dexterity and cunning to defeat his enemies. Dexterity and Willpower are his primary attributes.")))
        {
            tempCharClass = 2;
            tempClassHolder = "Rogue";
            showWindowNewGame = false;
            showWindowMain = false;
            showWindowNewChar = false;
            showWindowStats = true;
        }

        else if (GUILayout.Button(new GUIContent("Sorcerer", "The sorcerer is a master of magic and mana who uses intellect and power to overwhelm his foes. Intelligence is the primary attribute of the sorcerer.")))
        {
            tempCharClass = 3;
            tempClassHolder = "Sorcerer";
            showWindowNewGame = false;
            showWindowMain = false;
            showWindowNewChar = false;
            showWindowStats = true;
        }

        GUILayout.EndVertical();

        toolTipRect = new Rect(mouseX-(Screen.width / 20), mouseY, (Screen.width / 12), (Screen.height / 4));

        if (Event.current.type == EventType.Repaint)                                    //This part made everything work; I dont really know why
            currentToolTip = GUI.tooltip;

        if (currentToolTip != "" && currentToolTip != lastToolTip)
        {
            GUI.Label(toolTipRect, currentToolTip, "tooltip");

            //currentToolTip = lastToolTip; //Doesnt appear to be necessary.
        }


        //GUI.DragWindow(new Rect(0, 0, 10000, 10000)); //Comment out later.
    }

    int PlusMinusButtons (int tempAttribute)
    {
        if (GUILayout.Button(new GUIContent("-", "An attribute cannot be reduced below 10")))
        {
            if (charAttributePoints < 8 && tempAttribute > 10 && tempAttribute <= 13)
            {
                tempAttribute -= 1;
                charAttributePoints += 1;
                return tempAttribute;
            }
            else if (charAttributePoints < 8 && tempAttribute > 10 && tempAttribute <= 16)
            {
                tempAttribute -= 1;
                charAttributePoints += 2;
                return tempAttribute;
            }
            else if (charAttributePoints < 8 && tempAttribute > 10 && tempAttribute >= 17)
            {
                tempAttribute -= 1;
                charAttributePoints += 3;
                return tempAttribute;
            }

        }

        if (GUILayout.Button(new GUIContent("+", "Increasing an attribute by 1 between 13-16 takes 2 points, and between 17-20 takes 3 points")))
        {
            if (charAttributePoints > 0)
            {
                if (tempAttribute < 13)
                {
                    tempAttribute += 1;
                    charAttributePoints -= 1;
                    return tempAttribute;
                }
                else if (tempAttribute >= 13 && tempAttribute <= 16 && charAttributePoints >= 2)
                {
                    tempAttribute += 1;
                    charAttributePoints -= 2;
                    return tempAttribute;
                }
                else if (tempAttribute >= 17 && charAttributePoints >= 3)
                {
                    tempAttribute += 1;
                    charAttributePoints -= 3;
                    return tempAttribute;
                }
            }
        }
        return tempAttribute;
    }                                        //Is using a hard coded value for number of starting attribute points

    void StatsWindow (int windowID)
    {

        windowRect3Stats = standardMenuWindow;

        GUILayout.BeginVertical();
        GUILayout.Space(40);

        BackButton();

        GUILayout.Label(tempClassHolder);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Attribute Points:", GUILayout.Width(225));
        GUILayout.TextArea(charAttributePoints.ToString(), GUILayout.Width(30));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button(new GUIContent("Strength", "Strength influences your characters damage, and is the prime attribute of the Warrior"), GUILayout.Width(125));
        GUILayout.TextArea(tempStrength.ToString(), GUILayout.Width(30));
        tempStrength = PlusMinusButtons(tempStrength);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button(new GUIContent("Dexterity", "Dexterity dictates your characters skill with ranged weapons, and is the prime attribute of the Rogue"), GUILayout.Width(125));
        GUILayout.TextArea(tempDexterity.ToString(), GUILayout.Width(30));
        tempDexterity = PlusMinusButtons(tempDexterity);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button(new GUIContent("Intelligence", "Intelligence dictates your characters mana points, which is used for spells, and is the prime attribute of the Sorcerer"), GUILayout.Width(125));
        GUILayout.TextArea(tempIntelligence.ToString(), GUILayout.Width(30));
        tempIntelligence = PlusMinusButtons(tempIntelligence);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button(new GUIContent("Vitality", "Vitality influences your characters hit points"), GUILayout.Width(125));
        GUILayout.TextArea(tempVitality.ToString(), GUILayout.Width(30));
        tempVitality = PlusMinusButtons(tempVitality);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button(new GUIContent("Willpower", "Willpower influences your characters stamina, which is used for combat skills, and has a small impact on hit points gained per level"), GUILayout.Width(125));
        GUILayout.TextArea(tempWillpower.ToString(), GUILayout.Width(30));
        tempWillpower = PlusMinusButtons(tempWillpower);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:");
        tempCharName = GUILayout.TextField(tempCharName, 25);
        GUILayout.EndHorizontal();

        GUILayout.Space(40);
        if (GUILayout.Button("Continue?") && charAttributePoints == 0 && tempCharName != null && tempCharName != "Name")
        {
            characterGenDone = false;

            Application.LoadLevel("Main");
        }
        GUILayout.EndVertical();

        toolTipRect = new Rect(mouseX - (Screen.width / 20), mouseY, (Screen.width / 12), (Screen.height / 4));

        if (Event.current.type == EventType.Repaint)                                    //This part made everything work; I dont really know why
            currentToolTip = GUI.tooltip;

        if (currentToolTip != "" && currentToolTip != lastToolTip)
        {
            GUI.Label(toolTipRect, currentToolTip, "tooltip");

            //currentToolTip = lastToolTip; //Doesnt appear to be necessary.
        }

        //GUI.DragWindow(new Rect(0, 0, 10000, 10000)); //Comment out later.

    }


}
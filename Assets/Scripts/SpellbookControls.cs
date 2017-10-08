using UnityEngine;
using System.Collections;

public class SpellbookControls : MonoBehaviour {

    private bool isActive;
    GameObject SpellbookPanel;
    public AudioClip pageTurn1;
    public AudioClip closeBook1;

	// Use this for initialization
	void Start () {

        SpellbookPanel = GameObject.Find("Panel - Spellbook");
        SpellbookPanel.gameObject.SetActive(false);
        isActive = false;

	}
	
    public void OpenCloseSpellbook ()
    {
        if (isActive == false)
        {
            SpellbookPanel.gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(pageTurn1, Camera.main.transform.position, .03f);
            isActive = true;
        }
        else
        {
            SpellbookPanel.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(closeBook1, Camera.main.transform.position, .3f);
            isActive = false;
        }
    }

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("b"))
        {
            OpenCloseSpellbook();
        }
	
	}
}

using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using Completed;

public class AbilityUse : MonoBehaviour {

    /// I think that calling the same spin coroutine while its still running is causing the problem.
    /// need to set each spell to make all other spells non-interactable while waiting for key input.

    //this script is for use of any spell that uses the ranged class
    //Attach script to player

    Player playerRef;

    #region Spell Check

    private bool isFireball = false;
    private bool isFrostbolt = false;
    private bool isLightningBolt = false;

    public void SetIsFireball()
    {
        isFireball = true;
    }
    public void SetIsFrostbolt()
    {
        isFrostbolt = true;
    }
    public void SetIsLightningBolt()
    {
        isLightningBolt = true;
    }
    #endregion


    public GameObject fireballPrefab;
    private FireBallAbility fba;
    public GameObject frostboltPrefab;
    private FrostboltAbility frostba;
    public GameObject lightningBoltPrefab;
    private LightningBoltAbility lba;
    private Stopwatch baseCooldownTimer;
    private Stopwatch abilityCooldownTimer;                                                 //Going to have to change the cooldown system to turns.
    private Stopwatch abilityCooldownTimer2;                                                 //Going to have to change the cooldown system to turns.
    private Stopwatch abilityCooldownTimer3;                                                 //Going to have to change the cooldown system to turns.
    private Stopwatch chanceToCastTimer;                                                    //Cancels out of the spell cast if the player waits too long
    private Button button;
    private Image fillImage;
    private Image fillImage2;
    private Rigidbody2D rb;
    private GameObject spellbookRef;


    Button[] allSpellButtons;

    public bool isCasting = false;

    private bool _keyPressed = false;
    
    private int vDirection;                                                                  
    private int hDirection;                                                                  


    //Waits until a directional key is pressed so that we can determine a direction for the spell to travel.
    public IEnumerator WaitForKeyPress(GameObject btn)                                                     //Going to have to make sure to turn player turn to false until a button is pressed to make sure he doesnt move.
    {
        isCasting = true;
        spellbookRef = GameObject.FindGameObjectWithTag("Spellbook");
        allSpellButtons = spellbookRef.GetComponentsInChildren<Button>();
        foreach (Button element in allSpellButtons)
        {
            element.interactable = false;
        }
        chanceToCastTimer = new Stopwatch();
        chanceToCastTimer.Start();
        while (!_keyPressed)
        {
            vDirection = (int)Input.GetAxisRaw("Vertical");
            hDirection = (int)Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Vertical"))
            {
                chanceToCastTimer.Stop();
                chanceToCastTimer.Reset();
                StartAbility(btn);
                break;
            }
            else if (Input.GetButtonDown("Horizontal"))
            {
                chanceToCastTimer.Stop();
                chanceToCastTimer.Reset();
                StartAbility(btn);
                break;
            }
            else if (Input.GetButtonDown("Cancel") || chanceToCastTimer.Elapsed.TotalSeconds > 20f)
            {
                chanceToCastTimer.Stop();
                chanceToCastTimer.Reset();
                isCasting = false;
                fillImage = btn.transform.GetChild(0).gameObject.GetComponent<Image>();
                button = btn.GetComponent<Button>();
                button.interactable = true;
                foreach (Button element in allSpellButtons)
                {
                    element.interactable = true;
                }
                break;
            }
            //print("Awaiting key input.");
            yield return 0;
        }
    }

    private void StartAbility(GameObject btn)
    {
        baseCooldownTimer = new Stopwatch();
        baseCooldownTimer.Start();
        StartCoroutine(BaseCD(btn));
        this.GetComponent<BoxCollider2D>().enabled = false;                               //Turn off trigger or boxcollider on player so that the fireball doesnt hit him
        _keyPressed = true;
        //print("Starting the game officially!");
        if (isFireball == true)
        {
            OnAbilityUse2(btn);
        }
        else if (isFrostbolt == true)
        {
            FrostboltUse(btn);
        }
        else if (isLightningBolt == true)
        {
            LightningBoltUse(btn);
        }
        _keyPressed = false;
        vDirection = 0;
        hDirection = 0;
    }

    public void OnAbilityUse(GameObject btn)
    {
        if (!isCasting)
        {
            isCasting = true;
            spellbookRef = GameObject.FindGameObjectWithTag("Spellbook");
            allSpellButtons = spellbookRef.GetComponentsInChildren<Button>();
            foreach (Button element in allSpellButtons)
            {
                element.interactable = false;
            }
            //if ability is not on cool down use it
            fillImage = btn.transform.GetChild(0).gameObject.GetComponent<Image>();
            //UnityEngine.Debug.Log(btn.transform.GetChild(0).gameObject.name);
            button = btn.GetComponent<Button>();
            button.interactable = false;

            StartCoroutine(WaitForKeyPress(btn));

            /*
            fillImage.fillAmount = 1;
            abilityCooldownTimer = new Stopwatch();
            abilityCooldownTimer.Start();



            GameObject go = Instantiate<GameObject>(fireballPrefab);
            go.transform.position = this.transform.position;
            fba = new FireBallAbility();
            fba.AbilityPrefab = go;
            fba.UseAbility(this.gameObject);

            StartCoroutine(SpinImage()); 
            */
        }

    }

    public void OnAbilityUse2(GameObject btn)
    {
        fba = new FireBallAbility();

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().mana < fba.manaCost)
        {
            btn.GetComponent<Button>().interactable = true;
        }
        else
        {
            isFireball = false;
            btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
            abilityCooldownTimer = new Stopwatch();
            abilityCooldownTimer.Start();


            GameObject go = Instantiate<GameObject>(fireballPrefab);
            go.transform.position = this.transform.position;
            if (hDirection != 0)
            {
                go.GetComponent<Animator>().SetInteger("Horizontal", hDirection);
            }
            else if (vDirection != 0)
            {
                go.GetComponent<Animator>().SetInteger("Vertical", vDirection);
            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().mana -= fba.manaCost;
            fba.AbilityPrefab = go;
            fba.UseAbility(this.gameObject);
            rb = go.GetComponent<Rigidbody2D>();
            if (vDirection > 0)
            {
                rb.AddForce(Vector2.up * 100);
            }
            else if (vDirection < 0)
            {
                rb.AddForce(Vector2.down * 100);
            }
            else if (hDirection > 0)
            {
                rb.AddForce(Vector2.right * 100);
            }
            else if (hDirection < 0)
            {
                rb.AddForce(Vector2.left * 100);
            }

            //Turn off trigger or boxcollider on player so that the fireball doesnt hit him

            StartCoroutine(SpinImage(fba.AbilityCooldown, btn));
        }

    }

    public void FrostboltUse(GameObject btn)
    {
        frostba = new FrostboltAbility();

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().mana < frostba.manaCost)
        {
            btn.GetComponent<Button>().interactable = true;
        }
        else
        {
            isFrostbolt = false;
            btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
            abilityCooldownTimer2 = new Stopwatch();
            abilityCooldownTimer2.Start();


            GameObject go = Instantiate<GameObject>(frostboltPrefab);
            go.transform.position = this.transform.position;
            if (hDirection != 0)
            {
                go.GetComponent<Animator>().SetInteger("Horizontal", hDirection);
            }
            else if (vDirection != 0)
            {
                go.GetComponent<Animator>().SetInteger("Vertical", vDirection);
            }
            GameObject playerInfoGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRef = playerInfoGameObject.GetComponent<Player>();
            playerRef.mana -= frostba.manaCost;
            frostba.AbilityPrefab = go;
            frostba.UseAbility(this.gameObject);
            rb = go.GetComponent<Rigidbody2D>();
            if (vDirection > 0)
            {
                rb.AddForce(Vector2.up * 100);
            }
            else if (vDirection < 0)
            {
                rb.AddForce(Vector2.down * 100);
            }
            else if (hDirection > 0)
            {
                rb.AddForce(Vector2.right * 100);
            }
            else if (hDirection < 0)
            {
                rb.AddForce(Vector2.left * 100);
            }

            //Turn off trigger or boxcollider on player so that the fireball doesnt hit him

            StartCoroutine(SpinImage2(frostba.AbilityCooldown, btn));
        }

    }

    public void LightningBoltUse(GameObject btn)
    {
        lba = new LightningBoltAbility();

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().mana < frostba.manaCost)
        {
            btn.GetComponent<Button>().interactable = true;
        }
        else
        {
            isLightningBolt = false;
            btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 1;
            abilityCooldownTimer3 = new Stopwatch();
            abilityCooldownTimer3.Start();

            GameObject go = Instantiate<GameObject>(lightningBoltPrefab);
            go.transform.position = this.transform.position;
            if (hDirection != 0)
            {
                go.GetComponent<Animator>().SetInteger("Horizontal", hDirection);
            }
            else if (vDirection != 0)
            {
                go.GetComponent<Animator>().SetInteger("Vertical", vDirection);
            }
            GameObject playerInfoGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRef = playerInfoGameObject.GetComponent<Player>();
            playerRef.mana -= lba.manaCost;
            lba.AbilityPrefab = go;
            lba.UseAbility(this.gameObject);
            rb = go.GetComponent<Rigidbody2D>();
            if (vDirection > 0)
            {
                rb.AddForce(Vector2.up * 100);
            }
            else if (vDirection < 0)
            {
                rb.AddForce(Vector2.down * 100);
            }
            else if (hDirection > 0)
            {
                rb.AddForce(Vector2.right * 100);
            }
            else if (hDirection < 0)
            {
                rb.AddForce(Vector2.left * 100);
            }

            //Turn off trigger or boxcollider on player so that the fireball doesnt hit him

            StartCoroutine(SpinImage3(lba.AbilityCooldown, btn));
        }

    }

    private IEnumerator BaseCD(GameObject btn)
    {
        while (baseCooldownTimer.IsRunning && baseCooldownTimer.Elapsed.TotalSeconds < 1f)
        {
            foreach (Button element in allSpellButtons)
            {
                if (element != btn)
                {
                    element.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = ((float)baseCooldownTimer.Elapsed.TotalSeconds / 1f);
                    yield return null;
                }
            }
        }
        baseCooldownTimer.Stop();
        baseCooldownTimer.Reset();
        foreach (Button element in allSpellButtons)
        {
            if (element != btn && element.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount > 0.95f)
            {
                element.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                element.interactable = true;
            }
        }
    }


    private IEnumerator SpinImage(float cooldown, GameObject btn)
    {
        //UnityEngine.Debug.Log(fba.AbilityCooldown);


        while (abilityCooldownTimer.IsRunning && abilityCooldownTimer.Elapsed.TotalSeconds < cooldown)
        {
            if (abilityCooldownTimer.Elapsed.TotalSeconds > 0.2f && abilityCooldownTimer.Elapsed.TotalSeconds < 1f)
            {
                this.GetComponent<BoxCollider2D>().enabled = true;
                isCasting = false;
            }
            //UnityEngine.Debug.Log(fillImage.fillAmount);
            btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = ((float)abilityCooldownTimer.Elapsed.TotalSeconds / cooldown);
            yield return null;
        }
        btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
        button.interactable = true;
        abilityCooldownTimer.Stop();
        abilityCooldownTimer.Reset();

        yield return null;
    }

    /// Additional spin image coroutines are required for each spells cooldown to not overlap the other
    /// This fix is not ideal. Hopefully there is a better way.
    /// Each time a new spell is created another coroutine is required.
    #region Additional Spin Image Coroutines

    private IEnumerator SpinImage2(float cooldown, GameObject btn)
    {
        //UnityEngine.Debug.Log(fba.AbilityCooldown);


        while (abilityCooldownTimer2.IsRunning && abilityCooldownTimer2.Elapsed.TotalSeconds < cooldown)
        {
            if (abilityCooldownTimer2.Elapsed.TotalSeconds > 0.2f && abilityCooldownTimer2.Elapsed.TotalSeconds < 1f)
            {
                this.GetComponent<BoxCollider2D>().enabled = true;
                isCasting = false;
            }
            //UnityEngine.Debug.Log(fillImage.fillAmount);
            btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = ((float)abilityCooldownTimer2.Elapsed.TotalSeconds / cooldown);
            yield return null;
        }
        btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
        button.interactable = true;
        abilityCooldownTimer2.Stop();
        abilityCooldownTimer2.Reset();

        yield return null;
    }

    private IEnumerator SpinImage3(float cooldown, GameObject btn)
    {
        //UnityEngine.Debug.Log(fba.AbilityCooldown);


        while (abilityCooldownTimer3.IsRunning && abilityCooldownTimer3.Elapsed.TotalSeconds < cooldown)
        {
            if (abilityCooldownTimer3.Elapsed.TotalSeconds > 0.2f && abilityCooldownTimer3.Elapsed.TotalSeconds < 1f)
            {
                this.GetComponent<BoxCollider2D>().enabled = true;
                isCasting = false;
            }
            //UnityEngine.Debug.Log(fillImage.fillAmount);
            btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = ((float)abilityCooldownTimer3.Elapsed.TotalSeconds / cooldown);
            yield return null;
        }
        btn.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
        button.interactable = true;
        abilityCooldownTimer3.Stop();
        abilityCooldownTimer3.Reset();

        yield return null;
    }
    #endregion

}

using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;

public class FrostboltUse : MonoBehaviour
{

    //Attach script to player

    public GameObject frostboltPrefab;
    private FrostboltAbility fba;
    private Stopwatch abilityCooldownTimer;                                                  //Going to have to change the cooldown system to turns.
    private Button button;
    private Image fillImage;
    private Rigidbody2D rb;

    public bool isCasting = false;

    private bool _keyPressed = false;

    private int vDirection;                                                                  //Direction for the fireball to travel. #myown
    private int hDirection;                                                                  //Direction for the fireball to travel. #myown

    //Waits until a directional key is pressed so that we can determine a direction for the spell to travel.
    public IEnumerator WaitForKeyPress()                                                     //Going to have to make sure to turn player turn to false until a button is pressed to make sure he doesnt move.
    {
        isCasting = true;
        while (!_keyPressed)
        {
            vDirection = (int)Input.GetAxisRaw("Vertical");
            hDirection = (int)Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Vertical"))
            {
                StartAbility();
                break;
            }
            else if (Input.GetButtonDown("Horizontal"))
            {
                StartAbility();
                break;
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                isCasting = false;
                break;
            }
            //print("Awaiting key input.");
            yield return 0;
        }
    }

    private void StartAbility()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;                               //Turn off trigger or boxcollider on player so that the fireball doesnt hit him
        _keyPressed = true;
        //print("Starting the game officially!");
        OnAbilityUse2();
        _keyPressed = false;
        vDirection = 0;
        hDirection = 0;
    }

    public void OnAbilityUse(GameObject btn)
    {
        //if ability is not on cool down use it
        fillImage = btn.transform.GetChild(0).gameObject.GetComponent<Image>();
        UnityEngine.Debug.Log(btn.transform.GetChild(0).gameObject.name);
        button = btn.GetComponent<Button>();
        button.interactable = false;

        StartCoroutine(WaitForKeyPress());

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

    public void OnAbilityUse2()
    {
        fillImage.fillAmount = 1;
        abilityCooldownTimer = new Stopwatch();
        abilityCooldownTimer.Start();



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
        fba = new FrostboltAbility();
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

        StartCoroutine(SpinImage());
    }

    private IEnumerator SpinImage()
    {
        //UnityEngine.Debug.Log(fba.AbilityCooldown);


        while (abilityCooldownTimer.IsRunning && abilityCooldownTimer.Elapsed.TotalSeconds < fba.AbilityCooldown)
        {
            if (abilityCooldownTimer.Elapsed.TotalSeconds > 0.2 && abilityCooldownTimer.Elapsed.TotalSeconds > 0.5)
            {
                this.GetComponent<BoxCollider2D>().enabled = true;
                isCasting = false;
            }
            //UnityEngine.Debug.Log(fillImage.fillAmount);
            fillImage.fillAmount = ((float)abilityCooldownTimer.Elapsed.TotalSeconds / fba.AbilityCooldown);
            yield return null;
        }
        fillImage.fillAmount = 0;
        this.GetComponent<BoxCollider2D>().enabled = true;
        isCasting = false;
        button.interactable = true;
        abilityCooldownTimer.Stop();
        abilityCooldownTimer.Reset();

        yield return null;
    }

}
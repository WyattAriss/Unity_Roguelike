using UnityEngine;
using System.Collections;
using Completed;

public class Impact : MonoBehaviour {

    public GameObject Explosion;
    public bool dealDamage = false;
    public int minSpellDamage;
    public int maxSpellDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("COLLISION");
        Destroy(gameObject);
        GameObject.Instantiate(Explosion, other.transform.position, Quaternion.identity);
        //Destroy object on collision
        //do damage here again
        if (dealDamage == true)
        {
            int randSpellDamage = Random.Range(minSpellDamage, maxSpellDamage + 1);
            other.gameObject.GetComponent<Enemy>().DamageEnemy(randSpellDamage);
        }
    }

}

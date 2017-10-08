using UnityEngine;
using System.Collections;
using Completed;

public class AOE : MonoBehaviour
{

    public bool dealDamage = false;
    public int minAOESpellDamage;
    public int maxAOESpellDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dealDamage == true)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                int randAOESpellDamage = Random.Range(minAOESpellDamage, maxAOESpellDamage + 1);
                other.gameObject.GetComponent<Enemy>().DamageEnemy(randAOESpellDamage);
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                int randAOESpellDamage = Random.Range(minAOESpellDamage, maxAOESpellDamage + 1);
                other.gameObject.GetComponent<Player>().health -= randAOESpellDamage;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int attackDamage = 3;

    public float attackCooldown = 3f;

    private bool playerCanBeAttacked;

    // Start is called before the first frame update
    void Start()
    {
        playerCanBeAttacked = true;



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "weaponhitbox")
        {
            Debug.Log("hit");

            this.transform.parent.GetComponent<EnemyHealth>().TakeDamage(other.gameObject.GetComponent<Weapon>().damage);

        }

        else if (other.tag == "playerhitbox")
        {
            if (!playerCanBeAttacked){ return; }
            Debug.Log("hitting player...");

            this.transform.parent.GetChild(0).GetComponent<Animator>().SetTrigger("Attacking");

            //this.transform.parent.GetComponent<EnemyHealth>().TakeDamage(other.gameObject.GetComponent<Weapon>().damage);
            PlayerHealth.instance.TakeDamage(attackDamage);

            StartCoroutine(PlayerCanBeAttackedCooldown());
        }



    }

    private IEnumerator PlayerCanBeAttackedCooldown()
    {
        playerCanBeAttacked = false;
        Debug.Log("waiting 3 seconds...");
        Attack.instance.gameObject.transform.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(attackCooldown);
        playerCanBeAttacked = true;
        Debug.Log("wait over");
        Attack.instance.gameObject.transform.GetComponent<Collider>().enabled = true;


    }




}

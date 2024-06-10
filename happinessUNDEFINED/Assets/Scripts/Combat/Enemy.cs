using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int attackDamage = 3;

    public float attackCooldown = 3f;

    private bool playerCanBeAttacked;

    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCanBeAttacked = true;

        rb = this.transform.parent.GetComponent<Rigidbody2D>();



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "weaponhitbox")
        {
            //Debug.Log("hit");

            this.transform.parent.GetComponent<EnemyHealth>().TakeDamage(other.gameObject.GetComponent<Weapon>().damage);


                Vector2 knockbackDirection = other.GetComponent<Collider>().transform.position - transform.position;
                float knockbackForce = other.GetComponent<Weapon>().knockback; // Replace with your weapon's knockback force
                ApplyKnockback(knockbackDirection, knockbackForce);

        }



        else if (other.tag == "playerhitbox")
        {
            if (!KodaManager.instance.worldCanMove){ return; }
            if (!playerCanBeAttacked){ return; }
            //Debug.Log("hitting player...");

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



    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb != null)
        {
            isKnockedBack = true;
            rb.velocity = direction.normalized * force;
            StartCoroutine(KnockbackCoroutine());
        }
    }

    private IEnumerator KnockbackCoroutine()
    {
        yield return new WaitForSeconds(0.2f); // Adjust the duration of the knockback effect
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }




}

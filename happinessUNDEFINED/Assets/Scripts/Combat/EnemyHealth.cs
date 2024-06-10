using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private GameObject healthBar;
    private Image healthBarForeground;

    private EnemyAI enemyAI;

    private GameObject ActionPopup;

    private bool canBeAttacked;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        currentHealth = maxHealth;
        healthBar = this.transform.GetChild(2).gameObject;
        healthBarForeground = this.transform.GetChild(2).GetChild(1).GetComponent<Image>();
        healthBarForeground.fillAmount = currentHealth / maxHealth;
        ActionPopup = this.transform.GetChild(1).gameObject;

        ActionPopup.SetActive(false);

        canBeAttacked = true;


        // Adjust transparency

        LightUpHealthBar(0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LightUpHealthBar(1f);
            enemyAI.SpotPlayer(other.transform);
        }


    }


    private void OnTriggerStay(Collider other) // Player is in range; show health bar and action popup transform
    {
        if (other.tag == "Player")
        {
            LightUpHealthBar(1f);
            if (InventoryScript.instance.selectedItem == null)
            {
                ActionPopup.SetActive(false);
                return;
            }
            else if (InventoryScript.instance.selectedItem.tag == "harvestitem" || InventoryScript.instance.selectedItem.tag == "weapon")
            {
                ActionPopup.SetActive(true);

            }
            else
            {
                ActionPopup.SetActive(false);
            }

            if (!enemyAI.playerSpotted)
            {
                enemyAI.SpotPlayer(other.transform);
            }
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            ActionPopup.SetActive(false);
            enemyAI.LosePlayer();

            // Adjust transparency when player leaves the trigger

            LightUpHealthBar(0.5f);

        }
    }


    private void LightUpHealthBar(float t){
        Color transparent = healthBarForeground.color;
        transparent.a = t;
        healthBarForeground.color = transparent;

        transparent = Color.black;
        transparent.a = t;
        healthBarForeground.transform.parent.GetChild(0).GetComponent<Image>().color = transparent;
        healthBarForeground.transform.parent.GetChild(2).GetComponent<Image>().color = transparent;
    }



    public void TakeDamage(float damage)
    {
        // Update the health bar

        currentHealth -= damage;

        healthBarForeground.fillAmount = currentHealth / maxHealth;

        this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Injured");

        if (currentHealth / maxHealth > 0.50)
        {
            healthBarForeground.color = Color.white;
        }
        else if (currentHealth / maxHealth > 0.25)
        {
            healthBarForeground.color = new Color32(255, 145, 141, 255);
        }
        else
        {
            healthBarForeground.color = Color.red;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(CanBeAttackedCooldown());
    }

    private IEnumerator CanBeAttackedCooldown()
    {
        canBeAttacked = true;
        yield return new WaitForSeconds(0.3f);
        canBeAttacked = false;


    }




    private void Die()
    {
        Destroy(gameObject);
        Destroy(healthBar);
    }
}

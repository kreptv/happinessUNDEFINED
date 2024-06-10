using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    #region Singleton
    public static PlayerHealth instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    #endregion


    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    //public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        this.transform.GetChild(1).GetComponent<Image>().fillAmount = ((float)currentHealth / (float)maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        this.transform.GetChild(1).GetComponent<Image>().fillAmount = ((float)currentHealth / (float)maxHealth);

        PlayerMovementScript.instance.myAnimator.SetTrigger("injured");

        ColorTest();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ColorTest()
    {
        if ((float)currentHealth / (float)maxHealth > 0.50)
        {
            this.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.white;
        }
        else if ((float)currentHealth / (float)maxHealth > 0.25)
        {
            this.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(255, 145, 141, 255);
        }
        else
        {
            this.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        ColorTest();

        this.transform.GetChild(1).GetComponent<Image>().fillAmount = ((float)currentHealth / (float)maxHealth);
    }

    private void Die()
    {
        Debug.Log("Player died!");

        PlayerMovementScript.instance.myAnimator.SetBool("fallToKnees", true);

        KodaManager.instance.kodaCanMove = false;

        // Add death logic here (e.g., respawn, game over screen, etc.)
    }
}
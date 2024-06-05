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

    void Update()
    {
        // For testing purposes, pressing H to heal and J to take damage
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(20);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        this.transform.GetChild(1).GetComponent<Image>().fillAmount = ((float)currentHealth / (float)maxHealth);

        PlayerMovementScript.instance.myAnimator.SetTrigger("injured");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        this.transform.GetChild(1).GetComponent<Image>().fillAmount = ((float)currentHealth / (float)maxHealth);
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Add death logic here (e.g., respawn, game over screen, etc.)
    }
}
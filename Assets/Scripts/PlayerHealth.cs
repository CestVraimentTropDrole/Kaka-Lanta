using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }

        if(RFIDManager.instance != null && RFIDManager.instance.HasHeart())
        {
            Heal(20);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth < 0)
            currentHealth = 0;
            
        healthBar.SetHealth(currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void TakeDamageFromStarvation(int damage)
    {
        TakeDamage(damage);
    }

    private void Heal(int amount)
    {
        if (RFIDManager.instance.HasHeart())
        {
            currentHealth += 20;
            healthBar.SetHealth(currentHealth);
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }

        }

    }
    
    private void Die()
    {
        Debug.Log("💀 Le joueur est mort !");
        // Ici vous pouvez ajouter : écran de Game Over, restart, etc.
    }
}

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
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

        if (currentHealth == 0)
            Die();
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

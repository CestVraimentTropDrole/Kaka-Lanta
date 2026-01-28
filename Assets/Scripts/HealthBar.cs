using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private PlayersManager manager;
    private PlayerHealth currentPlayerHealth;

    void Start()
    {
        manager = FindFirstObjectByType<PlayersManager>();
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (manager == null) return;

        // Récupère le joueur actif
        GameObject activePlayer = manager.GetActivePlayer();
        if (activePlayer == null) return;

        // Récupère le script PlayerHealth du joueur actif
        PlayerHealth playerHealth = activePlayer.GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            // Met à jour la barre avec la vie du joueur actif
            slider.maxValue = playerHealth.maxHealth;
            slider.value = playerHealth.currentHealth;
        }
    }
}
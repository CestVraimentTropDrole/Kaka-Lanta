using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider slider;
    private PlayersManager manager;

    void Start()
    {
        manager = FindFirstObjectByType<PlayersManager>();
        UpdateHungerBar();
    }

    void Update()
    {
        UpdateHungerBar();
    }

    void UpdateHungerBar()
    {
        if (manager == null) return;

        // Récupère le joueur actif
        GameObject activePlayer = manager.GetActivePlayer();
        
        if (activePlayer == null) return;

        // Récupère le script PlayerHunger du joueur actif
        PlayerHunger playerHunger = activePlayer.GetComponent<PlayerHunger>();
        
        if (playerHunger != null)
        {
            // Met à jour la barre avec la faim du joueur actif
            slider.maxValue = playerHunger.maxHunger;
            slider.value = playerHunger.currentHunger;
        }
    }

    public void UpdateHungerBar(float current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
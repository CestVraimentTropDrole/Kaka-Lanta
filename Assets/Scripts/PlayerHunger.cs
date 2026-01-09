using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    public int maxHunger = 6;
    public int currentHunger;

    public HungerBar hungerBar;

    [Header("Dégâts de famine")]
    [SerializeField] private int starvationDamage = 20;

    void Start()
    {
        currentHunger = maxHunger;
        hungerBar.SetMaxHunger(maxHunger);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoseHunger(1);
        }
    }

    void LoseHunger(int hunger)
    {
        currentHunger -= hunger;
        if (currentHunger <= 0)
        {
            currentHunger = 0;
        }

        hungerBar.SetHunger(currentHunger);

        if (currentHunger == 0)
        {
            Debug.Log("Le joueur subit des dégâts de famine !");
            ApplyStarvationDamage();
        }
    }

    public void LoseHungerFromTurn(int hunger)
    {
        LoseHunger(hunger);
    }

    private void ApplyStarvationDamage()
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamageFromStarvation(starvationDamage);
            Debug.Log($"💀 Famine ! -{starvationDamage} PV");
        }
    }
}

using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    public int maxHunger = 6;
    public int currentHunger;

    public HungerBar hungerBar;

    [Header("Dégâts de famine")]
    [SerializeField] private int starvationDamage = 1;

    void Start()
    {
        currentHunger = maxHunger;
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

        if (currentHunger == 0)
        {
            ApplyStarvationDamage();
        }
    }


    void GainHunger(int hunger)
    {
        currentHunger += hunger;
        if (currentHunger > maxHunger)
        {
            currentHunger = maxHunger;
        }
    }

    public void LoseHungerFromTurn(int hunger)
    {
        LoseHunger(hunger);
    }

    public void GainHungerFromFood(int hunger)
    {
        GainHunger(hunger);
    }

    private void ApplyStarvationDamage()
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamageFromStarvation(starvationDamage);
        }
    }
}

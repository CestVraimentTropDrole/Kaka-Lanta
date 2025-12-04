using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    public int maxHunger = 6;
    public int currentHunger;

    public HungerBar hungerBar;

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
        hungerBar.SetHunger(currentHunger);
    }
}

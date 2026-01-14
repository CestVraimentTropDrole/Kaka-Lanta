using UnityEngine;

public class StonePickup : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int stoneValue = 1;

    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            PickupStone();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    private void PickupStone()
    {
        if (Inventory.instance == null)
        {
            return;
        }

        Inventory.instance.AddStone(stoneValue);

        Destroy(gameObject);
    }
}
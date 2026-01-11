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
            Debug.Log("→ Joueur près du stone: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("← Joueur éloigné du stone: " + gameObject.name);
        }
    }

    private void PickupStone()
    {
        if (Inventory.instance == null)
        {
            Debug.LogError("Inventory.instance est null!");
            return;
        }

        Inventory.instance.AddStone(stoneValue);

        Destroy(gameObject);
    }
}
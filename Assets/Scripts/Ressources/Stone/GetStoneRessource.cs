using UnityEngine;


public class StoneResource : MonoBehaviour
{
    private bool playerInZone = false;

    [Header("Sound")]
    public AudioClip PickupSound;
    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            HarvestStone();
             if (PickupSound != null)
{
    GameObject tempAudio = new GameObject("TempAudio");
    tempAudio.transform.position = transform.position;
    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
    audioSource.clip = PickupSound;
    audioSource.volume = 2f; // Définis le volume AVANT de jouer
    audioSource.Play();
    Destroy(tempAudio, PickupSound.length);
}
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

    private void HarvestStone()
    {
        if (Inventory.instance == null)
        {
            return;
        }
        
        int baseAmount = RFIDManager.instance.HasPioche() ? 1 : 1;
        
        if (GameEventSystem.instance != null)
        {
            baseAmount *= GameEventSystem.instance.GetWoodMultiplier();
        }

        Inventory.instance.AddStone(baseAmount);

        if (GameEventSystem.instance != null && GameEventSystem.instance.doubleStoneEvent)
        {
            Debug.Log($"Bois récolté : {baseAmount}");
        }
        else
        {
            Debug.Log($"Bois récolté : {baseAmount}");
        }

        Destroy(gameObject);
    }
}
using UnityEngine;

public class WoodPickup : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int woodValue = 1;

    [Header("Sound")]
    public AudioClip PickupSound;
    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            PickupWood();
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

    private void PickupWood()
    {
        if (Inventory.instance == null)
        {
            return;
        }

        Inventory.instance.AddWood(woodValue);

        Destroy(gameObject);
    }
}
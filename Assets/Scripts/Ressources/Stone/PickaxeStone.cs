using UnityEngine;

public class PickaxeStone : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject stonePrefab; // Le prefab de petite pierre à ramasser
    [SerializeField] private int minStoneDropped = 2;
    [SerializeField] private int maxStoneDropped = 4;
    [SerializeField] private float dropRadius = 1f;
    
    private bool playerInZone = false;

    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            BreakStone();
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

    private void BreakStone()
    {
        if (RFIDManager.instance == null || !RFIDManager.instance.HasPioche())
        {
            return;
        }

        if (stonePrefab == null)
        {
            return;
        }

        // Déterminer combien de pierres vont tomber
        int stoneAmount = Random.Range(minStoneDropped, maxStoneDropped + 1);

        // Faire spawn les pierres autour du rocher
        for (int i = 0; i < stoneAmount; i++)
        {
            // Position aléatoire autour du rocher
            Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            // Créer la pierre
            Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
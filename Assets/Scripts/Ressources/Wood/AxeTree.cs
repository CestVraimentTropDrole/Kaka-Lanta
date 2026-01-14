using UnityEngine;

public class AxeTree : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private int minWoodDropped = 2;
    [SerializeField] private int maxWoodDropped = 4;
    [SerializeField] private float dropRadius = 1f;
    
    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            BreakTree();
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

    private void BreakTree()
    {
        if (RFIDManager.instance == null || !RFIDManager.instance.HasAxe())
        {
            return;
        }

        if (treePrefab == null)
        {
            return;
        }

        // Déterminer combien de pierres vont tomber
        int woodAmount = Random.Range(minWoodDropped, maxWoodDropped + 1);

        // Faire spawn les pierres autour du rocher
        for (int i = 0; i < woodAmount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
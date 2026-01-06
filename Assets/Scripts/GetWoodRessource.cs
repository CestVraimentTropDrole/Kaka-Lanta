using UnityEngine;

public class GetWoodRessource : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory.instance.AddWood(1);
            Destroy(gameObject);
        }
    }
}

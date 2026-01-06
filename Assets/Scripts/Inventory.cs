using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int woodCount;
    public TextMeshProUGUI woodCountText;

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance Inventory dans la scène");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        woodCountText.text = woodCount.ToString();
    }

    public void AddWood(int count)
    {
        woodCount += count;
        woodCountText.text = woodCount.ToString();
    }
}

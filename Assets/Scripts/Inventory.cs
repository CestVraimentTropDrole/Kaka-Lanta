using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int woodCount;
    public int stoneCount;
    public TextMeshProUGUI woodCountText;
    public TextMeshProUGUI stoneCountText;

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
        stoneCountText.text = stoneCount.ToString();
    }

    public void AddWood(int count)
    {
        woodCount += count;
        woodCountText.text = woodCount.ToString();
    }

    public void AddStone(int count)
    {
        stoneCount += count;
        stoneCountText.text = stoneCount.ToString();
    }

    public void RemoveWood(int count)
    {
        woodCount -= count;
        if (woodCount < 0)
            woodCount = 0;
            
        woodCountText.text = woodCount.ToString();
        Debug.Log($"🪵 -{count} bois (Total: {woodCount})");
    }

    public void RemoveStone(int count)
    {
        stoneCount -= count;
        if (stoneCount < 0)
            stoneCount = 0;
            
        stoneCountText.text = stoneCount.ToString();
        Debug.Log($"🪨 -{count} pierre (Total: {stoneCount})");
    }

    public int GetWoodCount()
    {
        return woodCount;
    }

    public int GetStoneCount()
    {
        return stoneCount;
    }

    public bool HasEnoughWood(int amount)
    {
        return woodCount >= amount;
    }

    public bool HasEnoughStone(int amount)
    {
        return stoneCount >= amount;
    }
}
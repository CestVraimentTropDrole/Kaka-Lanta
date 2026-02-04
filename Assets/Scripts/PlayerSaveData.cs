using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int playerNumber;
    public Vector3 position;

    public float currentHunger;
    public int maxHunger;
    public int currentHealth;
    public int maxHealth;
}

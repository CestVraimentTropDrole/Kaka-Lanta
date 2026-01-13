using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public int numberPlayers = 2;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Il y a plus d'une instance GameData dans la scène");
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetNumberOfPlayers(int number)
    {
        numberPlayers = Mathf.Clamp(number, 2, 4);
        Debug.Log("Number of players set to: " + numberPlayers);
    }
}
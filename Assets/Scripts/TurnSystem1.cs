using UnityEngine;
using System.IO.Ports;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem instance;

    [Header("Config des tours")]

    [SerializeField] private float turnDuration = 30f;
    [SerializeField] private TMP_Text turnNumberText;
    [SerializeField] private float maxDays = 10;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject endTurnButton;

    private int currentTurn = 1;
    private float currentTurnTime;
    private bool turnActive = true;
    private PlayersManager manager;
    private int totalPlayers;
    private int currentDay = 1;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        manager = FindFirstObjectByType<PlayersManager>();
        if (manager == null) { Debug.LogError("PlayersManager non trouvé dans la scène !"); }

        totalPlayers = manager.GetNumberOfPlayers();

        StartNewTurn();
    }

    void Update()
    {
        if (!turnActive) return;

        currentTurnTime -= Time.deltaTime;

        UpdateUI();

        if (currentTurnTime <= 0)
        {
            manager.NextPlayer();
            EndTurn();
        }
    }

    private void StartNewTurn()
    {
        currentTurnTime = turnDuration;
        turnActive = true;

        Debug.Log($"Tour {currentTurn} commence !");

        OnTurnStart();
    }

    public void EndTurn()
    {
        if (!turnActive) return;

        turnActive = false;

        Debug.Log($"Tour {currentTurn} terminé !");

        OnTurnEnd();

        Invoke(nameof(NextTurn), 3f);
    }

    private void NextTurn()
    {
        currentTurn++;

        if (currentTurn % totalPlayers == 1)    // Quand tous les joueurs ont fini leur tour
        {
            currentDay++;   // Change de jour
            if (GameData.instance != null) { 
                GameData.instance.SetCurrentRound(currentDay);  // Sauvegarde le jour actuel
                manager.SavePlayers();  // Sauvegarde les joueurs
                Debug.Log("Joueurs sauvegardés dans GameData.");
            }

            Debug.Log($"Nouveau jour {currentDay} !");
            Debug.Log($"Lancement mini-jeu");

            SceneManager.LoadScene("MinigamesScene");    // Change vers la scène de mini-jeu
        }

        if (maxDays > 0 && currentTurn > maxDays)
        {
            EndGame();
            return;
        }

        StartNewTurn();
    }

    private void UpdateUI()
    {
        if (turnNumberText != null)
        {
            if (maxDays > 0)
                turnNumberText.text = $"Jour {currentDay}/{maxDays} \n Tour Joueur {((currentTurn - 1) % totalPlayers) + 1}/{totalPlayers}";
            else
                turnNumberText.text = $"Jour {currentTurn}";
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTurnTime / 60);
            int seconds = Mathf.FloorToInt(currentTurnTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void OnTurnStart()
    {
        // Actions au début de chaque tour
    }

    private void OnTurnEnd()
    {
        PlayerHunger playerHunger = FindFirstObjectByType<PlayerHunger>();
        if (playerHunger != null)
        {
            int hungerLoss = CalculateHungerLoss();
            playerHunger.LoseHungerFromTurn(hungerLoss);
        }
    }

    private int CalculateHungerLoss()
    {
        return 2 + (currentTurn - 1) / 3;
    }

    private void EndGame()
    {
        Debug.Log("🎯 Partie terminée !");
        turnActive = false;
    }

    public bool IsTurnActive()
    {
        return turnActive;
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }

    public float GetRemainingTime()
    {
        return currentTurnTime;
    }

    public void AddTime(float seconds)
    {
        currentTurnTime += seconds;
        Debug.Log($"⏰ +{seconds} secondes ajoutées au tour");
    }

    public void OnEndTurnButtonClicked()
    {
        EndTurn();
    }
}
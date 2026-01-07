using UnityEngine;
using System.IO.Ports;
using System;

public class RFIDManager : MonoBehaviour
{
    [Header("Arduino RFID")]
    [SerializeField] private string portRFID = "COM6";

    [Header("Arduino Bouton")]
    [SerializeField] private string portBouton = "COM8";

    [SerializeField] private int baudRate = 9600;

    private SerialPort serialRFID;
    private SerialPort serialBouton;

    void Start()
    {
        try
        {
            serialRFID = new SerialPort(portRFID, baudRate);
            serialBouton = new SerialPort(portBouton, baudRate);

            serialRFID.ReadTimeout = 100;
            serialBouton.ReadTimeout = 100;

            serialRFID.Open();
            serialBouton.Open();

            Debug.Log("Ports série ouverts avec succès");
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur ouverture port série : " + e.Message);
        }
    }

    void Update()
    {
        // RFID
        if (serialRFID != null && serialRFID.IsOpen && serialRFID.BytesToRead > 0)
        {
            try
            {
                string rfidData = serialRFID.ReadLine();
                Debug.Log("RFID: " + rfidData);
            }
            catch { }
        }

        // Bouton
        if (serialBouton != null && serialBouton.IsOpen && serialBouton.BytesToRead > 0)
        {
            try
            {
                string boutonData = serialBouton.ReadLine();
                Debug.Log("Bouton: " + boutonData);
            }
            catch { }
        }
    }

    void OnDestroy()
    {
        if (serialRFID != null && serialRFID.IsOpen)
            serialRFID.Close();

        if (serialBouton != null && serialBouton.IsOpen)
            serialBouton.Close();
    }
}

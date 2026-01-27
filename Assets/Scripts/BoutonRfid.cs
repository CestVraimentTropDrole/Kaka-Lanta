using UnityEngine;
using System.IO.Ports;
using System;

public class RFIDManager : MonoBehaviour
{
    public static RFIDManager instance;

    [SerializeField] private string portName = "COM8";
    [SerializeField] private int baudRate = 9600;

    private SerialPort serial;

    private string lastRFID = "";
    private bool buttonJustPressed = false;

    private bool _AL, _AR, _AU, _AD;

    public bool AL() => _AL;
    public bool AR() => _AR;
    public bool AU() => _AU;
    public bool AD() => _AD;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        try
        {
            serial = new SerialPort(portName, baudRate);
            serial.ReadTimeout = 100;
            serial.Open();
            Debug.Log("Arduino connecté");
        }
        catch (Exception e)
        {
            
        }
    }

    void Update()
    {
        buttonJustPressed = false;

        if (serial != null && serial.IsOpen && serial.BytesToRead > 0)
        {
            try
            {
                string data = serial.ReadLine().Trim();
                Debug.Log(data);

                // ---------- RFID ----------
                if (data.StartsWith("RFID:"))
                {
                    lastRFID = data;
                }

                // ---------- BOUTON ----------
                else if (data == "BTN:PRESSED")
                {
                    buttonJustPressed = true;
                }

                // ---------- JOYSTICK ----------
                else if (data.StartsWith("JOY:"))
                {
                    _AL = _AR = _AU = _AD = false;

                    switch (data)
                    {
                        case "JOY:LEFT":  _AR = true; break;
                        case "JOY:RIGHT": _AL = true; break;
                        case "JOY:UP":    _AU = true; break;
                        case "JOY:DOWN":  _AD = true; break;
                    }
                }
            }
            catch { }
        }

        // Contrôles clavier
        if (Input.GetKey(KeyCode.Q)) _AL = true;
        if (Input.GetKey(KeyCode.D)) _AR = true;
        if (Input.GetKey(KeyCode.Z)) _AU = true;
        if (Input.GetKey(KeyCode.S)) _AD = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonJustPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            lastRFID = "RFID:O1";
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            lastRFID = "RFID:O2";
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            lastRFID = "RFID:O3";
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            lastRFID = "RFID:O4";
        }
    }
    
    public bool IsButtonPressed() => buttonJustPressed;

    public bool HasAxe()        => lastRFID == "RFID:O1";
    public bool HasPioche()     => lastRFID == "RFID:O2";
    public bool HasFishingRod() => lastRFID == "RFID:O3";
    public bool HasHeart()      => lastRFID == "RFID:O4";

    public string GetLastRFID() => lastRFID;

    void OnApplicationQuit()
    {
        if (serial != null && serial.IsOpen)
            serial.Close();
    }
}

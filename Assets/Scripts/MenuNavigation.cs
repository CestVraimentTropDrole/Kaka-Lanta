using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuJoystickNavigation : MonoBehaviour
{
    public Selectable[] menuButtons;
    private int currentIndex = 0;
    private float inputCooldown = 0.25f;
    private float lastInputTime;
    private bool dropdownOpen = false;

    void Start()
    {
        SelectButton(0);
    }

    void Update()
    {
        if (Time.time - lastInputTime > inputCooldown)
        {
            if (RFIDManager.instance.AU())
            {
                if (dropdownOpen)
                    ChangeDropdown(-1);
                else
                    MoveSelection(-1);
            }
            else if (RFIDManager.instance.AD())
            {
                if (dropdownOpen)
                    ChangeDropdown(1);
                else
                    MoveSelection(1);
            }
        }
        if (RFIDManager.instance.IsButtonPressed())
        {
            PressButton();
        }
    }

    void MoveSelection(int direction)
    {
        currentIndex += direction;

        if (currentIndex < 0) currentIndex = menuButtons.Length - 1;
        if (currentIndex >= menuButtons.Length) currentIndex = 0;

        SelectButton(currentIndex);
        lastInputTime = Time.time;
    }

    void SelectButton(int index)
    {
        if (EventSystem.current != null) {
            EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
        }
    }

    void PressButton()
    {
        var selected = menuButtons[currentIndex];

        // 🔽 CAS DROPDOWN
        TMP_Dropdown dropdown = selected.GetComponent<TMP_Dropdown>();
        if (dropdown != null)
        {
            if (!dropdownOpen)
            {
                dropdown.Show();
                dropdownOpen = true;
            }
            else
            {
                dropdown.Hide();
                dropdownOpen = false;
            }
            return;
        }

        // 🔘 CAS BOUTON CLASSIQUE
        Button btn = selected.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.Invoke();
        }
    }

    void ChangeDropdown(int direction)
    {
        TMP_Dropdown dropdown = menuButtons[currentIndex].GetComponent<TMP_Dropdown>();
        if (dropdown == null) return;

        int newValue = dropdown.value + direction;

        if (newValue < 0) newValue = dropdown.options.Count - 1;
        if (newValue >= dropdown.options.Count) newValue = 0;

        dropdown.value = newValue;
        dropdown.RefreshShownValue();
        lastInputTime = Time.time;
    }
}
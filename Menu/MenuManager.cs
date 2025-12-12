using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Main menu UI elements")]
    [SerializeField] private RectTransform arrowMain;
    [SerializeField] private TextMeshProUGUI[] mainTexts;
    [SerializeField] private float mainArrowOffset = 350f;

    // color for disable option
    [SerializeField] private Color disabledColor = Color.gray;

    [Header("Settings UI elements")]
    [SerializeField] private RectTransform arrowSettings;
    [SerializeField] private TextMeshProUGUI[] settingsTexts;
    [SerializeField] private Image[] checkBoxes; // [0] - music, [1] - ambient, [2] - sound effects
    [SerializeField] private TextMeshProUGUI returnText;
    [SerializeField] private Sprite boxChecked;
    [SerializeField] private Sprite boxUnchecked;
    [SerializeField] private float settingsArrowOffset = 250f;

    [Header("Colors")]
    [SerializeField] private Color selectedColor = new Color(255f/255f, 151f/255f, 47f/255f);
    [SerializeField] private Color unselectedColor = new Color(51f/255f, 48f/255f, 48f/255f);

    // internal states
    private int currentMainIndex = 1;
    private int currentSettingsIndex = 0;
    private bool isInSettings = false;

    // saved
    private bool hasSavedGame = false;

    // input 
    private float verticalInput;
    private bool isPressedDown;
    private bool selectPressed;

    private float inputCooldown = 0.2f;
    private float lastInputTime;

    private void Start()
    {
        // verify if there is a saved game
        if (GameManager.Instance != null)
        {
            hasSavedGame = GameManager.Instance.HasSaveFile();
        }

        // if exist a saved game, start in continue game
        currentMainIndex = hasSavedGame ? 0 : 1;

        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        UpdateMainVisuals();
    }

    private void Update()
    {
        if (Time.time - lastInputTime > inputCooldown)
        {
            if (verticalInput > 0.2f)
            {
                MoveUp();
                lastInputTime = Time.time;
            }
            else if (verticalInput < -0.2f)
            {
                MoveDown();
                lastInputTime = Time.time;
            }
        }

        if (selectPressed)
        {
            selectPressed = false;
            ConfirmSelection();
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        verticalInput = input.y;
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selectPressed = true;
        }
    }

    private void MoveUp()
    {
        if (!isInSettings)
        {
            currentMainIndex--;

            int minIndex = hasSavedGame ? 0 : 1;

            if (currentMainIndex < minIndex)
            {
                currentMainIndex = 3;
            }

            UpdateMainVisuals();
        }
        else
        {
            currentSettingsIndex--;
            if (currentSettingsIndex < 0)
            {
                currentSettingsIndex = 3;
            }

            UpdateSettingsVisuals();
        }
    }

    private void MoveDown()
    {
        if (!isInSettings)
        {
            currentMainIndex++;
            if (currentMainIndex > 3)
            {
                currentMainIndex = hasSavedGame ? 0 : 1;
            }
            UpdateMainVisuals();
        }
        else 
        {
            currentSettingsIndex++;
            if (currentSettingsIndex > 3)
            {
                currentSettingsIndex = 0;
            }
            UpdateSettingsVisuals();
        }
    }

    private void UpdateMainVisuals()
    {
        for (int i = 0; i < mainTexts.Length; i++)
        {
            if (i == 0 && !hasSavedGame)
            {
                mainTexts[i].color = disabledColor;
            }
            else
            {
                mainTexts[i].color = unselectedColor;
            }
        }

        // Update the color of the selected text
        if (currentMainIndex >= 0 && currentMainIndex < mainTexts.Length)
        {
            TextMeshProUGUI target = mainTexts[currentMainIndex];
            target.color = selectedColor;

            arrowMain.anchoredPosition = new Vector2(target.rectTransform.anchoredPosition.x - mainArrowOffset, target.rectTransform.anchoredPosition.y + 15f);
        }
    }

    private void UpdateSettingsVisuals()
    {
        foreach (var txt in settingsTexts)
        {
            txt.color = unselectedColor;
        }

        if (currentSettingsIndex >= 0 && currentSettingsIndex < settingsTexts.Length)
        {
            TextMeshProUGUI target = settingsTexts[currentSettingsIndex];
            target.color = selectedColor;

            arrowSettings.anchoredPosition = new Vector2(target.rectTransform.anchoredPosition.x - settingsArrowOffset, target.rectTransform.anchoredPosition.y + 15f);
        }

        // update the checkbox according to SoundManager
        if (SoundManager.Instance != null)
        {
            if (checkBoxes.Length > 0) 
            {
                checkBoxes[0].sprite = SoundManager.Instance.IsMusicMuted() ? boxUnchecked : boxChecked;
            }
            if (checkBoxes.Length > 1) 
            {
                checkBoxes[1].sprite = SoundManager.Instance.IsAmbientMuted() ? boxUnchecked : boxChecked;
            }
            if (checkBoxes.Length > 2) 
            {
                checkBoxes[2].sprite = SoundManager.Instance.IsSfxMuted() ? boxUnchecked : boxChecked;
            }
        }
    }

    private void ConfirmSelection()
    {
        if (!isInSettings)
        {
            switch (currentMainIndex)
            {
                case 0:
                    if (hasSavedGame)
                    {
                        GameManager.Instance.IsLoadingContinue = true;
                        SceneManager.LoadScene("Game_scene");
                    }
                    break;
                case 1:
                    GameManager.Instance.IsLoadingContinue = false;
                    GameManager.Instance.ClearSave();
                    SceneManager.LoadScene("Game_scene");
                    break;
                case 2:
                    OpenSettings();
                    break;
                case 3:
                    Application.Quit();
                    break;
            }
        }
        else
        {
            switch (currentSettingsIndex)
            {
                case 0:
                    SoundManager.Instance.ToggleMusic();
                    UpdateSettingsVisuals();
                    break;
                case 1:
                    SoundManager.Instance.ToggleAmbient();
                    UpdateSettingsVisuals();
                    break;
                case 2:
                    SoundManager.Instance.ToggleSfx();
                    UpdateSettingsVisuals();
                    break;
                case 3:
                    CloseSettings();
                    break;
            }
        }
    }

    private void OpenSettings()
    {
        isInSettings = true;
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        currentSettingsIndex = 0;
        UpdateSettingsVisuals();
    }

    private void CloseSettings()
    {
        isInSettings = false;
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        UpdateMainVisuals();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PauseMenuHandler : MonoBehaviour
{
    [Header("General UI")]
    [SerializeField] private GameObject pauseCanvasObject;
    [SerializeField] private GameObject pauseMenuContainer;
    [SerializeField] private GameObject settingsContainer;

    [Header("Pause Options")]
    [SerializeField] private RectTransform arrowPause;
    [SerializeField] private TextMeshProUGUI[] pauseTexts;
    [SerializeField] private float pauseArrowOffset = 350f;

    [Header("Settings Options")]
    [SerializeField] private RectTransform arrowSettings;
    [SerializeField] private TextMeshProUGUI[] settingsTexts;
    [SerializeField] private Image[] checkBoxes;
    [SerializeField] private Sprite boxChecked;
    [SerializeField] private Sprite boxUnchecked;
    [SerializeField] private float settingsArrowOffset = 250f;

    [Header("Colors")]
    [SerializeField] private Color selectedColor = new Color(255f/255f, 151f/255f, 47f/255f);
    [SerializeField] private Color unselectedColor = new Color(51f/255f, 48f/255f, 48f/255f);

    // states
    private bool isPaused = false;
    private bool isInSettings = false;
    private int currentPauseIndex = 0;
    private int currentSettingsIndex = 0;

    // inputs
    private float verticalInput;
    private bool selectPressed;
    private float inputCooldown = 0.2f;
    private float lastInputTime;

    private void Start()
    {
        pauseCanvasObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player.GetComponent<InputHandler>() != null)
            {
                player.GetComponent<InputHandler>().OnPauseEvent += TogglePauseState;
            }
        }
    }

    private void Update()
    {
        if (!isPaused) return;

        if (Time.unscaledTime - lastInputTime > inputCooldown)
        {
            if (verticalInput > 0.2f)
            {
                MoveUp();
                lastInputTime = Time.unscaledTime;
            }
            else if (verticalInput < -0.2f)
            {
                MoveDown();
                lastInputTime = Time.unscaledTime;
            }
        }

        if (selectPressed)
        {
            selectPressed = false;
            ConfirmSelection();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseCanvasObject.SetActive(true);

        isInSettings = false;
        pauseMenuContainer.SetActive(true);
        settingsContainer.SetActive(false);

        currentPauseIndex = 0;
        UpdateVisuals();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseCanvasObject.SetActive(false);
    }

    public void TogglePauseState()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else 
        {
            PauseGame();
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!isPaused) return;

        Vector2 input = context.ReadValue<Vector2>();
        verticalInput = input.y;
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!isPaused) return;

        if (context.started)
        {
            selectPressed = true;
        }
    }

    private void MoveUp()
    {
        if (!isInSettings)
        {
            currentPauseIndex--;
            if (currentPauseIndex < 0) currentPauseIndex = 3;
        }
        else
        {
            currentSettingsIndex--;
            if (currentSettingsIndex < 0) currentSettingsIndex = 3;
        }
        UpdateVisuals();
    }

    private void MoveDown()
    {
        if (!isInSettings)
        {
            currentPauseIndex++;
            if (currentPauseIndex > 3) currentPauseIndex = 0;
        }
        else
        {
            currentSettingsIndex++;
            if (currentSettingsIndex > 3) currentSettingsIndex = 0;
        }
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (!isInSettings)
        {
            // Reset colors
            foreach (var t in pauseTexts) t.color = unselectedColor;
            
            // Set selected
            TextMeshProUGUI target = pauseTexts[currentPauseIndex];
            target.color = selectedColor;
            
            // Move arrow
            arrowPause.anchoredPosition = new Vector2(
                target.rectTransform.anchoredPosition.x - pauseArrowOffset,
                target.rectTransform.anchoredPosition.y + 15f
            );
        }
        else
        {
            // Reset colors
            foreach (var t in settingsTexts) t.color = unselectedColor;

            // Set selected
            TextMeshProUGUI target = settingsTexts[currentSettingsIndex];
            target.color = selectedColor;

            // Move arrow
            arrowSettings.anchoredPosition = new Vector2(
                target.rectTransform.anchoredPosition.x - settingsArrowOffset,
                target.rectTransform.anchoredPosition.y + 15f
            );

            // Update checkboxes
            if (SoundManager.Instance != null)
            {
                checkBoxes[0].sprite = SoundManager.Instance.IsMusicMuted() ? boxUnchecked : boxChecked;
                checkBoxes[1].sprite = SoundManager.Instance.IsAmbientMuted() ? boxUnchecked : boxChecked;
                checkBoxes[2].sprite = SoundManager.Instance.IsSfxMuted() ? boxUnchecked : boxChecked;
            }
        }
    }

    private void ConfirmSelection()
    {
        if (!isInSettings)
        {
            switch (currentPauseIndex)
            {
                case 0: 
                    ResumeGame();
                    break;
                case 1: 
                    OpenSettings();
                    break;
                case 2: 
                    Time.timeScale = 1f; 
                    if (GameManager.Instance != null) 
                    {
                        GameObject player = GameObject.FindGameObjectWithTag("Player");

                        if (player != null)
                        {
                            AbilityManager playerAbilities = player.GetComponent<AbilityManager>();
                            GameManager.Instance.SaveGame(player.transform.position, playerAbilities);
                        }
                    }
                    SceneManager.LoadScene("ui"); 
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
                case 0: SoundManager.Instance.ToggleMusic(); UpdateVisuals(); break;
                case 1: SoundManager.Instance.ToggleAmbient(); UpdateVisuals(); break;
                case 2: SoundManager.Instance.ToggleSfx(); UpdateVisuals(); break;
                case 3: CloseSettings(); break;
            }
        }
    }

    private void OpenSettings()
    {
        isInSettings = true;
        pauseMenuContainer.SetActive(false);
        settingsContainer.SetActive(true);
        currentSettingsIndex = 0;
        UpdateVisuals();
    }

    private void CloseSettings()
    {
        isInSettings = false;
        settingsContainer.SetActive(false);
        pauseMenuContainer.SetActive(true);
        UpdateVisuals();
    }
}

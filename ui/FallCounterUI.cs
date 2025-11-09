using UnityEngine;
using TMPro;

public class FallCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private string label = "FALLS = ";

    private GameManager cachedGameManager;

    private void Awake()
    {
        if (counterText == null)
        {
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        // Cache the instance once. If application is quitting or the manager was destroyed,
        // this will be null and we avoid creating a new GameObject from the Instance getter.
        cachedGameManager = GameManager.Instance;
        if (cachedGameManager != null)
        {
            counterText.text = label + cachedGameManager.FallCount;
            cachedGameManager.OnFallCountChanged += UpdateCounter;
        }
        else
        {
            // Fallback: show zero if no manager is available (safe during teardown)
            counterText.text = label + "0";
        }
    }

    private void OnDisable()
    {
        if (cachedGameManager != null)
            cachedGameManager.OnFallCountChanged -= UpdateCounter;
    }

    private void UpdateCounter(int newCount)
    {
        if (counterText != null)
            counterText.text = label + newCount;
    }
}

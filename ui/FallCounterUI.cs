using UnityEngine;
using TMPro;

public class FallCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;

    private void Awake()
    {
        if (counterText == null)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            counterText.text = "Falls: " + GameManager.Instance.FallCount;
            GameManager.Instance.OnFallCountChanged += UpdateCounter;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnFallCountChanged -= UpdateCounter;
    }

    private void UpdateCounter(int newCount)
    {
        counterText.text = "Falls: " + newCount;
    }
}

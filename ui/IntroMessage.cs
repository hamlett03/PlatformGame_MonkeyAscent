using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroMessage : MonoBehaviour
{
    [SerializeField] private GameObject messagePanel;

    [SerializeField] private float delayBeforeShowing = 4f;
    [SerializeField] private float displayDuration = 5f;

    private void Start()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }

        // ensure if is a new game
        if (GameManager.Instance != null && !GameManager.Instance.IsLoadingContinue)
        {
            StartCoroutine(ShowMessageRoutine());
        }
    }

    private IEnumerator ShowMessageRoutine()
    {
        yield return new WaitForSeconds(delayBeforeShowing);

        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }

        yield return new WaitForSeconds(displayDuration);

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
}

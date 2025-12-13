using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpFruit : MonoBehaviour
{
    [SerializeField] private AbilityData abilityToUnlock;
    [SerializeField] private GameObject feedbackPopupPrefab;

    private bool isCollected = false;

    private void Start()
    {
        StartCoroutine(CheckIfCollected());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            AbilityManager manager = other.GetComponent<AbilityManager>();
            if (manager != null)
            {
                manager.UnlockAbility(abilityToUnlock);
                CollectFeedback();
            }
        }
    }

    private void CollectFeedback()
    {
        isCollected = true;

        SoundManager.Instance.PlayPowerUpSound();

        float timeToDestroy = 0.5f;

        if (feedbackPopupPrefab != null)
        {
            GameObject popup = Instantiate(feedbackPopupPrefab);
            
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                popup.transform.SetParent(canvas.transform, false);
            }
            
            TextMeshProUGUI[] popupTexts = popup.GetComponentsInChildren<TextMeshProUGUI>();
            
            if (popupTexts.Length > 0 && popupTexts[0] != null)
            {
                popupTexts[0].text = $"Muy bien consumiste el fruto de habilidad de {abilityToUnlock.abilityName}!";
            }
        
            if (popupTexts.Length > 1 && popupTexts[1] != null)
            {
                popupTexts[1].text = "Puedes ver tus habilidades en la parte superior izquierda";
            }

            if (popupTexts.Length > 2 && popupTexts[2] != null)
            {
                popupTexts[2].text = "Presiona la habilidad mientras estes en el aire";
            }

            Destroy(popup, 5f);
        }
        
        Destroy(gameObject, timeToDestroy);
    }

    private System.Collections.IEnumerator CheckIfCollected()
    {
        yield return null;

        AbilityManager manager = FindObjectOfType<AbilityManager>();

        if (manager != null)
        {
            if (manager.HasAbility(abilityToUnlock))
            {
                Destroy(gameObject);
            }
        }
    }
}
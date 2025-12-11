using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameHandler : MonoBehaviour
{
    [SerializeField] private GameObject endPopupPrefab;
    [SerializeField] private Transform circleCenter;
    [SerializeField] private float popupDuration = 5f;

    [SerializeField] private float circleRadius = 6f;
    [SerializeField] private float circleSpeed = 1.4f;
    [SerializeField] private float flyAwaySpeed = 4.5f;
    [SerializeField] private float durationCircling = 8.5f;
    [SerializeField] private float exitDuration = 3f;

    private Animator anim;
    private bool hasTriggered = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure this only triggers once and only for the player
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(EndSequenceRoutine());
        }
    }

    private IEnumerator EndSequenceRoutine()
    {
        ShowEndMessage();

        yield return new WaitForSeconds(popupDuration);

        // start animation of flying
        if (anim != null)
        {
            anim.SetTrigger("fly");
        }

        // movement 
        yield return StartCoroutine(FlySquence());

        Debug.Log("End Game Sequence Completed");
    }

    private void ShowEndMessage()
    {
        if (endPopupPrefab != null)
        {
            GameObject popup = Instantiate(endPopupPrefab);
            
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                popup.transform.SetParent(canvas.transform, false);
            }
            
            TextMeshProUGUI[] popupTexts = popup.GetComponentsInChildren<TextMeshProUGUI>();
            
            if (popupTexts.Length > 0 && popupTexts[0] != null)
            {
                popupTexts[0].text = "Felicidades has llegado al final de la demo del juego";
            }

            if (popupTexts.Length > 1 && popupTexts[1] != null)
            {
                popupTexts[1].text = "Monkey Ascent";
            }
        
            if (popupTexts.Length > 2 && popupTexts[1] != null)
            {
                popupTexts[2].text = "Gracias por jugar la demo!";
            }

            if (popupTexts.Length > 3 && popupTexts[2] != null)
            {
                popupTexts[3].text = "Realizado por: Hamlet Gabriel Salinas Bravo";
            }

            Destroy(popup, popupDuration);
        }
    }

    private IEnumerator FlySquence()
    {
        float timer = 0f;
        Vector3 center = circleCenter != null ? circleCenter.position : transform.position;

        Vector3 initialOffset = transform.position - center;
        float initialAngle = Mathf.Atan2(initialOffset.y, initialOffset.x);

        // Circling phase
        while (timer < durationCircling)
        {
            timer += Time.deltaTime;

            float angle = initialAngle + timer * circleSpeed;

            // maths for circling
            float x = Mathf.Cos(angle) * circleRadius;
            float y = Mathf.Sin(angle) * circleRadius;

            // offset
            transform.position = center + new Vector3(x, y, 0);

            // flip sprite based on direction
            float direction = Mathf.Cos(angle);
            transform.localScale = new Vector3(Mathf.Sign(direction) * -1, 1, 1);

            yield return null;
        }

        // fly to top left
        float exitTimer = 0f;
        while (exitTimer < exitDuration)
        {
            exitTimer +=  Time.deltaTime;

            // move to top left
            transform.Translate(new Vector3(-1, 0.5f, 0) * flyAwaySpeed * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityContainer : MonoBehaviour
{
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI abilityNameText;

    private AbilityManager abilityManager;

    private void Start()
    {
        abilityManager = FindObjectOfType<AbilityManager>();
        
        if (abilityManager != null)
        {
            abilityManager.OnAbilityChanged += UpdateUI;

            abilityManager.UpdateAbilityState();
        }
    }

    private void OnDestroy()
    {
        if (abilityManager != null)
        {
            abilityManager.OnAbilityChanged -= UpdateUI;
        }
    }

    private void UpdateUI(AbilityData ability)
    {
        if (ability != null)
        {
            abilityIcon.sprite = ability.abilityIcon;
            abilityIcon.enabled = true;

            if (abilityNameText != null)
            {
                abilityNameText.text = ability.abilityName;
            }
        }
        else 
        {
            abilityIcon.enabled = false;

            if (abilityNameText != null)
            {
                abilityNameText.text = "X";
            }
        }
    }

    public void OnContainerClicked()
    {
        if (abilityManager != null)
        {
            abilityManager.CiclyNextAbility();
        }
    }
}

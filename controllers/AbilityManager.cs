using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    // list of abilities
    public List<AbilityData> unlockedAbilities = new List<AbilityData>();
    public List<AbilityData> allAbilitiesDataBase;

    // actual ability selected
    private int currentIndex = -1;

    // event to notify UI when ability changes
    public event Action<AbilityData> OnAbilityChanged;

    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Start()
    {
        if (stateMachine.Input != null)
        {
            stateMachine.Input.OnAbilitySelectionEvent += CiclyNextAbility;
        }
    }

    private void Update()
    {
        if (stateMachine.Input.AbilityPressed)
        {
            UseCurrentAbility();
        }
    }

    private void UseCurrentAbility()
    {
        if (currentIndex >= 0 && currentIndex < unlockedAbilities.Count)
        {
            unlockedAbilities[currentIndex].OnAbilityPressed(stateMachine);
        }
    }

    public void UnlockAbility(AbilityData newAbility)
    {
        if (!unlockedAbilities.Contains(newAbility))
        {
            unlockedAbilities.Add(newAbility);
            // if it's the first ability unlocked, select it
            if (unlockedAbilities.Count == 1)
            {
                currentIndex = 0;
            }

            UpdateAbilityState();
        }
    }

    public void CiclyNextAbility()
    {
        if (unlockedAbilities.Count == 0) return;

        currentIndex++;

        if (currentIndex >= unlockedAbilities.Count)
        {
            currentIndex = -1;
        }

        UpdateAbilityState();
    }

    public void UpdateAbilityState()
    {
        if (currentIndex >= 0 && currentIndex < unlockedAbilities.Count)
        {
            OnAbilityChanged?.Invoke(unlockedAbilities[currentIndex]);
        }
        else
        {
            OnAbilityChanged?.Invoke(null);
        }
    }

    public void RestoreAbilityByName(string name)
    {
        AbilityData abilityToRestore = allAbilitiesDataBase.Find(a => a.abilityName == name);

        if (abilityToRestore != null)
        {
            UnlockAbility(abilityToRestore);
        }
    }

    public bool HasAbility(AbilityData abilityToCheck)
    {
        return unlockedAbilities.Contains(abilityToCheck);
    }
}

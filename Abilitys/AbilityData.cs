using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public string abilityName;
    public Sprite abilityIcon;

    // each ability will implement its own behavior
    public abstract void OnAbilityPressed(PlayerStateMachine player);
}
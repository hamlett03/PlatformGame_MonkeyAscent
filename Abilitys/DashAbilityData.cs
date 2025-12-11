using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class DashAbilityData : AbilityData
{
    public override void OnAbilityPressed(PlayerStateMachine player)
    {
        // ability only works if the player is in the air
        if (!player.Ground.IsGrounded() && !player.HasUsedAbility)
        {
            player.HasUsedAbility = true;
            player.ChangeState(player.PlayerDashState);
        }
    }
}

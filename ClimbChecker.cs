using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClimbChecker : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private Vector2 offset = new Vector2(0.11f, 0.71f);
    [SerializeField] private Vector2 checkSize = new Vector2(0.15f, 0.87f);
    [SerializeField] private LayerMask climbLayer;

    // returns true if the player is touching a climbable object
    public bool IsClimbable()
    {
        if  (point != null)
        {
            Vector2 checkPosition = (Vector2)point.position + offset;

            return Physics2D.OverlapCapsule(
                checkPosition,
                checkSize,
                CapsuleDirection2D.Vertical,
                0,
                climbLayer
            );
        }
        else
        {
            return false;
        }
    }
}
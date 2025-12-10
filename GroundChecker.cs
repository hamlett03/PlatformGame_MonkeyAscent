using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundChecker : MonoBehaviour
{
    [System.Serializable]
    public class GroundCheckPoint
    {
        public Transform point;
        public Vector2 offset = Vector2.zero;
        public Vector2 checkSize = Vector2.zero;
    }

    [SerializeField] private GroundCheckPoint[] checkPoints = new GroundCheckPoint[3];
    [SerializeField] private int requiredGroundPoints = 2;
    [SerializeField] private LayerMask groundLayer;

    // Returns true if the player is grounded
    public bool IsGrounded()
    {
        int groundedPoints = 0;

        foreach (var checkPoint in checkPoints)
        {
            if (checkPoint != null && checkPoint.point != null)
            {
                Vector2 checkPosition = (Vector2)checkPoint.point.position + checkPoint.offset;
                if (Physics2D.OverlapCapsule(
                    checkPosition,
                    checkPoint.checkSize,
                    CapsuleDirection2D.Horizontal,
                    0,
                    groundLayer
                ))
                {
                    groundedPoints++;
                }
            }
        }

        return groundedPoints >= requiredGroundPoints;
    }
}
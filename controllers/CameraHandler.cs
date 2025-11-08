using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerHandler : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTargetNext;
    [SerializeField] private Transform cameraTargetPrevious;
    [SerializeField] private float playerThreshold = 0.1f;

    private bool playerInside = false;
    private Transform player; 

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (player.position.y > transform.position.y + playerThreshold)
            {
                MoveCameraTo(cameraTargetNext);
            }
            else if (player.position.y < transform.position.y - playerThreshold)
            {
                MoveCameraTo(cameraTargetPrevious);
                SoundManager.Instance.PlayCameraSnap();
            }
        }
    }

    private void MoveCameraTo(Transform target)
    {
        if (Camera.main != null && target != null)
        {
            Camera.main.transform.position = new Vector3(
                target.position.x,
                target.position.y,
                Camera.main.transform.position.z
            );
        }

        Debug.Log("Camera snapped to " + target.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (cameraTargetNext != null)
            Gizmos.DrawLine(transform.position, cameraTargetNext.position);
        if (cameraTargetPrevious != null)
            Gizmos.DrawLine(transform.position, cameraTargetPrevious.position);
    }
}
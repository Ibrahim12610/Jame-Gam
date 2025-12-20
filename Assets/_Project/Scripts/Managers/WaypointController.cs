using System;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [SerializeField] private Transform waypointTarget;
    [SerializeField] private GameObject waypoint;
    [SerializeField] private float edgePadding = 50f;
    [SerializeField] private GameObject waypointPanel;

    private void Update()
    {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(waypointTarget.position);

        if (IsWithinScreen(targetScreenPos))
        {
            waypoint.SetActive(false);
        }
        else
        {
            HandleOffScreen(targetScreenPos);
            waypoint.SetActive(true);
        }
    }

    private bool IsWithinScreen(Vector3 screenPos)
    {
        return screenPos.x > edgePadding &&
               screenPos.x < Screen.width - edgePadding &&
               screenPos.y > edgePadding &&
               screenPos.y < Screen.height - edgePadding;
    }

    private void HandleOffScreen(Vector3 screenPos)
    {
        Vector3 clampedScreenPos = ClampToScreenEdges(screenPos);

        waypointPanel.transform.SetPositionAndRotation(
            clampedScreenPos,
            CalculateRotationTowards(screenPos, clampedScreenPos)
        );
    }

    private Vector3 ClampToScreenEdges(Vector3 screenPos)
    {
        return new Vector3(
            Mathf.Clamp(screenPos.x, edgePadding, Screen.width - edgePadding),
            Mathf.Clamp(screenPos.y, edgePadding, Screen.height - edgePadding),
            screenPos.z
        );
    }

    private Quaternion CalculateRotationTowards(Vector3 targetPos, Vector3 waypointPos)
    {
        Vector2 direction = (waypointPos - targetPos).normalized;
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
    }
}

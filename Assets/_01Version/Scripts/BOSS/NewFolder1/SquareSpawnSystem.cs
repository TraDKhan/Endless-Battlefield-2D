using System.Collections.Generic;
using UnityEngine;

public class SquareSpawnSystem : MonoBehaviour
{
    [Header("Line Length")]
    [SerializeField] private float width = 10f;

    [Header("Distance Between Lines")]
    [SerializeField] private float spacing = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = transform.position;

        // Line giữa
        DrawLine(center);

        // Line trên
        DrawLine(center + Vector3.up * spacing);

        // Line dưới
        DrawLine(center + Vector3.down * spacing);
    }

    void DrawLine(Vector3 center)
    {
        Vector3 left = center + Vector3.left * width / 2;
        Vector3 right = center + Vector3.right * width / 2;

        Gizmos.DrawLine(left, right);
    }
}
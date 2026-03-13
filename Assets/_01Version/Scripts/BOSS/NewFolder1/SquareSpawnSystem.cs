using System.Collections.Generic;
using UnityEngine;

public class SquareSpawnSystem : MonoBehaviour
{
    [Header("Square")]
    public Transform squareA;
    public Transform squareB;
    public Vector2 size = new Vector2(5, 5);

    [Header("Spawn")]
    public int pointCount = 10;
    public GameObject bulletPrefab;
    public float moveSpeed = 5f;

    private List<Vector3> pointsA = new();
    private List<Vector3> pointsB = new();

    void Start()
    {
        GeneratePoints();
        SpawnBullets();
    }

    void GeneratePoints()
    {
        pointsA.Clear();
        pointsB.Clear();

        for (int i = 0; i < pointCount; i++)
        {
            float x = Random.Range(-size.x / 2, size.x / 2);
            float y = Random.Range(-size.y / 2, size.y / 2);

            Vector3 localPos = new Vector3(x, y, 0);

            Vector3 worldA = squareA.TransformPoint(localPos);
            Vector3 worldB = squareB.TransformPoint(localPos);

            pointsA.Add(worldA);
            pointsB.Add(worldB);
        }
    }

    void SpawnBullets()
    {
        for (int i = 0; i < pointsA.Count; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, pointsA[i], Quaternion.identity);
            bullet.GetComponent<MoveToTarget>().Init(pointsB[i], moveSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (squareA == null || squareB == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(squareA.position, size);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(squareB.position, size);

        Gizmos.color = Color.red;

        if (pointsA != null)
        {
            for (int i = 0; i < pointsA.Count; i++)
            {
                Gizmos.DrawSphere(pointsA[i], 0.1f);
                Gizmos.DrawSphere(pointsB[i], 0.1f);

                Gizmos.DrawLine(pointsA[i], pointsB[i]);
            }
        }
    }
}
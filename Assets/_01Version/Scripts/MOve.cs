using System;
using UnityEngine;

internal class MOve : MonoBehaviour
{
    public float moveSpeed = 5f;          // tốc độ di chuyển
    private Rigidbody2D rb;               // tham chiếu Rigidbody2D
    private Vector2 movement;             // vector nhập từ bàn phím

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Đảm bảo Rigidbody2D có Collision Detection = Continuous
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        // Lấy input từ bàn phím (WASD hoặc phím mũi tên)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // tránh di chuyển nhanh hơn khi đi chéo
    }

    void FixedUpdate()
    {
        // Di chuyển bằng velocity để Unity xử lý va chạm
        rb.linearVelocity = movement * moveSpeed;
    }

}


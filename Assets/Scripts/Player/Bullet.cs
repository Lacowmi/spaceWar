using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 5f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = Vector2.up * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private float speed;
    public Rigidbody2D rigidbody;
    public AudioSource engine;

    void Start()
    {
        engine.Play();
        speed = 4.5f;
    }


    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = new Vector2(-speed, 0);
            transform.rotation = Quaternion.Euler(0f, 0f, 90f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = new Vector2(speed, 0);
            transform.rotation = Quaternion.Euler(0f, 0f, -90f * Time.deltaTime);
        }

        rigidbody.velocity = direction;
    }
}

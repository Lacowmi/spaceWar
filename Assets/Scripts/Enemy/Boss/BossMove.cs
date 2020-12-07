using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossMove: MonoBehaviour
{
    public static float speed;
    private Vector2 direction;
    private bool wasFirstBounce = false;
    private bool inField = false;

    void Start()
    {
        if (Spawner.waveIndex > 20)
        {
            speed = 2f;
        }
        else
        {
            speed = 1f;
        }

        direction = Vector2.down;
    }

    void Update()
    {
        if (!inField)
        {
            CheckInField(ref inField);
        }

        if (!wasFirstBounce && inField)
        {
            direction = new Vector2(Random.Range(-3, 3), Random.Range(0, 3));
            wasFirstBounce = true;
        }

        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!BossController.isBossDestroying)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                direction = new Vector2(-direction.x + Random.Range(-1,1), direction.y);
            }
            else if (collision.gameObject.CompareTag("UpDownWall") && inField)
            {
                direction = new Vector2(direction.x, -direction.y + Random.Range(-1, 1));
            }

        }
    }
    private void CheckInField(ref bool inField)
    {
        if (transform.position.y <= (-Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y) / 2)
            inField = true;
    }
}

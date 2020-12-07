using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Vector2 direction;
    private Rigidbody2D rb;
    public static float speed;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        if(Spawner.typesOfWaves.Peek() == 3 || Spawner.typesOfWaves.Peek() == 2)
        {
            if (Spawner.waveIndex < 10)
            {
                speed = 1.5f;
            }
            else if (Spawner.waveIndex < 20)
            {
                speed = 2f;
            }
            else
            {
                speed = 3f;
            }
        }

        gameObject.GetComponent<Animator>().SetBool("isIdleWithMove", false);

        direction = new Vector2(Random.Range(-4, 4), Random.Range(-1, 0));
    }

    void Update()
    {
        if(Spawner.typesOfWaves.Peek() == 5)
        {
            MoveRandom();
        } 
        else if (Spawner.typesOfWaves.Peek() == 4)
        {
            MoveDown();
            gameObject.GetComponent<Animator>().SetBool("isIdleWithMove", true);
        } 
        else if (Spawner.typesOfWaves.Peek() == 3 || Spawner.typesOfWaves.Peek() == 2)
        {
            gameObject.GetComponent<Animator>().SetBool("isIdleWithMove", true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction.x = -direction.x;
        }
    }

    void MoveRandom()
    {
        rb.velocity = direction.normalized * speed * 1.5f;
    }

    void MoveDown()
    {
        rb.velocity = Vector2.down.normalized * speed;
    }
}
